/*
	Chapter 8 Exercise 12
*/

#include <iostream>
#include <string>
#include <vector>

using namespace std;

const string close_window = "~~";

void keep_window_open(const string& s)
{
	cin.clear();
	cin.ignore(120, '\n');
	string ss = "";
	do {
		cout << "Please enter " << s << " to exit:\n";
	} while (cin >> ss && ss != s);
}

void print_until_ss(const vector<string>& v, const string& quit)
{
	unsigned int counter = 0;
	for (const string s : v) {
		if (s == quit) ++counter;
		if (counter == 2) return;
		cout << s << '\n';
	}
}

int main()
try {
	print_until_ss({ "~!1", "whe", "hi", "whe" }, "whe");
	keep_window_open(close_window);
	return 0;
}
catch (exception& e) {
	cerr << e.what() << '\n';
	keep_window_open(close_window);
	return 1;
}
catch (...) {
	cerr << "Unknown exception\n";
	keep_window_open(close_window);
	return 2;
}