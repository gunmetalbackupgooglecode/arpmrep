#include "utils.h"

unsigned short calc_checksum(unsigned short* buf_ptr, int buf_len)
{
  unsigned short result;
  unsigned int sum = 0;

  // sum all words
  while (buf_len > 1)
  {
    sum += *buf_ptr ++;
    buf_len -= 2;
  }

  // add last byte if it's needed
  if (1 == buf_len)
  {
    sum += *(unsigned char*)buf_ptr;
  }

  sum = (sum >> 16) + (sum & 0xFFFF);
  sum += (sum >> 16);
  result = ~sum;
  return result;
}
