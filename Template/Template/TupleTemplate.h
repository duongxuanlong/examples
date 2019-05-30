template <typename... Ts>
struct MyTuple {
	MyTuple()
	{
		std::cout << __FUNCSIG__ << std::endl;
	}
};


template <typename T, typename... Ts>
struct MyTuple <T, Ts...> : MyTuple <Ts...>
{
	MyTuple(T t, Ts... ts) : MyTuple<Ts...>(ts...)
	{
		std::cout << __FUNCSIG__ << std::endl;
		mTail = t;
	}
	T mTail;
};

template <int, typename... Ts>
struct Elem_Type_Holder {
	Elem_Type_Holder(Ts...)
	{
		std::cout << __FUNCSIG__ << std::endl;
	}
};

template <typename T, typename... Ts>
struct Elem_Type_Holder <0, MyTuple<T, Ts...>>
{
	Elem_Type_Holder()
	{
		std::cout << __FUNCSIG__ << std::endl;
	}
	typedef T type;
};

template <int k, typename T, typename... Ts>
struct Elem_Type_Holder<k, MyTuple<T, Ts...>>
{
	Elem_Type_Holder()
	{
		std::cout << __FUNCSIG__ << std::endl;
	}
	typedef typename Elem_Type_Holder < k - 1, MyTuple<Ts...>>::type type;
};

template <int k, typename... Ts>
typename std::enable_if<k == 0, typename Elem_Type_Holder<0, MyTuple<Ts...>>::type&>::type
get(MyTuple<Ts...>& t)
{
	return t.mTail;
};

template <int k, typename T, typename... Ts>
typename std::enable_if<k != 0, typename Elem_Type_Holder<k, MyTuple<T, Ts...>>::type&>::type
get(MyTuple<T, Ts...>& t)
{
	MyTuple<Ts...>& base = t;
	return get<k - 1>(base);
};
