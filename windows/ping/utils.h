#ifndef UTILS_H_
#define UTILS_H_


#define MIN(x, y) ((x) <= (y) ? (x) : (y))
#define MAX(x, y) ((x) >= (y) ? (x) : (y))


unsigned short calc_checksum(unsigned short* buf_ptr, int buf_len);

#endif /* UTILS_H_ */
