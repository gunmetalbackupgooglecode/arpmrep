#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <errno.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/ip.h>
#include <netinet/ip_icmp.h>
#include <arpa/inet.h>
#include <netdb.h>
#include <sys/time.h>
#include <signal.h>
#include <unistd.h>

#include "utils.h"

#define BUF_SIZE 1500

int socket_d;
pid_t pid;
struct sockaddr_in serv_addr;

double time_min = 999999999.0;
double time_max = 0;
double time_sum = 0;
int packets_sent = 0;
int packets_received = 0;


void pinger();
void print_data(char* packet_ptr, int packet_len, struct sockaddr_in* from_addr_prt);
void catcher(int signum);


void main_ping(struct hostent* host_ptr)
{
  // init socket
  socket_d = socket(PF_INET, SOCK_RAW, IPPROTO_ICMP);
  if (socket_d < 0)
  {
    perror("socket() failed");
    exit(-1);
  }

  const int on = 1;
  const int buf_size = 60 * 1024;

  setsockopt(socket_d, SOL_SOCKET, SO_BROADCAST, &on, sizeof(on)); // enable broadcast
  setsockopt(socket_d, SOL_SOCKET, SO_RCVBUF, &buf_size, sizeof(buf_size)); // enlarge buffer size

  // return initial rights
  setuid(getuid());

  // pid - icmp message id
  pid = getpid();

  // init signal handler
  struct sigaction act;

  bzero(&act, sizeof(act));
  act.sa_handler = &catcher;
  sigaction(SIGALRM, &act, NULL);
  sigaction(SIGINT, &act, NULL);

  // init timer
  struct itimerval timer;

  timer.it_value.tv_sec = 0; // start it in 1 microsecond
  timer.it_value.tv_usec = 1;
  timer.it_interval.tv_sec = 1; // activate it every 1 second
  timer.it_interval.tv_usec = 0;
  setitimer(ITIMER_REAL, &timer, NULL);

  bzero(&serv_addr, sizeof(serv_addr));
  serv_addr.sin_family = AF_INET;
  serv_addr.sin_addr = *((struct in_addr*)host_ptr->h_addr);

  struct sockaddr_in from_addr;
  unsigned int from_addr_len = sizeof(from_addr);
  char recv_buf[BUF_SIZE];
  int recv_buf_len;

  // lets get packets
  while (1)
  {
    recv_buf_len = recvfrom(socket_d, recv_buf, sizeof(recv_buf), 0, (struct sockaddr*)&from_addr, &from_addr_len);
    if (recv_buf_len < 0)
    {
      if (EINTR == errno)
      {
        continue;
      }
      perror("recvfrom() failed");
      continue;
    }

    print_data(recv_buf, recv_buf_len, &from_addr);
  }
}


void print_data(char* packet_ptr, int packet_len, struct sockaddr_in* from_addr_ptr)
{
  struct timeval recv_time; // current time

  gettimeofday(&recv_time, NULL);

  struct ip* ip_ptr = (struct ip*)packet_ptr; // ip header
  int ip_len = ip_ptr->ip_hl << 2; // length in bytes
  struct icmp* icmp_ptr = (struct icmp*)(packet_ptr + ip_len); // icmp header
  int icmplen = packet_len - ip_len;

  if (icmplen < 8)
  {
    fprintf(stderr, "icmplen (%d) < 8", icmplen);
  }

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
    struct timeval* send_time_ptr = (struct timeval *) icmp_ptr->icmp_data;

    time_sub(&recv_time, send_time_ptr);

    double round_trip_time = recv_time.tv_sec * 1000.0 + recv_time.tv_usec / 1000.0;

    ++ packets_received;

    time_sum += round_trip_time;
    time_min = MIN(time_min, round_trip_time);
    time_max = MAX(time_max, round_trip_time);

    printf("%d bytes from %s: icmp_seq=%u, ttl=%d, time=%.3f ms\n",
           icmplen, inet_ntoa(from_addr_ptr->sin_addr),
           icmp_ptr->icmp_seq, ip_ptr->ip_ttl, round_trip_time);
  }
}


void pinger()
{
  char sendbuf[BUF_SIZE];
  struct icmp* icmp_ptr = (struct icmp*)sendbuf;

  // init icmp message
  icmp_ptr->icmp_type = ICMP_ECHO;
  icmp_ptr->icmp_code = 0;
  icmp_ptr->icmp_id = pid;
  icmp_ptr->icmp_seq = packets_sent ++;
  gettimeofday((struct timeval*)icmp_ptr->icmp_data, NULL);

  int icmp_len = 8 + 56; // 8 - header, 56 - data

  icmp_ptr->icmp_cksum = 0;
  icmp_ptr->icmp_cksum = calc_checksum((unsigned short*)icmp_ptr, icmp_len);

  int func_res = sendto(socket_d, sendbuf, icmp_len, 0, (struct sockaddr*)&serv_addr, sizeof(serv_addr));

  if (-1 == func_res)
  {
    perror("sendto() failed");
    exit(-1);
  }
}

void catcher(int signum)
{
  if (SIGALRM == signum)
  {
    pinger();
    return;
  }
  else if (SIGINT == signum)
  {
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
    exit(-1);
  }
}
