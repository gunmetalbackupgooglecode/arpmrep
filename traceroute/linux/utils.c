#include "utils.h"

void time_sub(struct timeval* time1_out, struct timeval* time2)
{
  if ((time1_out->tv_usec -= time2->tv_usec) < 0)
  {
    -- time1_out->tv_sec;
    time1_out->tv_usec += 1000000;
  }
  time1_out->tv_sec -= time2->tv_sec;
}

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

  sum = (sum >> 16) + (sum & 0xFFFF); /* добавляем перенос */
  sum += (sum >> 16);                 /* еще раз */
  result = ~sum;                      /* инвертируем результат */
  return result;
}
