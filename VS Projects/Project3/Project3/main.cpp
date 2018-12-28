#include <iostream>
#include <string>

using namespace std;

int main()
{
	int mainMenuChoice;
	string stringInput;

	cout << "-----------------------------------" << endl;
	cout << "Please select an option:" << endl;
	cout << "1 - Add a new Trophy" << endl;
	cout << "2 - Copy a Trophy" << endl;
	cout << "3 - Delete a Trophy" << endl;
	cout << "4 - Rename a Trophy" << endl;
	cout << "5 - Change the level of a Trophy" << endl;
	cout << "6 - Change the color of a Trophy" << endl;
	cout << "7 - Print ALL the Trophies" << endl;
	cout << "8 - Exit the program" << endl;
	cout << "-----------------------------------" << endl;

	do
	{
		cin.clear();
		cin.ignore(256, '\n');
		cout << "Please enter a number between 1 - 8." << endl;
	} while (!(cin >> mainMenuChoice));


	do
	{
		cin.clear();
		cin.ignore(256, '\n');
		cout << "Please enter a string." << endl;
	} while (!(cin >> stringInput));

	cout << "You have entered: " << mainMenuChoice << " for the main menu choice, and: " << stringInput << " for the string." << endl;

	while (true);
	return 0;
}