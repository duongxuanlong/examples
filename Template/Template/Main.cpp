#include <iostream>
#include <string>

#include "AddVariadicTemplate.h"
#include "StreamOverLoadTemplate.h"
#include "TupleTemplate.h"

#include "InitArray.h"
#include "Storage.h"

struct Test
{
	Test()
	{

	}
}; 

int main(int count, char** args)
{
	int *aa, bb;

	int cc = 10;

	bb = 20;

	std::cout << "Hello Template" << "\n\n";

	int a = 10;

	auto&& b = a;

	

	//std::cout << __PRETTY_FUNCTION__ << std::endl;

	//std::cout << __EDG_VERSION__ << std::endl;

	//std::cout << __FUNCSIG__ << std::endl;

	//Simple addition
	//std::cout << Add(3, 2.0, 5.0) << std::endl;

	//Sum of power
	/*std::cout << Sum(2, 4, 6) << "\n\n";

	std::cout << NewSum(2, 4, 6) << std::endl;*/

	//PrintAll_01(std::cout, 3, 4, 5);

	//PrintAll_02(std::cout, 3, 4, 5);

	//Tuple implementation
	/*MyTuple<int, char, std::string> t1(2, 'a', "who I am");

	std::cout << get<2>(t1) << std::endl;
	*/

	/*InitArray<int> intArray(12);
	InitArray<double> doubleArray(12);

	for (int i = 0; i < intArray.GetLength(); ++i)
	{
		intArray[i] = i;
		doubleArray[i] = i + 0.5;
	}

	for (int i = 0; i < intArray.GetLength(); ++i)
		std::cout << intArray[i] << '\t' << doubleArray[i] << std::endl;

	StaticArray<int, 12> staticArray;
	for (int i = 0; i < 12; ++i)
		staticArray[i] = i;

	for (int i = 11; i >= 0; --i)
		std::cout << staticArray[i] << ' ';
	std::cout << '\n';

	StaticArray<double, 4> dArray;
	for (int i = 0; i < 4; ++i)
		dArray[i] = 4.4 + 0.1 * i;

	for (int i = 0; i < 4; ++i)
		std::cout << dArray[i] << ' ';
	std::cout << std::endl;*/

	/*Storage<int> intValue(7);
	Storage<double> doubleValue(4.5);

	std::cout << "int Value: " << intValue << '\n';
	std::cout << "double value: " << doubleValue << '\n';

	char* mystring = new char[40];

	std::cout << "Enter your name: ";
	std::cin >> mystring;

	Storage<char*> charValue(mystring);

	delete[] mystring;

	std::cout << "Char value is: " << charValue << std::endl;

	std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');*/

	/*Storage8<int> intStorage;
	for (int i = 0; i < intStorage.GetLength(); ++i)
		intStorage.Set(i, i);
	for (int i = 0; i < intStorage.GetLength(); ++i)
		std::cout << intStorage.Get(i) << "\n\n";

	Storage8<bool> boolStorage;
	for (int i = 0; i < boolStorage.GetLength(); ++i)
		boolStorage.Set(i, i & 3);
	for (int i = 0; i < boolStorage.GetLength(); ++i)
		std::cout << (boolStorage.Get(i) ? "true" : "false") << std::endl;*/

	StaticArrayBase<int, 6> intArray;
	for (int i = 0; i < 6; ++i)
		intArray[i] = i;
	//intArray.Print();
	std::cout << intArray;

	StaticArrayBase<double, 4> doubleArray;
	for (int i = 0; i < 4; ++i)
		doubleArray[i] = (4 + 0.5);
	//doubleArray.Print();
	std::cout << doubleArray;

	//Test a;

	std::getchar();

	return 0;
}