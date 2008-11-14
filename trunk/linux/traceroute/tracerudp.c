#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <netinet/ip.h>
#include <netinet/ip_icmp.h>
#include <netinet/udp.h>
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
#define INITIAL_DEST_PORT (32768 + 666) // ports after this value are rarely used

// UDP data
struct outdata
{
  int seq;
  int ttl; // packet's TTL value
  struct timeval send_time;
};
const int DATA_LEN = sizeof(struct outdata);

int socket_udp_send_d;
int socket_icmp_recv_d;
struct sockaddr_in send_sockaddr;
struct sockaddr_in bind_sockaddr;
struct sockaddr_in recv_sockaddr;
int src_port;

enum get_packet_result
{
  ERROR_RESULT,
  ICMP_PORT_UNREACHABLE_RESULT,
  ICMP_TIME_EXCEEDED_RESULT,
  TIME_OUT_RESULT
};

static enum get_packet_result get_packet(int port, struct timeval* recv_time_ptr);

void main_udp(struct hostent* host_ptr)
{
  int func_res;
  double rtt;
  struct outdata *outdata;

  socket_icmp_recv_d = socket(PF_INET, SOCK_RAW, IPPROTO_ICMP);
  if (-1 == socket_icmp_recv_d)
  {
    perror("socket() failed");
    exit(-1);
  }

  setuid(getuid());

  socket_udp_send_d = socket(PF_INET,SOCK_DGRAM, 0);
  if (-1 == socket_udp_send_d)
  {
    perror("socket() failed");
    exit(-1);
  }

  bzero(&send_sockaddr, sizeof(send_sockaddr));
  send_sockaddr.sin_family = AF_INET;
  send_sockaddr.sin_addr = *((struct in_addr*)host_ptr->h_addr);

  // we need bind to know if received packet is our one
  bzero(&bind_sockaddr, sizeof(bind_sockaddr));
  bind_sockaddr.sin_family = AF_INET;
  src_port = (getpid() & 0xFFFF) | 0x8000; // unique for process port number
  bind_sockaddr.sin_port = htons(src_port);
  func_res = bind(socket_udp_send_d, (struct sockaddr*)&bind_sockaddr, sizeof(bind_sockaddr));
  if (-1 == func_res)
  {
    perror("bind() failed");
  }

  char send_buf[BUF_SIZE];
  struct sockaddr_in last_probe_addr; // print all different hosts for a single probe
  struct timeval recv_time;
  enum get_packet_result gp_result;
  struct hostent* cur_host;
  int is_done = 0;
  int seq = 0;
  int ttl;
  int probe;
  int dest_port;

  for (ttl = 1; ttl <= MAX_TTL && 0 == is_done; ++ ttl)
  {
    setsockopt(socket_udp_send_d, SOL_IP, IP_TTL, &ttl, sizeof(int)); // set new TTL value
    bzero(&last_probe_addr, sizeof(last_probe_addr)); // clear last address

    printf("%2d  ", ttl);
    fflush(stdout);

    // lets send MAX_PROBE packets to different ports
    // it's useful if current port is in use
    for (probe = 0; probe < MAX_PROBE; ++ probe)
    {
      outdata = (struct outdata*)send_buf;
      outdata->seq = ++ seq;
      outdata->ttl = ttl;
      gettimeofday(&outdata->send_time, NULL);

      dest_port = INITIAL_DEST_PORT + seq;

      send_sockaddr.sin_port = htons(dest_port);

      func_res = sendto(socket_udp_send_d, send_buf, DATA_LEN, 0,
                        (struct sockaddr*)&send_sockaddr, sizeof(send_sockaddr));
      if (-1 == func_res)
      {
        perror("sendto() failed");
        exit(-1);
      }

      gp_result = get_packet(dest_port, &recv_time);
      if (TIME_OUT_RESULT == gp_result)      {
        // not successful probe
        printf(" *");
      }
      else // we got smth
      {
        // if we got answer from another host then last probe, print its name
        func_res = memcmp(&recv_sockaddr.sin_addr, &last_probe_addr.sin_addr, sizeof(recv_sockaddr.sin_addr));
        if (func_res != 0)
        {
          // try to resolve host name
          cur_host = gethostbyaddr(&recv_sockaddr.sin_addr,
                                   sizeof(recv_sockaddr.sin_addr),
                                   recv_sockaddr.sin_family);
          if (cur_host != NULL)
          {
            printf(" %s (%s)", inet_ntoa(recv_sockaddr.sin_addr), cur_host->h_name);
          }
          else
          {
            printf(" %s", inet_ntoa(recv_sockaddr.sin_addr));
          }
          // save last address
          memcpy(&last_probe_addr.sin_addr, &recv_sockaddr.sin_addr, sizeof(last_probe_addr.sin_addr));
        }

        time_sub(&recv_time, &outdata->send_time);
        rtt = recv_time.tv_sec * 1000.0 + recv_time.tv_usec / 1000.0;
        printf("  %.3f ms", rtt);

        is_done = (ICMP_PORT_UNREACHABLE_RESULT == gp_result);
      }

      fflush(stdout);
    }

    printf("\n");
  }
}


