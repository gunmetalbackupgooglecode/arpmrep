#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <netinet/ip.h>
#include <netinet/ip_icmp.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netdb.h>
#include <sys/time.h>
#include <unistd.h>
#include <arpa/inet.h>
#include "utils.h"


#define BUF_SIZE 1500
#define MAX_TTL 30
#define MAX_PROBE 3


pid_t pid;
int socket_d;
struct sockaddr_in recv_sockaddr;

enum output_result
{
  ERROR_RESULT,
  ICMP_ECHO_REPLY,
  ICMP_TIME_EXCEEDED_RESULT,
  TIME_OUT_RESULT
};

static enum output_result output(int seq, struct timeval* recv_time_ptr);


int main_icmp(struct hostent* host_ptr)
{
  pid = getpid();

  socket_d = socket(PF_INET, SOCK_RAW, IPPROTO_ICMP);
  if (socket_d < 0)
  {
    perror ("socket() failed");
    exit(-1);
  }

  setuid(getuid());

  struct sockaddr_in send_sockaddr;

  bzero(&send_sockaddr, sizeof(send_sockaddr));
  send_sockaddr.sin_family = AF_INET;
  send_sockaddr.sin_addr= *((struct in_addr *) host_ptr->h_addr);

  struct sockaddr_in last_probe_sockaddr;
  char send_buf[BUF_SIZE];
  struct icmp* icmp_ptr;
  int icmp_len;
  struct timeval* send_time_ptr;
  struct timeval recv_time;
  double round_time_trip;
  int code;
  int ttl;
  int probe;
  int seq = 0;
  int is_done = 0;

  for (ttl = 1; ttl <= MAX_TTL && !is_done; ++ ttl)
  {
    setsockopt(socket_d, SOL_IP, IP_TTL, &ttl, sizeof(int)); // set new TTL value
    bzero(&last_probe_sockaddr, sizeof(last_probe_sockaddr)); // clear last address

    printf("%2d  ", ttl);
    fflush(stdout);

    for (probe = 0; probe < MAX_PROBE; ++ probe)
    {
      // fill ICMP packet
      icmp_ptr = (struct icmp*)send_buf;
      icmp_ptr->icmp_type = ICMP_ECHO;
      icmp_ptr->icmp_code = 0;
      icmp_ptr->icmp_id = pid;
      icmp_ptr->icmp_seq = ++ seq;
      send_time_ptr = (struct timeval*)icmp_ptr->icmp_data;
      gettimeofday(send_time_ptr, NULL);

      icmp_len = 8 + 56;
      icmp_ptr->icmp_cksum = 0;
      icmp_ptr->icmp_cksum = calc_checksum((unsigned short*)icmp_ptr, icmp_len);

      if (sendto(socket_d, send_buf, icmp_len, 0, (struct sockaddr*)&send_sockaddr, sizeof(send_sockaddr)) < 0)
      {
        perror("sendto() failed");
        exit(-1);
      }

      code = output(seq, &recv_time);
      if (TIME_OUT_RESULT == code)
      {
        printf(" *");
      }
      else
      {
        if (memcmp(&recv_sockaddr.sin_addr, &last_probe_sockaddr.sin_addr, sizeof(recv_sockaddr.sin_addr)) != 0)
        {
          host_ptr = gethostbyaddr(&recv_sockaddr.sin_addr, sizeof(recv_sockaddr.sin_addr), recv_sockaddr.sin_family);
          if (host_ptr  != 0)
          {
            printf(" %s (%s)", inet_ntoa(recv_sockaddr.sin_addr), host_ptr->h_name);
          }
          else
          {
            printf(" %s", inet_ntoa(recv_sockaddr.sin_addr));
          }
          memcpy(&last_probe_sockaddr.sin_addr, &recv_sockaddr.sin_addr, sizeof(last_probe_sockaddr.sin_addr));
        }

        time_sub(&recv_time, send_time_ptr);
        round_time_trip = recv_time.tv_sec * 1000.0 + recv_time.tv_usec / 1000.0;
        printf("  %.3f ms", round_time_trip);

        is_done = (ICMP_ECHO_REPLY == code);
      }

      fflush(stdout);
    }

    printf("\n");
  }

  return 0;
}


enum output_result output(int seq, struct timeval* recv_time_ptr)
{  int n;
  unsigned int sockaddr_len;
  char recv_buf[BUF_SIZE];
  fd_set fds;
  struct ip* ip_ptr;
  int ip_len;
  struct icmp* icmp_header;
  struct ip* inner_ip_ptr;
  int inner_ip_len;
  struct icmp* inner_icmp_ptr;

  struct timeval wait_time;

  wait_time.tv_sec = 4;  wait_time.tv_usec = 0;

  while (1)
  {
    sockaddr_len = sizeof(recv_sockaddr);

    FD_ZERO(&fds);
    FD_SET(socket_d, &fds);

    if (select(socket_d+1, &fds, NULL, NULL, &wait_time) > 0)
    {
      n = recvfrom(socket_d, recv_buf, sizeof(recv_buf), 0, (struct sockaddr*)&recv_sockaddr, &sockaddr_len);
    }    else if (!FD_ISSET(socket_d, &fds))
    {
      return TIME_OUT_RESULT;
    }
    else
    {
      perror("recvfrom() failed");
      continue;
    }

    gettimeofday(recv_time_ptr, NULL);

    ip_ptr = (struct ip*)recv_buf;	// IP header
    ip_len = ip_ptr->ip_hl << 2;
    icmp_header = (struct icmp*) (recv_buf + ip_len); // ICMP header
    if (ICMP_TIMXCEED == icmp_header->icmp_type &&
        ICMP_TIMXCEED_INTRANS == icmp_header->icmp_code)
    {
      inner_ip_ptr = (struct ip*)(recv_buf + ip_len + 8);
      inner_ip_len = inner_ip_ptr->ip_hl << 2;
      inner_icmp_ptr = (struct icmp*) (recv_buf + ip_len + 8 + inner_ip_len);
      if (inner_icmp_ptr->icmp_id == pid &&
          inner_icmp_ptr->icmp_seq == seq)
        return ICMP_TIME_EXCEEDED_RESULT;
    }

    if (ICMP_ECHOREPLY == icmp_header->icmp_type &&
        icmp_header->icmp_id == pid &&
        icmp_header->icmp_seq == seq)
    {
      return ICMP_ECHO_REPLY;
    }
  }
}
