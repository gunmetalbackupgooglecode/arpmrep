#include <stdio.h>
#include <stdlib.h>
#include <netdb.h>


void main_udp(struct hostent* host_ptr);


int main(int argc, char** argv)
{
  if (argc != 2)
  {
    fprintf(stderr, "Usage: %s <hostname>\n", argv[0]);
    exit(-1);
  }

  struct hostent* host_ptr = gethostbyname(argv[1]);

  if (NULL == host_ptr)
  {
    herror("gethostbyname() failed");
    exit(-1);
  }

  main_udp(host_ptr);

  return 0;
}