enum get_packet_result get_packet(int dest_port, struct timeval* recv_time_ptr)
{
  unsigned int sockaddr_len = sizeof(recv_sockaddr);
  fd_set fds;
  char recv_buf[BUF_SIZE];
  struct timeval wait_time;
  struct ip* outer_ip_ptr;
  int outer_ip_len;
  struct icmp* icmp_ptr;
  struct ip* inner_ip_ptr;
  int inner_ip_len;
  struct udphdr* udp_ptr;

  // we will wait 4 seconds for response, else time out
  wait_time.tv_sec = 4;
  wait_time.tv_usec = 0;

  while (1)
  {
    FD_ZERO(&fds);
    FD_SET(socket_icmp_recv_d, &fds);

    if (select(socket_icmp_recv_d + 1, &fds, NULL, NULL, &wait_time) > 0)
    {
      // socket is ready - read data
      recvfrom(socket_icmp_recv_d, recv_buf, sizeof(recv_buf),
               0, (struct sockaddr*)&recv_sockaddr, &sockaddr_len);
    }
    else if (!FD_ISSET(socket_icmp_recv_d, &fds))
    {
      return TIME_OUT_RESULT;
    }
    else
    {
      perror("recvfrom() failed");
      continue;
//      return ERROR_RESULT;
    }

    gettimeofday(recv_time_ptr, NULL);

    outer_ip_ptr = (struct ip*)recv_buf; // outer IP header
    outer_ip_len = outer_ip_ptr->ip_hl << 2;
    icmp_ptr = (struct icmp*)(recv_buf + outer_ip_len); // ICMP header
    inner_ip_ptr = (struct ip*)(recv_buf + outer_ip_len + 8); // saved IP header
    inner_ip_len = inner_ip_ptr->ip_hl << 2;
    udp_ptr = (struct udphdr*)(recv_buf + outer_ip_len + 8 + inner_ip_len); // UDP header

    if (inner_ip_ptr->ip_p != IPPROTO_UDP ||
        udp_ptr->source != htons(src_port) ||
        udp_ptr->dest != htons(dest_port))
    {
      // not our packet
      continue;
    }

    if (ICMP_TIMXCEED == icmp_ptr->icmp_type &&
        ICMP_TIMXCEED_INTRANS == icmp_ptr->icmp_code)
    {
      return ICMP_TIME_EXCEEDED_RESULT;
    }

    if (ICMP_UNREACH == icmp_ptr->icmp_type &&
        ICMP_UNREACH_PORT == icmp_ptr->icmp_code)
    {
      return ICMP_PORT_UNREACHABLE_RESULT;
    }
  }
}
