/*
	Chapter 8 Exercise 11

	By Marko J. 10/11/2018

	Return a vector containing the length of each string in a string vector.

	Also find the longest and shortest string and the lexicographically first and last string.
*/

#include <iostream>
#include <string>
#include <vector>

using namespace std;

//-------------------------------------------------------------------------------------------------

const string close_window = "~~";

//-------------------------------------------------------------------------------------------------

void keep_window_open(const string& s)
{
	cin.clear();
	cin.ignore(120, '\n');
	string ss = "";
	do {
		cout << "Please enter " << s << " to exit:\n";
	} while (cin >> ss && ss != s);
}

//-------------------------------------------------------------------------------------------------

void compute_strings(const vector<string>& v, vector<int>& results)
{
	for (const string s : v)
		results.push_back(s.size());
}

//-------------------------------------------------------------------------------------------------

void min_s(const vector<string>& v, string& s)
{
	for (unsigned int i = 0; i < v.size(); ++i) {
		unsigned int counter = 0;
		for (unsigned int j = 0; j < v.size(); ++j) {
			if (v[i].size() <= v[j].size()) ++counter;
			if (counter == v.size()) {
				s = v[i];
				return;
			}
		}
	}
}

//-------------------------------------------------------------------------------------------------

void max_s(const vector<string>& v, string& s)
{
	for (unsigned int i = 0; i < v.size(); ++i) {
		unsigned int counter = 0;
		for (unsigned int j = 0; j < v.size(); ++j) {
			if (v[i].size() >= v[j].size()) ++counter;
			if (counter == v.size()) {
				s = v[i];
				return;
			}
		}
	}
}

//-------------------------------------------------------------------------------------------------

void find_last(const vector<string>& v, string& s)
{
	for (unsigned int i = 0; i < v.size(); ++i) {
		unsigned int counter = 0;
		for (unsigned int j = 0; j < v.size(); ++j) {
			if (v[i] >= v[j]) ++counter;
			if (counter == v.size()) {
				s = v[i];
				return;
			}
		}
	}
}

//-------------------------------------------------------------------------------------------------

void find_first(const vector<string>& v, string& s)
{
	for (unsigned int i = 0; i < v.size(); ++i) {
		unsigned int counter = 0;
		for (unsigned int j = 0; j < v.size(); ++j) {
			if (v[i] <= v[j]) ++counter;
			if (counter == v.size()) {
				s = v[i];
				return;
			}
		}
	}
}

//-------------------------------------------------------------------------------------------------

int main()
try {
	vector<string> v = { "hello", "I", "Who" };
	vector<int> results;
	compute_strings(v, results);
	for (const int n : results)
		cout << n << '\n';

	string min = "";
	string max = "";
	string first = "";
	string last = "";

	min_s(v, min);
	max_s(v, max);
	find_first(v, first);
	find_last(v, last);

	cout << "\n\n";

	cout << min << '\n'
		<< max << '\n'
		<< first << '\n'
		<< last << '\n';

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