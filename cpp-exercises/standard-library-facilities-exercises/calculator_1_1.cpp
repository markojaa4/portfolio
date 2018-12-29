/*
	Written by Marko J. using Stroustrup's "Programming"

	Simple calculator

	v1:
	This version is being modified for further exercise
	This is continuing from the version as it should at the end of Chapter 7 (I also applied steps 1 - 5 of the drill, which deal with debugging and testing)
	I also added return values for functions so the compiler doesn't give warnings

	v1.1:
	Continuing with exercises

	Revision history:
		v1.1:
			Revised by Marko J. - 10/16/2018:
				Made ts to be passed to functions by reference, instead of calling the global variable directly (Chapter 8 Exercise 1)

		v1:
			Revised by Marko J. - 9/8/2018 4:00 AM:
				Improved help and added vector for holding reserved keywords
			Revised by Marko J. - 9/8/2018 1:00 AM:
				Implemented 'list' command to list variables
				Tidied main() by adding more functions
			Revised by Marko J. - 9/7/2018 11:33 PM:
				Added some handling for fatal errors from missing exponents in exponential notation, e.g. '1e'. Now it gives a message but the program still fails to recover
			Revised by Marko J. - 9/6/2018 12:30 AM:
				Made curly braces acceptable as well (Chapter 6 Exercise 2)
				Added factorial operator (Chapter 6 Exercise 3)
			Revised by Marko J. - 9/5/2018 20:00 PM:
				Implemented help command (Chapter 7 Exercise 6)
				Changed the help command from 'h' and 'H' to 'help' (chapter 7 Exercise 7)
			Revised by Marko J. - 9/5/2018 2:30 PM:
				Returned a constant 'let' for '#'
				Allowed newline input to terminate expressions as well (Chapter 7 Exercise 5)
			Revised by Marko J. - 9/5/2018 4:30 AM:
				Implemented constant variable functionality (Chapter 7 Exercise 3)
				Implemented a Symbol_table class for variables (Chapter 7 Exercise 4)
			Revised by Marko J. - 9/5/2018 1:30 AM:
				Implemented assignments (Chapter 7 Exercise 2)
			Revised by Marko J. - 9/5/2018 12:40 AM:
				Moved sqrt and pow to separate rules/functions
			Revised by Marko J. - 9/4/2018 13:30 PM:
				Allowed underscores in variable names (but not at the beginning) (Chapter 7 Exercise 1)
			Revised by Marko J. - 9/4/2018 21:40 PM:
				Implemented pow(x,i);
				Changed the declaration keyword from 'let' to '#' (by eliminating it from the isalfa() recognition part and making an '#' case in Token_stream::get())
				Changed the quit keyword from 'q' to 'quit'
			Revised by Marko J. - 9/4/2018 5:00 AM:
				Started implementing pow(x,i) - edited the grammar and added ',' case to Token_stream::get()
				In declaration(), chanded error("name expected in declaration) to error("proper name expected in declaration)
			Revised by Marko J. - 9/4/2018 4:40 AM:
				Revised the grammar for sqrt() and implemented it through primary instead of using a separate rule/function
				Polished the representation of the grammar in the comments
			Revised by Marko J. - 9/4/2018 3:50 AM:
				Added "print instructions" comment to the part that prints instructions
				Added sqrt() function to the calculator's functionality
			Revised by Marko J. - 9/2/2018 19:00 PM:
				Added k predefined constant
			Revised by Marko J. - 9/2/2018 16:20 PM:
				Added return values to eliminate compiler warnings to Token_stream::get(), get_value() and primary();
			Revised by Marko J. - 9/2/2018 14:50 PM:
				Removed cin >> s from Token_stream::get()'s default case, that I accidentally left behind
				Added '=' case to Token_stream::get()
			Revised by Marko J. - 9/2/2018 12:40 PM:
				Added 'name' case to primary()
			Made by piecing together steps and snippets from Stroustrup's "Programming"
				9/1/2018 10:30 PM

	This program implements a basic expression calculator.
	Input from cin; output to cout.

	The grammar for input is:

		Calculation:
			Statement
			Print
			Quit
			Help
			Names_help
			List_vars
			Calculation Statement
			Statement Calculation
		Statement:
			Declaration
			Const_declaration
			Expression
		Declaration:
			Decl_keyword Name "=" Expression
		Const_declaration:
			Const_keyword Name "=" Expression
		Expression:
			Term
			Expression "+" Term
			Expression "-" Term
		Term:
			Function
			Term "*" Function
			Term "/" Function
			Term "%" Function
		Primary:
			Number
			Name
			Assignment
			Sqrt
			Pow
			"(" Expression ")"
			"-" Primary
			"+" Primary
			"!" Primary
		Sqrt:
			Sqrt_keyword "(" Expression ")"
		Pow:
			Pow_keyword "(" Expression "," Expression ")"
		Assignment:
			Name "=" Expression
		Print:
			";"
		Quit:
			"q"
		Help:
			"h"
		Names_help:
			"v"
		List_vars:
			"L"
		Name:
			"a"
		Decl_keyword:
			"#"
		Const_keyword:
			"const"
		Sqrt_keyword:
			"r"
		Pow_keyword:
			"p"
		Number:
			"floating-point-literal"

	Input comes from cin through the Token_stream called ts.

	NOTES:
		Problem with the following part of Chapter 8 Exercise 1:
			"Also give the Token_stream constructor an istream& parameter so that when we figure out how to make our own istreams 
			(e.g., attached to files), we can use the calculator for those. Hint: Don’t try to copy an istream"
*/



