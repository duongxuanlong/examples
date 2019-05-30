#ifndef __XXTEA__
#define __XXTEA__

#include <stdint.h>

class XXTEA
{
public:
	static void Encrypt(uint32_t* v, int n, const uint32_t key[4]);
	static void Decrypt(uint32_t* v, int n, const uint32_t key[4]);
};

#endif