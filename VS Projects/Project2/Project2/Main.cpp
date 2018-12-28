#include <iostream>
#include <string>
using namespace std;

class EncryptString
{
public:
	string str;

	void encrypted()
	{
		for (unsigned int i = 0; i<str.length(); i++)
		{
			str[i]++;
		}
		cout << "The string after encryption: " << str << endl;
	}
};

int main()
{
	EncryptString mainStr;

	cout << "Enter a string: ";
	getline(cin, mainStr.str);
	mainStr.encrypted();

	while (true);
	return 0;
}