#include "../../std_lib_facilities.h"



const string prompt = "> ";                    // prints when an input is to be taken
const string result = "= ";                    // marks that what follows is a result

const char number = '8';                       // indicate that a token is of kind 'number'
const char name = 'a';                         // indicate that a token is of kind 'name'
const char print = ';';                        // print command
const char let = '#';	                       // indicate the beginning of a declaration

const char quit = 'q';                         // indicate that a token is of kind 'quit'
const string quit_key = "quit";                // keyword for quitting

const char const_var = 'c';                    // indicate that a token is of kind 'constant variable keyword'
const string const_var_key = "const";          // keyword for declaring a constant variable

const char sq_root = 'r';                      // indicate that a token is of kind 'square root keyword'
const string sq_root_key = "sqrt";             // keyword for using a square root function

const char pow_fn = 'p';                       // indicate that a token is of kind 'power keyword'
const string pow_fn_key = "pow";               // keyword for using the x to the power of i function

const char help = 'h';                         // indicate that a token is of kind 'help'
const string help_key = "help";                // help keyword

const char var_help = 'v';                     // indicate that a token is of kind 'variables help'
const string var_help_key = "naming_rules";    // keyword to list naming rules

const char list_vars = 'L';                    // indicate that a token is of kind 'list'
const string list_vars_key = "list";           // keyword to list variables and constants

// -----------------------------------------------------------------------------------------------------

vector<string> keywords = {                    // hold all reserved keywords
	quit_key,
	const_var_key,
	sq_root_key,
	pow_fn_key,
	help_key,
	var_help_key,
	list_vars_key
};

// -----------------------------------------------------------------------------------------------------

class Token {
public:
	char kind;
	double value;
	string name;
	Token(char ch)    // hold kind (for symbols)
		:kind{ ch } { }
	Token(char ch, double val)    // hold kind and value (for numbers)
		:kind{ ch }, value{ val } { }
	Token(char ch, string n)    // hold kind and name (for variables and keywords)
		:kind{ ch }, name{ n } { }
};

// -----------------------------------------------------------------------------------------------------

class Token_stream {
public:
	Token_stream() :full{ false }, buffer{ 0 } { }
	Token get();
	void putback(Token t);
	void ignore(char c);
private:
	bool full{ false };
	Token buffer;
};

// -----------------------------------------------------------------------------------------------------

void Token_stream::ignore(char c)  // flush all input until character c
								   // c represents the kind of Token
{
	// first look in buffer:
	if (full && c == buffer.kind) {
		full = false;
		return;
	}
	full = false;

	// now search input:
	char ch = 0;
	while (cin.get(ch))
		if (ch == c || ch == '\n') return;
}

// -----------------------------------------------------------------------------------------------------

void Token_stream::putback(Token t)
{
	if (full) error("putback() into a full buffer");
	buffer = t;
	full = true;
}

