// Sean Anderson
// CS 151, 1/22/2017
// CH10, PC2
/*
Modify the program of Programming Challenge 1 to allow the user to enter name-score
pairs. For each student taking a test, the user types a string representing the name of the
student, followed by an integer representing the student's score. Modify both the sorting
and average-calculating functions so they take arrays of structures, with each structure
containing the name and score of a single student. In traversing the arrays, use pointers
rather than array indices.
*/

#include <iostream>
#include <string>
#include <algorithm>
using namespace std;

struct Student
{
	string name;
	double grade;

	bool operator >(const Student& other)
	{
		return (grade > other.grade);
	}

	bool operator <(const Student& other)
	{
		return (grade < other.grade);
	}

	bool operator >=(const Student& other)
	{
		return (grade >= other.grade);
	}

	bool operator <=(const Student& other)
	{
		return (grade <= other.grade);
	}

	bool operator ==(const Student& other)
	{
		return (grade == other.grade && name == other.name);
	}

	bool operator !=(const Student& other)
	{
		return (grade != other.grade || name != other.name);
	}
};

// Function Prototypes
double average(Student *, int);
void sort(Student*, int);

int main()
{
	int numberOfScores;
	Student *testScores;     // Pointer to dynamically allocated array of scores
	double ave;              // Average scores

							 // Determine number of scores and allocate array
	cout << "Number of scores that you will input: ";
	cin >> numberOfScores;
	testScores = new Student[numberOfScores];

	// Get scores from the user
	for (int count = 0; count < numberOfScores; count++)
	{
		cout << "Student #" << (count + 1) << endl;
		cout << "    Name: ";
		//cin >> testScores[count].name;
		cin >> (testScores + count)->name;

		do {
			cout << "    Score: ";
			cin >> (testScores + count)->grade;

			if ((testScores + count)->grade < 0)
			{
				cout << "Please only enter scores greater than 0.";
			}
		} while ((testScores + count)->grade < 0);
	}

	// Echo scores back to user
	cout << "\nThe test scores that you entered: ";
	for (int count = 0; count < numberOfScores; count++)
	{
		cout << "\n Test #" << (count + 1) << ": " << (testScores + count)->name << ": " << (testScores + count)->grade;
	}

	// Sort students based on their grade using a bubble sort function
	//sort(testScores, numberOfScores);
	std::sort(testScores, testScores + numberOfScores);

	// Print the sorted scores
	cout << "\n\nThe test scores after being sorted: ";
	for (int count = 0; count < numberOfScores; count++)
	{
		cout << " " << (testScores + count)->grade;
	}

	// Computer average
	ave = average(testScores, numberOfScores);

	// Print the output
	cout << "\nThe average test score was: " << ave << endl;
	
	cout << "Press any key to exit.." << endl;
	cin >> ave;

	// Delete the array
	delete[] testScores;
	testScores = 0;
	return 0;
}

double average(Student *testscores, int numberOfScores)
{
	double total = 0;

	for (int count = 0; count < numberOfScores; count++)
	{
		total += (testscores + count)->grade;
	}

	return total / numberOfScores;
}

//This is a simple bubble sort that sorts the students by their grades.
void sort(Student *students, int nbrScores)
{
	Student temp;
	for (int i = 0; i < nbrScores; i++)
	{
		for (int j = 0; j < nbrScores; j++)
		{
			if ((students + i)->grade > (students + j)->grade)
			{
				temp = *(students + i);
				*(students + i) = *(students + j);
				*(students + j) = temp;
			}
		}
	}
}
