// Marko J. 23/8/2018
// Stroustrup, Programming, 2nd edition
// Chapter 6 Exercise 9

#include "../../std_lib_facilities.h"

/*
	Takes a sequence of digits and evaluates the resulting number
*/

int main()
try {
	vector<string> units = { "one", "ten", "hundred", "thousand", "ten thousand", "hundred thousand" };

	while (true) {
		cout << "Please enter a sequence of up to four digits to evaluate.\n"
			<< "Terminate with '|'. Exit with 'x'\n";

		vector<int> digits;
		char digit;

		while (cin >> digit) {
			if (digit < '0' || digit > '9') break;
			digits.push_back(digit - '0');
		}

		if (digit == 'x') break;
		if (digits.size() == 0) error("no input");
		if (digits.size() > units.size()) error("too many digits");

		int number = 0;
		for (unsigned int i = 0; i < digits.size(); ++i)
			number = number * 10 + digits[i];

		cout << number << " is";

		for (unsigned int i = 0; i < digits.size(); ++i) {
			if (digits[i]) {
				string digit_word;
				switch (digits[i]) {
				case 1:
					digit_word = "";
					break;
				case 2:
					digit_word = "two ";
					break;
				case 3:
					digit_word = "three ";
					break;
				case 4:
					digit_word = "four ";
					break;
				case 5:
					digit_word = "five ";
					break;
				case 6:
					digit_word = "six ";
					break;
				case 7:
					digit_word = "seven ";
					break;
				case 8:
					digit_word = "eight ";
					break;
				case 9:
					digit_word = "nine ";
					break;
				}

				if (i == digits.size() - 2 && digits[i] == 1 && (digits[i + 1] == 1 || digits[i + 1] == 2)) {
					if (digits.size() > 2) cout << " and";
					if (digits[i + 1] == 1) cout << " eleven";
					if (digits[i + 1] == 2) cout << " twelve";
					break;
				}
				else {
					cout << ' ' << digit_word << units[digits.size() - i - 1];
					if (digits[i] > 1) cout << 's';
				}
			}
			if (i == digits.size() - 2 && digits[i + 1] != 0) cout << " and";
		}
		cout << '\n';
	}

	keep_window_open("~");
	return 0;
}

catch (exception& e) {
	cerr << "Error: " << e.what() << '\n';
	keep_window_open("~");
	return 1;
}
catch (...) {
	cerr << "Unknown exception\n";
	keep_window_open("~");
	return 2;
}