// -----------------------------------------------------------------------------------------------------

Token Token_stream::get()
// reads from cin to generate a token
{
	if (full) {
		full = false;
		return buffer;
	}
	char ch;
	cin.get(ch);
	while (isspace(ch)) {  // eat white spaces and return newline as print token
		if (ch == '\n') return Token{ print };
		cin.get(ch);
	}
	switch (ch) {
	case print:
	case let:
	case '(':
	case ')':
	case '{':
	case '}':
	case '+':
	case '-':
	case '*':
	case '/':
	case '%':
	case '=':
	case ',':
	case '!':
		return Token{ ch };
	case '.':
	case '0': case '1': case '2': case '3': case '4':
	case '5': case '6': case '7': case '8': case '9':
	{
		// putback the first char and read the entire number as double
		cin.putback(ch);
		double val;
		if (!(cin >> val)) error("inappropriate input! missing exponent");       // handle cases such as 123e; will fail to recover
		return Token(number, val);
	}
	default:
		if (isalpha(ch)) {
			// deal with variable names and keywords
			string s;
			s += ch;
			while (cin.get(ch) && (isalpha(ch) || isdigit(ch) || ch == '_')) s += ch;
			cin.putback(ch);
			if (s == help_key) return Token{ help };
			if (s == quit_key) return Token{ quit };
			if (s == sq_root_key) return Token{ sq_root };
			if (s == pow_fn_key) return Token{ pow_fn };
			if (s == const_var_key) return Token{ const_var };
			if (s == list_vars_key) return Token{ list_vars };
			if (s == var_help_key) return Token{ var_help };
			return Token{ name, s };
		}
		error("bad token");
	}
	return Token{ 'n' };  // a nominal return value (to silence compiler warnings)
}

// -----------------------------------------------------------------------------------------------------

Token_stream ts;

// -----------------------------------------------------------------------------------------------------

class Variable {
public:
	string name;
	double value;
	bool is_const;
	Variable(string n, double val, bool c) :name{ n }, value{ val }, is_const{ c } { }
};

// -----------------------------------------------------------------------------------------------------

class Symbol_table {
	// provides a vecror for Variables and members functions to operate on it
public:
	double get(string s);
	void set(string s, double d);
	bool is_declared(string var);
	double declare(string var, double val, bool is_const);
	void list_variables();
private:
	vector<Variable> var_table;
};

// -----------------------------------------------------------------------------------------------------

double Symbol_table::get(string s)
// return the value of the Variable named s
{
	for (const Variable& v : var_table)
		if (v.name == s) return v.value;
	error("get: undefined variable ", s);
	return 0.0;
}

// -----------------------------------------------------------------------------------------------------

void Symbol_table::set(string s, double d)
// set the Variable named s to d
{
	for (Variable& v : var_table)
		if (v.name == s) {
			if (v.is_const == true) error(v.name, " is a constant");
			v.value = d;
			return;
		}
	error("set: undefined variable ", s);
}

// -----------------------------------------------------------------------------------------------------

bool Symbol_table::is_declared(string var)
// is var already in var_table?
{
	for (const Variable& v : var_table)
		if (v.name == var) return true;
	return false;
}

// -----------------------------------------------------------------------------------------------------

double Symbol_table::declare(string var, double val, bool is_const)
// add (var, val) to var_table
{
	if (is_declared(var)) error(var, " declared twice");
	var_table.push_back(Variable{ var, val, is_const });
	return val;
}

// -----------------------------------------------------------------------------------------------------

void Symbol_table::list_variables()
// prints out all declared variables and constants
{
	cout << '\n';
	cout << "-------------------------------------------------------------------\n";
	cout << "Listing all declared variables and constants:\n\n";
	for (const Variable& v : var_table) {
		if (v.is_const) cout << "constant";
		else cout << "variable";
		cout << ": " << v.name << " = " << v.value << '\n';
	}
	cout << '\n';
	cout << "-------------------------------------------------------------------\n\n";
}

// -----------------------------------------------------------------------------------------------------

Symbol_table sym_tab;

double expression(Token_stream&);

// -----------------------------------------------------------------------------------------------------

