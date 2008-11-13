#ifndef UTILS_H_
#define UTILS_H_

#include <sys/time.h>


#define MIN(x, y) ((x) <= (y) ? (x) : (y))
#define MAX(x, y) ((x) >= (y) ? (x) : (y))


void time_sub(struct timeval* time1_out, struct timeval* time2);

#endif /* UTILS_H_ */
