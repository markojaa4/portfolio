// use the sieve of eratostenes method for finding prime numbers
#include "../../std_lib_facilities.h"
int main()
{
	cout << "Lists prime numbers from 1 to...\n"
		<< "(input max number):\n";
	unsigned int n;
	cin >> n;
	vector<char> marked_numbers;                            // use unsigned char t/f instead of bool true/false, because of bool vector compile-time errors
	for (unsigned int i = 2; i <= n; ++i)
		marked_numbers.push_back('t');
	for (unsigned int i = 2; i <= sqrt(n); ++i)
		if (marked_numbers[i - 2] == 't')
			for (unsigned int j = i * i; j <= n; j += i)
				marked_numbers[j - 2] = 'f';
	cout << "The prime numbers from 1 to " << n << " are:\n";
	for (unsigned int i = 0; i < marked_numbers.size(); ++i)
		if (marked_numbers[i] == 't')
			cout << i + 2 << '\n';
	keep_window_open();
	return 0;
}