void print_intro()
{
	cout << "This is a simple expression calculator.\n"
		<< "(type '" << help_key << "' for instructions)\n\n";
}

// -----------------------------------------------------------------------------------------------------

void print_help()
{
	cout << '\n';
	cout << "---------------------------------------------------------------------\n";

	cout << "Help instructions:\n\n\n"

		<< "Simply enter the expressions You wish to calculate.\n\n"

		<< "Terminate expressions with '" << print << "' or press enter.\n\n"

		<< "Available operations are:\n\n"

		<< "    +, -, *, /,\n"
		<< "    % (modulo),\n"
		<< "    ! (factorial),\n"
		<< "    " << sq_root_key << "(x),\n"
		<< "    " << pow_fn_key << "(x,i) - note: 'i' must be a non-negative integer,\n"
		<< "    braces (or curly braces).\n\n"

		<< "Use '" << let << "' to define variables, e.g. '" << let << " x = 123'.\n\n"

		<< "Use '" << const_var_key << "' to define constants, e.g. '" << const_var_key << " y = 123'.\n\n"

		<< "For rules for naming variables and constants, type '" << var_help_key << "'.\n\n"

		<< "For a list of defined variables and constants, type '" << list_vars_key << "'.\n\n"

		<< "Enter '" << quit_key << "' to exit program.\n\n";

	cout << "---------------------------------------------------------------------\n\n";
}

// -----------------------------------------------------------------------------------------------------

void print_keywords()
// print the keywords in the 'keywords' array
{
	for (const string keyword : keywords)
		cout << "    " << keyword << '\n';
}

// -----------------------------------------------------------------------------------------------------

void print_var_help()
// print the rules for naming variables
{
	cout << '\n';
	cout << "---------------------------------------------------------------------\n";

	cout << "Naming rules for constants and variables:\n\n\n"

		<< "Names must start with a letter\n\n"

		<< "Names can be comprised of letters, numbers and underscores (_).\n\n"

		<< "The following reserved keywords can't be used for names:\n\n";

	print_keywords();

	cout << '\n';

	cout << "---------------------------------------------------------------------\n\n";
}

// -----------------------------------------------------------------------------------------------------

double sqrt_rule(Token_stream& ts)
// assume we have seen 'sqrt'
// process 'sqrt(x)' syntax and return the result
// x is an expression
{
	Token t = ts.get();
	if (t.kind != '(') error("'(' expected after ", sq_root_key);
	double d = expression(ts);
	if (d < 0) error("sqrt has no real result for negatives");
	t = ts.get();
	if (t.kind != ')') error(sq_root_key, " not closed properly");
	return sqrt(d);
}

// -----------------------------------------------------------------------------------------------------

double pow_rule(Token_stream& ts)
// assume we have seen 'pow'
// process 'pow(x, i)' syntax and return result
// x and i are expressions but i must be integer
{
	Token t = ts.get();
	if (t.kind != '(') error("'(' expected after ", pow_fn_key);
	double d = expression(ts);
	t = ts.get();
	if (t.kind != ',') error("',' expected when using ", pow_fn_key);
	int i = narrow_cast<int>(expression(ts));
	if (i < 0) error(pow_fn_key, ": exponent must be a non-negative integer");
	t = ts.get();
	if (t.kind != ')') error(sq_root_key, " not closed properly");

	double result = 1;
	if (i > 0)
		for (int j = 1; j <= i; ++j)
			result *= d;
	return result;
}

// -----------------------------------------------------------------------------------------------------

int factorial(double d)
{
	int x = narrow_cast<int>(d);
	int result = 1;
	for (int i = 1; i <= x; ++i)
		result *= i;
	if (result < 1) error("factorial overflow");
	return result;
}

// -----------------------------------------------------------------------------------------------------

double assignment(string var_name)
// assume we have seen 'name ='
{
	double d = expression(ts);
	sym_tab.set(var_name, d);
	return d;
}

// -----------------------------------------------------------------------------------------------------

