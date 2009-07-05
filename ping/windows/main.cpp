#include <stdio.h>
#include <Windows.h>

void ping(char* host_str);

int main(int argc, char** argv)
{
  if (argc != 2)
  {
    printf("Usage: %s <hostname>\n", argv[0]);
    return 1;
  }

  // init winsock
  WSADATA wsaData;
  int errorCode = WSAStartup(MAKEWORD(2, 2), &wsaData);

  if (errorCode != 0)
  {
    printf("WSAStartup() failed. Error code = %d\n", errorCode);
    WSACleanup();
    return 1;
  }

  ping(argv[1]);

  // close winsock
  WSACleanup();
  
  return 0;
}
