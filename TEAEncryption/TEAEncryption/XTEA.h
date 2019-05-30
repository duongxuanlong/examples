#ifndef __XTEA__
#define __XTEA__

#include <stdint.h>

class XTEA
{
public:
	void static Encrypt(uint32_t v[2], uint32_t k[4]);
	void static Decrypt(uint32_t v[2], uint32_t k[4]);
};

#endif