double primary(Token_stream& ts)
// deal with braces, unary operators, existing variables and functions
{
	Token t = ts.get();
	switch (t.kind) {
	case '(': case '{':
	{
		double d = expression(ts);
		t = ts.get();
		if (t.kind != ')' && t.kind != '}') error("')' expected");
		return d;
	}
	case number:
		return t.value;
	case '-':
		return -primary(ts);
	case '+':
		return primary(ts);
	case '!':
		return factorial(primary(ts));
	case name:
	{
		Token t2 = ts.get();
		if (t2.kind == '=') return assignment(t.name);
		else {
			ts.putback(t2);
			return sym_tab.get(t.name);
		}
	}
	case sq_root:
		return sqrt_rule(ts);
	case pow_fn:
		return pow_rule(ts);
	default:
		error("primary expected");
	}
	return 0.0;
}

// -----------------------------------------------------------------------------------------------------

double term(Token_stream& ts)
// deal with *, / and % operations
{
	double left = primary(ts);
	Token t = ts.get();
	while (true) {
		switch (t.kind) {
		case '*':
			left *= primary(ts);
			t = ts.get();
			break;
		case '/':
		{
			double d = primary(ts);
			if (d == 0) error("divide by zero");
			left /= d;
			t = ts.get();
			break;
		}
		case '%':
		{
			int i1 = narrow_cast<int>(left);
			int i2 = narrow_cast<int>(primary(ts));
			if (i2 == 0) error("%: divide by zero");
			left = i1 % i2;
			t = ts.get();
			break;
		}
		default:
			ts.putback(t);
			return left;
		}
	}
}

// -----------------------------------------------------------------------------------------------------

double expression(Token_stream& ts)
// deal with + and - operations
{
	double left = term(ts);
	Token t = ts.get();
	while (true) {
		switch (t.kind) {
		case '+':
			left += term(ts);
			t = ts.get();
			break;
		case '-':
			left -= term(ts);
			t = ts.get();
			break;
		default:
			ts.putback(t);
			return left;
		}
	}
}

// -----------------------------------------------------------------------------------------------------

double declaration(Token_stream& ts, bool is_const)
// assume we have seen "let"
// handle: name = expression
// declare a variable called "name" with the initial value "expression"
{
	Token t = ts.get();
	if (t.kind != name) error("proper name expected in declaration");
	string var_name = t.name;

	Token t2 = ts.get();
	if (t2.kind != '=') error("'=' missing in declaration of ", var_name);

	double d = expression(ts);
	sym_tab.declare(var_name, d, is_const);
	return d;
}

// -----------------------------------------------------------------------------------------------------

double statement(Token_stream& ts)
// deal with declarations and expressions
{
	Token t = ts.get();
	switch (t.kind) {
	case let:
		return declaration(ts, false);
	case const_var:
		return declaration(ts, true);
	default:
		ts.putback(t);
		return expression(ts);
	}
}

// -----------------------------------------------------------------------------------------------------

void clean_up_mess()
{
	ts.ignore(print);
}

// -----------------------------------------------------------------------------------------------------

void calculate(Token_stream& ts)
// keeps taking input for new calculations until 'quit'
// handles error recovery
{
	while (cin)
		try {
		cout << prompt;
		Token t = ts.get();
		while (t.kind == print) t = ts.get();    // eat the print tokens
		if (t.kind == quit) return;

		if (t.kind == list_vars) sym_tab.list_variables();
		else if (t.kind == var_help) print_var_help();
		else if (t.kind == help) print_help();
		else {
			ts.putback(t);
			cout << result << statement(ts) << '\n';
		}
	}
	catch (exception& e) {
		cerr << e.what() << '\n';
		clean_up_mess();
	}
}

// -----------------------------------------------------------------------------------------------------

void predefine_variables()
{
	sym_tab.declare("k", 1000, true);
	sym_tab.declare("pi", 3.1415926535, true);
	sym_tab.declare("e", 2.7182818284, true);
	sym_tab.declare("golden_ratio", 1.6180339887, true);
}

// -----------------------------------------------------------------------------------------------------

int main()
try {
	predefine_variables();
	print_intro();
	calculate(ts);

	keep_window_open("~~");
	return 0;
}

catch (exception& e) {
	cerr << e.what() << '\n';
	keep_window_open("~~");
	return 1;
}
catch (...) {
	cerr << "Oops! Unknown exception\n";
	keep_window_open("~~");
	return 2;
}