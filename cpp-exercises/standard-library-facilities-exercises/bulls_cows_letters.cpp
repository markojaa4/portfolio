// Marko J. 8/15/2018
// Chapter 5 Exercise 12 and 13
// and
// Chapter 6 Exercise 8

/*
Bulls and Cows game
Guess a sequence of four unique letters
1 cow for each correct letter not in its place
1 bull for each correct letter in correct position
*/

#include "../../std_lib_facilities.h"


/*
Ask for an integer to use for seed to generate the characters.
Read a guess, compare it to target sequence and output the number of bulls and cows.
Repeat until the user guesses correctly, then start a new game.

NOTE: Better to use current time as seed instead.
*/


int main()
try {
	cout << "-Bulls and Cows-\n";
	for (;;) {
		// start new game and set the seed:
		cout << "Enter a random integer number to start a new game:\n";
		int seed = 0;
		if (!(cin >> seed)) error("invalid input");
		srand(seed);
		cout << "Guess a sequence of four unique letters (a - z).\n"
			<< "Use lower-case letters only.\n";

		// generate the target sequence
		vector<char> target;
		for (unsigned int i = 1; i <= 4; ++i) {
			char random = (rand() % 26) + 97;
			for (unsigned int j = 0; j < target.size(); ++j) // makes sure the value that is to be added is unique
				if (target[j] == random) {
					j = 0; // resets the loop to check again
					random = (rand() % 26) + 97;
				}
			target.push_back(random);
		}
		// compute each guess:
		vector<char> guess;
		while (guess != target) {
			// reset the values for each new iteration:
			guess = {};
			int bulls = 0;
			int cows = 0;

			// read in the user's guess:
			for (char val = 0; guess.size() < 4 && cin >> val;) {
				if (val)
				if (int(val) < 97 || int(val) > 122) error("invalid input", "; use lower-case letters only!");
				guess.push_back(val);
			}

			if (guess.size() != target.size()) error("invalid input"); // handle incomplete guess vectors (mainly caused by non-integer input)

																	   // Handle guesses that contain repeated digits:
			for (unsigned int i = 0; i < guess.size(); ++i) {
				int repeated = -1; // will increase to 0 because any enement will at least match itself
				for (unsigned int j = 0; j < guess.size(); ++j)
					if (guess[i] == guess[j]) ++repeated;
				if (repeated > 0) error("invalid input", "; sequence can't contain repeated digits!");
			}

			// compute the guess:
			for (unsigned int i = 0; i < guess.size(); ++i) {
				if (guess[i] == target[i]) bulls++;
				for (unsigned int j = 0; j < target.size(); ++j)
					if (guess[i] == target[j]) cows++;
			}
			cows -= bulls;

			// write out feedback
			if (guess != target) {
				cout << bulls << " bull";
				if (bulls != 1) cout << 's';
				cout << " and " << cows << " cow";
				if (cows != 1) cout << 's';
				cout << '\n';
			}
		}
		if (guess == target) cout << "Correct!\n";
	}
	return 0;
}

catch (out_of_range& e) {
	cerr << e.what() << '\n';
	keep_window_open("~");
	return 1;
}
catch (runtime_error& e) {
	cerr << "runtime error: " << e.what() << '\n';
	keep_window_open("~");
	return 2;
}
catch (exception& e) {
	cerr << "error: " << e.what() << '\n';
	keep_window_open("~");
	return 3;
}
catch (...) {
	cerr << "Oops! Unknown exceptio";
	keep_window_open("~");
	return 4;
}