#include "TEA.h"


void TEA::Encrypt(uint32_t v[2], uint32_t k[4])
{
	//cache data
	uint32_t v0 = v[0];
	uint32_t v1 = v[1];

	//cache keys
	uint32_t k0 = k[0], k1 = k[1], k2 = k[2], k3 = k[3];
	
	//magic constant, 2654435769 or 0x9E3779B9 is chosen to be is the golden ratio.
	uint32_t delta = 0x9E3779B9;

	uint32_t sum = 0; //added to delta
	uint32_t passes = 32; //32 passes for 64 rounds

	uint32_t pass = passes;
	while (pass > 0)
	{
		sum += delta;

		v0 += ((v1 << 4) + k0) ^ (v1 + sum) ^ ((v1 >> 5) + k1);
		v1 += ((v0 << 4) + k2) ^ (v0 + sum) ^ ((v0 >> 5) + k3);

		--pass;
	}
	v[0] = v0;
	v[1] = v1;

	//uint32_t v0 = v[0], v1 = v[1], sum = 0, i;           /* set up */
	//uint32_t delta = 0x9E3779B9;                     /* a key schedule constant */
	//uint32_t k0 = k[0], k1 = k[1], k2 = k[2], k3 = k[3];   /* cache key */
	//for (i = 0; i<32; i++) {                         /* basic cycle start */
	//	sum += delta;
	//	v0 += ((v1 << 4) + k0) ^ (v1 + sum) ^ ((v1 >> 5) + k1);
	//	v1 += ((v0 << 4) + k2) ^ (v0 + sum) ^ ((v0 >> 5) + k3);
	//}                                              /* end cycle */
	//v[0] = v0; v[1] = v1;
}

void TEA::Decrypt(uint32_t v[2], uint32_t k[4])
{
	//cache data
	uint32_t v0 = v[0];
	uint32_t v1 = v[1];

	//cache keys
	uint32_t k0 = k[0], k1 = k[1], k2 = k[2], k3 = k[3];

	//magic constant, 2654435769 or 0x9E3779B9 is chosen to be is the golden ratio.
	uint32_t delta = 0x9E3779B9;

	uint32_t sum = 0xC6EF3720; //32 * delta
	uint32_t passes = 32; //32 passes for 64 rounds

	for (uint32_t i = 0; i < passes; ++i)
	{
		v1 -= ((v0 << 4) + k2) ^ (v0 + sum) ^ ((v0 >> 5) + k3);
		v0 -= ((v1 << 4) + k0) ^ (v1 + sum) ^ ((v1 >> 5) + k1);

		sum -= delta;
	}

	v[0] = v0;
	v[1] = v1;

	//uint32_t v0 = v[0], v1 = v[1], sum = 0xC6EF3720, i;  /* set up; sum is 32*delta */
	//uint32_t delta = 0x9E3779B9;                     /* a key schedule constant */
	//uint32_t k0 = k[0], k1 = k[1], k2 = k[2], k3 = k[3];   /* cache key */
	//for (i = 0; i<32; i++) {                         /* basic cycle start */
	//	v1 -= ((v0 << 4) + k2) ^ (v0 + sum) ^ ((v0 >> 5) + k3);
	//	v0 -= ((v1 << 4) + k0) ^ (v1 + sum) ^ ((v1 >> 5) + k1);
	//	sum -= delta;
	//}                                              /* end cycle */
	//v[0] = v0; v[1] = v1;
}