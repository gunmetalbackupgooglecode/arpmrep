#include <stdio.h>
#include <windows.h>
#include "netdef.h"

#define BUF_SIZE 1500

static bool isStoped = false;
static SOCKET socket_d;
static DWORD pid = GetCurrentProcessId(); // ICMP message id
static SOCKADDR_IN serv_addr;
static double time_min = 999999999.0;
static double time_max = 0;
static double time_sum = 0;
static int packets_sent = 0;
static int packets_received = 0;

void pinger();
void print_data(char* packet_ptr, int packet_len, SOCKADDR_IN* from_addr_prt);
VOID CALLBACK TimerProc(HWND hwnd, UINT uMsg, UINT_PTR idEvent, DWORD dwTime);
DWORD WINAPI MessageLoopThreadProc(LPVOID lpParameter);
BOOL CtrlHandler(DWORD dwCtrlType);
unsigned short calc_checksum(unsigned short* buf_ptr, int buf_len);


void ping(char* host_str)
{
  // parse hostname
  HOSTENT* host_ptr = gethostbyname(host_str);

  if (NULL == host_ptr)
  {
    printf("gethostbyname() failed. Error code = %d\n", WSAGetLastError());
    return;
  }

  // init socket
  socket_d = socket(PF_INET, SOCK_RAW, IPPROTO_ICMP);
  if (INVALID_SOCKET == socket_d)
  {
    printf("socket() failed. Error code = %d\n", WSAGetLastError());
    return;
  }

  // program will finish its work after ctrl+c
  if (!SetConsoleCtrlHandler((PHANDLER_ROUTINE)CtrlHandler, TRUE))
  {
    printf("SetConsoleCtrlHandler() failed. Error = %d\n", GetLastError());
    closesocket(socket_d);
    return;
  }

  // create timer
  HANDLE hThread = CreateThread(NULL, NULL, MessageLoopThreadProc, NULL, NULL, NULL);
  
  if (NULL == hThread)
  {
    printf("CreateThread() failed. Error = %d\n", GetLastError());
    closesocket(socket_d);
    return;
  }
  CloseHandle(hThread);

  const int on = 1;
  const int buf_size = 60 * 1024;

  setsockopt(socket_d, SOL_SOCKET, SO_BROADCAST, (char*)&on, sizeof(on)); // enable broadcast
  setsockopt(socket_d, SOL_SOCKET, SO_RCVBUF, (char*)&buf_size, sizeof(buf_size)); // enlarge buffer size

  ZeroMemory(&serv_addr, sizeof(serv_addr));
  serv_addr.sin_family = AF_INET;
  serv_addr.sin_addr = *((struct in_addr*)host_ptr->h_addr);

  SOCKADDR_IN from_addr;
  int from_addr_len = sizeof(from_addr);
  char recv_buf[BUF_SIZE];
  int recv_buf_len;
  int socketError;

  // lets get packets
  while (!isStoped)
  {
    recv_buf_len = recvfrom(socket_d, recv_buf, sizeof(recv_buf), 0, (struct sockaddr*)&from_addr, &from_addr_len);
    if (SOCKET_ERROR == recv_buf_len && !isStoped)
    {
      socketError = WSAGetLastError();
      if (WSAEINVAL == socketError)
      {
        continue;
      }
      printf("recvfrom() failed. Error = %d\n", socketError);
      continue;
    }

    print_data(recv_buf, recv_buf_len, &from_addr);
  }
}

static void print_data(char* packet_ptr, int packet_len, struct sockaddr_in* from_addr_ptr)
{
  DWORD recv_time = GetTickCount();
  IPV4_HEADER* ip_ptr = (IPV4_HEADER*)packet_ptr;
  int ip_len = (ip_ptr->ip_verlen & 0xF) << 2; // length in bytes
  ICMP_HEADER* icmp_ptr = (ICMP_HEADER*)(packet_ptr + ip_len);
  int icmplen = packet_len - ip_len;

  // return if it's not our packet
  if (icmp_ptr->icmp_id != pid)
  {
    return;
  }

//  if (ICMP_DEST_UNREACH == icmp_ptr->icmp_type)
//  {
//    printf("From %s icmp_seq=%u Destination Host Unreachable\n",
//           inet_ntoa(from_addr_ptr->sin_addr), icmp_ptr->icmp_seq);
//  }

  if (ICMP_ECHOREPLY == icmp_ptr->icmp_type)
  {
    DWORD send_time = *(DWORD*)icmp_ptr->icmp_data;
    DWORD round_trip_time = recv_time - send_time;

    ++ packets_received;

    time_sum += round_trip_time;
    time_min = min(time_min, round_trip_time);
    time_max = max(time_max, round_trip_time);

    printf("%d bytes from %s: icmp_seq=%u, ttl=%d, time=%d ms\n",
      icmplen, inet_ntoa(from_addr_ptr->sin_addr),
      icmp_ptr->icmp_seq, ip_ptr->ip_ttl, round_trip_time);
  }
}


static void pinger()
{
  char sendbuf[BUF_SIZE];
  ICMP_HEADER* icmp_ptr = (ICMP_HEADER*)sendbuf;

  // init icmp message
  icmp_ptr->icmp_type = ICMP_ECHO;
  icmp_ptr->icmp_code = 0;
  icmp_ptr->icmp_id = (short)pid;
  icmp_ptr->icmp_seq = packets_sent ++;
  *(DWORD*)icmp_ptr->icmp_data = GetTickCount();

  int icmp_len = 8 + 56; // 8 - header, 56 - data

  icmp_ptr->icmp_cksum = 0;
  icmp_ptr->icmp_cksum = calc_checksum((unsigned short*)icmp_ptr, icmp_len);

  int func_res = sendto(socket_d, sendbuf, icmp_len, 0, (struct sockaddr*)&serv_addr, sizeof(serv_addr));

  if (-1 == func_res)
  {
    perror("sendto() failed");
  }
}


static VOID CALLBACK TimerProc(HWND hwnd, UINT uMsg, UINT_PTR idEvent, DWORD dwTime)
{
  pinger();
}


static DWORD WINAPI MessageLoopThreadProc(LPVOID lpParameter)
{
  UINT timerId = SetTimer(NULL, 1, 1000, (TIMERPROC)TimerProc);

  if (0 == timerId)
  {
    printf("SetTimer() failed. Error = %d", GetLastError());
    return 1;
  }

  MSG msg;
  
  while (GetMessage(&msg, NULL, 0, 0))
  {
    DispatchMessage(&msg);
  }

  return 0;
}


static BOOL CtrlHandler(DWORD dwCtrlType) 
{
  if (dwCtrlType != CTRL_C_EVENT && dwCtrlType != CTRL_BREAK_EVENT)
  {
    return FALSE;
  }
  
  isStoped = true;

  printf("\n--- %s ping statistics ---\n", inet_ntoa(serv_addr.sin_addr));
  printf("%d packets transmitted, ", packets_sent);
  printf("%d packets received, ", packets_received);
  if (packets_sent)
  {
    if (packets_received > packets_sent)
    {
      printf("-- somebody's printing up packets!");
    }
    else
    {
      printf("%d%% packet loss", (int)(((packets_sent - packets_received) * 100) / packets_sent));
    }
  }
  printf("\n");
  if (packets_received > 0)
  {
    printf("round-trip min/avg/max = %.3f/%.3f/%.3f ms\n",
      time_min, time_sum / packets_received, time_max);
  }
  fflush(stdout);
  
  closesocket(socket_d);
  
  return FALSE;
}


static unsigned short calc_checksum(unsigned short* buf_ptr, int buf_len)
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
