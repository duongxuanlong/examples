#ifndef __TEA__
#define __TEA__

#include <stdint.h>

//This is the original version of TEA algorithm
#define BLOCK_SIZE 8

class TEA
{
public:
	static void Encrypt(uint32_t data[2], uint32_t keys[4]);
	static void Decrypt(uint32_t data[2], uint32_t keys[4]);
};

#endif