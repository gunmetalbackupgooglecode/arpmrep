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
