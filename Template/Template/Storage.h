#ifndef __STORAGE__
#define __STORAGE__


#include <iostream>
#include <assert.h>

// forward specialized function 
template <class T>
class Storage;

//specific case
std::ostream& operator << (std::ostream& os, const Storage<double>& storage);

//general case
template <class T>
std::ostream& operator << (std::ostream& os, const Storage<T>& storage);


template <class T>
class Storage
{
private:
	T m_Value;

public:
	Storage(T value)
	{
		m_Value = value;
	}

	~Storage()
	{

	}

	//function template specific specialization
	friend std::ostream& operator << (std::ostream& os, const Storage<double>& storage);

	//fuction template full specialization for a particular T
	friend std::ostream& operator << <> (std::ostream& os, const Storage& storage);
	
	void Print()
	{
		std::cout << m_Value;// << '\n';
	}
};

//std::ostream& operator << (std::ostream& os, const Storage<double>& storage)
//{
//	os << std::scientific << storage.m_Value;
//	return os;
//}

template<>
void Storage<double>::Print()
{
	std::cout << std::scientific << m_Value;
}

std::ostream& operator << (std::ostream& os, const Storage<double>& storage)
{
	std::cout << std::scientific << storage.m_Value;
	return os;
}

template< class T>
std::ostream& operator << (std::ostream& os, const Storage<T>& storage)
{
	std::cout << storage.m_Value;
	return os;
}

template <typename T>
class Storage <T* >
{
private:
	T* m_Value;
public:
	Storage(T* value)
	{
		m_Value = new T();
		*m_Value = *value;
	}

	~Storage()
	{
		if (m_Value != nullptr)
			delete m_Value;
	}

	void Print()
	{
		std::cout << *m_Value;
	}
};

template <>
Storage<char*>::Storage(char* value)
{
	int length = 0;
	while (value[length] != '\0')
		length++;

	m_Value = new char[length + 1];
	for (int i = 0; i < length; ++i)
		m_Value[i] = value[i];
	m_Value[length] = '\0';
}

template <>
Storage<char*>::~Storage()
{
	if (m_Value != nullptr)
		delete[] m_Value;
}

template <class T, int k = 8>
class Storage8
{
private:
	int m_Length = k;
	T m_Array[8];
public:
	void Set(int index, const T& value)
	{
		assert(index >= 0 && index < 8);
		m_Array[index] = value;
	}

	T& Get(int index)
	{
		assert(index >= 0 && index < 8);
		return m_Array[index];
	}

	int GetLength()
	{
		return m_Length;
	}
};

template <>
class Storage8 < bool >
{
private:
	unsigned char m_Data;
public:

	Storage8() : m_Data(0)
	{

	}

	void Set(int index, bool value)
	{
		assert(index >= 0 && index < (sizeof(char) * 8));
		unsigned char mask = 1 << index;
		
		if (value)
			m_Data |= mask;
		else
			m_Data &= ~mask;
	}

	bool Get(int index)
	{
		assert(index >= 0 && index < (sizeof(char) * 8));
		
		unsigned char mask = 1 << index;
		return m_Data & mask;
	}

	int GetLength()
	{
		return sizeof (char) * 8;
	}
};

#endif