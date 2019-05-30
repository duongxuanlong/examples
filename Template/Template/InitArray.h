#ifndef __INITARRAY__
#define __INITARRAY__

#include <assert.h>
#include <iostream>

template <typename T>
class InitArray
{
private:
	int m_Length;
	T* m_Data;
public:
	InitArray()
	{
		m_Length = 0;
		m_Data = nullptr;
	}

	InitArray(int length)
	{
		m_Length = length;
		m_Data = new T[m_Length];
	}

	~InitArray()
	{
		if (m_Data != nullptr)
			delete[] m_Data;
	}

	void Erase()
	{
		if (m_Data != nullptr)
			delete[] m_Data;

		m_Data = nullptr;
		m_Length = 0;
	}

	T& operator[] (int index)
	{
		assert(index >= 0 && index <= m_Length);
		return m_Data[index];
	}

	int GetLength();
};

template <typename T>
int InitArray<T>::GetLength()
{
	return m_Length;
}

template <class T, int k>
class StaticArrayBase;

template <int k>
std::ostream&  operator << (std::ostream& os, const StaticArrayBase<double, k>& param);

template <class T, int k>
std::ostream& operator << (std::ostream& os, const StaticArrayBase<T, k>& param);

template <class T, int k>
class StaticArrayBase
{
protected:
	T m_Data[k];
	int m_Length = k;

public:
	T* GetArray()
	{
		return m_Data;
	}

	T& operator [] (int index)
	{
		assert(index >= 0 && index <= m_Length);
		return m_Data[index];
	}

	//specific case
	template <int kk>
	friend std::ostream& operator << (std::ostream& os, const StaticArrayBase<double, kk>& param);

	//Generic case
	friend std::ostream& operator << <>(std::ostream& os, const StaticArrayBase& param);

	virtual void Print()
	{
		for (int i = 0; i < m_Length; ++i)
			std::cout << m_Data[i] << ' ';
		std::cout << std::endl;
	}
};

template <int k>
std::ostream& operator << (std::ostream& os, const StaticArrayBase<double, k>& param)
{
	for (int i = 0; i < param.m_Length; ++i)
		std::cout << std::scientific << param.m_Data[i] << ' ';
	std::cout << std::endl;
	return os;
}

template <class T, int k>
std::ostream& operator << (std::ostream& os, const StaticArrayBase<T, k>& param)
{
	for (int i = 0; i < param.m_Length; ++i)
		std::cout << param.m_Data[i] << ' ';
	std::cout << std::endl;
	return os;
}


template < class T, int k >
class StaticArray : public StaticArrayBase < T, k >
{
public:
	StaticArray()
	{

	}
};

template <int k>
class StaticArray<double, k> : public StaticArrayBase<double, k>
{
public :
	StaticArray()
	{

	};

	virtual void Print() override
	{
		for (int i = 0; i < m_Length; ++i)
			std::cout << std::scientific << m_Data[i] << ' ';
		std::cout << std::endl;
	}
};

#endif