
#include "TEA.h"
#include "XTEA.h"
#include "XXTEA.h"

#include <iostream>
#include <string>

bool IsBigEndian (int param = 1)
{
	//std::cout << "IsBigEndian: " << *((char*)&param) << std::endl;
	return *((char*) &param) == 0;
}

int main(int count, char** args)
{
	char* c;
	short int* si;
	int* i;
	float* f;
	double* d;
	int param = 32;//64;
	std::cout << "hexa of 64 is: " << (param >> 5) << std::endl;
	std::cout << "Size of address c: " << sizeof (c) << std::endl;
	std::cout << "Size of address si: " << sizeof (si) << std::endl;
	std::cout << "Size of address i: " << sizeof (i) << std::endl;
	std::cout << "Size of address f: " << sizeof (f) << std::endl;
	std::cout << "Size of address d: " << sizeof (d) << std::endl;
	std::cout << "Is BigEndian: " << IsBigEndian() << std::endl;
	//Temporarily used this hardcode keys
	unsigned int keys[4] = { 0xB5D1, 0x22BA, 0xC2BC, 0x9A4E };

	std::string plainText = "Encrypt this text with XXTEA algorithm. This text isn't enough length. So, you have to put something else here to make it long enough";

	std::cout << "Text before encryption: " << plainText << " and size: " << plainText.size() << std::endl;

	//int len = plainText.size();
	////if (len >= BLOCK_SIZE)
	//	len += len % BLOCK_SIZE;
	//plainText.resize(len);
	//for (int i = 0; i < len; i += BLOCK_SIZE)
	//{
	//	//TEA::Encrypt(reinterpret_cast<uint32_t*> (&plainText[i]), keys);
	//	XTEA::Encrypt(reinterpret_cast<uint32_t*>(&plainText[i]), keys);
	//}
	char s[128] = "hello world bla bla bla bla bla bla";
	int length = plainText.size();
	int encodedlength = (length & 3) ? ((length & (~3)) + 4) : length;
	char* temp = new char[encodedlength + 1];
	temp[encodedlength] = '\0';
	memcpy(temp, plainText.c_str(), length);
	if (encodedlength != length)
	{
		memset(temp + length, 0, encodedlength - length);
		std::cout << "string with padding: " << temp << std::endl;
	}
	//temp[length] = '\0';
	int n = encodedlength >> 2;// / sizeof (uint32_t);

	//int n = plainText.size() / sizeof (uint32_t);
	
	XXTEA::Encrypt(reinterpret_cast<uint32_t*> (temp), n, keys);
	
	std::cout << "Text after encryption: " << temp << std::endl;

	// unused
	//std::string cipherText = plainText;
	//len = cipherText.size();
	////if (len >= BLOCK_SIZE)
		//len += len % BLOCK_SIZE;
	//cipherText.resize(len);
	// end unused
	//for (int i = 0; i < len; i += BLOCK_SIZE)
	//{
	//	//TEA::Decrypt(reinterpret_cast<uint32_t*>(&plainText[i]), keys);
	//	XTEA::Decrypt(reinterpret_cast<uint32_t*>(&plainText[i]), keys);
	//}
	
	int newlength = strlen(temp);
	int newn = newlength >> 2;
	XXTEA::Decrypt(reinterpret_cast<uint32_t*>(temp), newn, keys);
	std::cout << "Text after decryption: " << temp << std::endl;

	//uint32_t values[2];
	//memset(values, 0x0, 8);
	//const char* indata = plainText.c_str();
	//memcpy(static_cast<void*>(values), indata, 8);
	//TEA::Encrypt(values, keys);
	//std::cout << "Text after encryption: " << values[0] << " and " << values[1] << std::endl;

	//uint32_t devalues[2];
	//memset(devalues, 0x0, 8);
	////memset(devalues, values, 8);

	//TEA::Decrypt(values, keys);
	// 
	//std::cout << "Text after decryption: " << (char*)(values) << std::endl;

	//std::cout << "size of uint32_t: " << sizeof(uint32_t) << std::endl;
	std::getchar();

	return 0;
}