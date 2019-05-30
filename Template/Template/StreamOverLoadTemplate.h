#include <iostream>
#include <initializer_list>
#include <functional>

template <typename... Args>
void PrintAll_01(std::ostream& os, const Args&... args)
{
	std::cout << __FUNCSIG__ << std::endl;
	std::initializer_list < int > 
	{
		(os << args << "\t", 0)...
	};

}

template<typename...Ts>
void print_all(std::ostream& stream, const Ts&... args) {
	(void)std::initializer_list<int>
	{
		([&](const Ts& arg){stream << arg; return 0; }(args)...);
	};
}

template <typename... Args>
void PrintAll_02(std::ostream& os, const Args&... args)
{
	std::cout << __FUNCSIG__ << std::endl;

	//std::function<int(const Arg& arg)> newfunction = [&](/*std::ostream& o,*/ const Arg& arg) -> int
	//{
	//	os << arg << "\t";
	//	return 0;
	//};

	/*std::initializer_list < int >
	{
		[&] 
		{
			std::cout << args << "\t";
			return 0;
		}(args)...;
	};*/

	[&](const Args& arg)
	{
		os << arg << "\t";
	}(args)...;
}