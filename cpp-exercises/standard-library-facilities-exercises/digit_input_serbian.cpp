// Marko J. 23/8/2018
// Stroustrup, Programming, 2nd edition
// Chapter 6 Exercise 9

#include "../../std_lib_facilities.h"

/*
Takes a sequence of digits and evaluates the resulting number
Serbian version
*/

int main()
try {
	vector<string> units = { "jedinica", "desetica", "stotina", "hiljada", "desetina hiljada", "stotina hiljada" };
	vector<string> units_plural = { "jedinice", "desetice", "stotine", "hiljade", "desetine hiljada", "stotine hiljada" };
	if (units.size() != units_plural.size()) error("greska u kodu", "vektori 'units' i 'units_plural' nisu uskladjeni");

	while (true) {
		cout << "---------------------------------------------------\n"
			<< "Unesite niz do sest cifara koje ce biti preracunate.\n"
			<< "Zavrsite unos sa '|'.\n"
			<< "Unesite 'x' ako zelite da izadjete iz programa\n"
			<< "(potom '~' za zatvaranje prozora).\n"
			<< "----------------------------------------------------\n";

		vector<int> digits;
		char digit;

		while (cin >> digit) {
			if (digit < '0' || digit > '9') break;
			digits.push_back(digit - '0');
		}

		if (digit == 'x') break;
		if (digits.size() == 0) error("nema unosa");
		if (digits.size() > units.size()) error("previse cifara");

		// compute the value of the entire number:
		int number = 0;
		for (unsigned int i = 0; i < digits.size(); ++i)
			number = number * 10 + digits[i];

		cout << number << " je"; // output as 'number' as integer

		// loop through all input digits, compute and write out information
		for (unsigned int i = 0; i < digits.size(); ++i) {
			if (digits[i]) { // write things only if digit is not 0
				
				// find the corresponding word for the digit:
				string digit_word;
				switch (digits[i]) {
				case 1:
					digit_word = "";
					break;
				case 2:
					digit_word = "dve ";
					break;
				case 3:
					digit_word = "tri ";
					break;
				case 4:
					digit_word = "cetiri ";
					break;
				case 5:
					digit_word = "pet ";
					break;
				case 6:
					digit_word = "sest ";
					break;
				case 7:
					digit_word = "sedam ";
					break;
				case 8:
					digit_word = "osam ";
					break;
				case 9:
					digit_word = "devet ";
					break;
				}

				// deal with special wordings for last two digits:
				if (i == digits.size() - 2 && digits[i] == 1 && digits[i + 1] != 0) {
					switch (digits[i + 1]) {
					case 1:
						cout << " jedanaest";
						break;
					case 2:
						cout << " dvanaest";
						break;
					case 3:
						cout << " trinaest";
						break;
					case 4:
						cout << " cetrnaest";
						break;
					case 5:
						cout << " petnaest";
						break;
					case 6:
						cout << " petnaest";
						break;
					case 7:
						cout << " sedamnaest";
						break;
					case 8:
						cout << " osamnaest";
						break;
					case 9:
						cout << " devetnaest";
						break;
					}
					break;
				}
				// else compute appropreate output:
				else {
					if (digits[i] == 1 || digits[i] >= 5) cout << ' ' << digit_word << units[digits.size() - i - 1];
					else cout << ' ' << digit_word << units_plural[digits.size() - i - 1];
				}
			}
		}
		cout << '\n';
	}

	keep_window_open("~");
	return 0;
}

catch (exception& e) {
	cerr << "Greska: " << e.what() << '\n';
	keep_window_open("~");
	return 1;
}
catch (...) {
	cerr << "Nepoznata greska\n";
	keep_window_open("~");
	return 2;
}