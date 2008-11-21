#include <stdio.h>
#include <Windows.h>


void main_ping(HOSTENT* host_ptr);


int main(int argc, char** argv)
{
  // init winsock
  WSADATA wsaData;
  int errorCode = WSAStartup(MAKEWORD(2,2), &wsaData);

  if (errorCode != 0)
  {
    printf("WSAStartup() failed. Error code = %d\n", errorCode);
    WSACleanup();
    exit(1);
  }

  if (argc != 2)
  {
    printf("Usage: %s <hostname>\n", argv[0]);
    WSACleanup();
    return 1;
  }

  // parse hostname
  HOSTENT* host_ptr = gethostbyname(argv[1]);

  if (NULL == host_ptr)
  {
    printf("gethostbyname() failed. Error code = %d\n", WSAGetLastError());
    WSACleanup();
    return 1;
  }
  
  main_ping(host_ptr);

  // close winsock
  WSACleanup();
  
  return 0;
}
