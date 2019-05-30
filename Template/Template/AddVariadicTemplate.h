//----------------- Recursive Addition ----------------------------------------------
////////////////////////////////////////////////////////////////////////////////////
//base case
template <typename Ts>
Ts Add(Ts t)
{
	std::cout << __FUNCSIG__ << "\n\n";
	return t;
}

//recursive case
template <typename Ts, typename... Rest>
Ts Add(Ts t, Rest... rest)
{
	std::cout << __FUNCSIG__ << std::endl;
	return t + Add(rest...);
}
/////////////////////////////////////////////////////////////////////////////////


//-------------------------- Recursive sum of power ----------------------------------------------
//////////////////////////////////////////////////////////////////////////////////////////
template <typename Ts>
Ts Power(Ts t)
{
	std::cout << __FUNCSIG__ << std::endl;
	return t * t;
}

template <typename Ts>
Ts Sum(Ts t)
{
	std::cout << __FUNCSIG__ << "\n";
	return Power(t);
}

template <typename Ts, typename... Rest>
Ts Sum(Ts t, Rest... rest)
{
	std::cout << __FUNCSIG__ << std::endl;
	return Power(t) + Sum(rest...);
}

//another way to use ...
template <typename Ts>
Ts NewSum(Ts t)
{
	std::cout << __FUNCSIG__ << "\n\n";
	return t;
}

template <typename Ts, typename... Rest>
Ts NewSum(Ts t, Rest... rest)
{
	std::cout << __FUNCSIG__ << std::endl;
	return t + NewSum(Power(rest)...);
}
//////////////////////////////////////////////////////////////////////////////////////////

