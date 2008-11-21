#include <Windows.h>

struct ip_v4
{
  UCHAR  ip_verlen;
  
//  #define ip_v (ip_verlen >> 4)
//  #define ip_hl (ip_verlen & 0xF) // length in dwords
  
  UCHAR  ip_tos;
  USHORT ip_len;
  USHORT ip_id;
  USHORT ip_off;
  UCHAR  ip_ttl;
  UCHAR  ip_p;
  USHORT ip_sum;
  struct in_addr ip_src;
  struct in_addr ip_dst;
};

typedef struct ip_v4 IPV4_HEADER;

struct icmp
{
  BYTE icmp_type;
  BYTE icmp_code;
  WORD icmp_cksum;

  union
  {
    u_char ih_pptr;
    struct in_addr ih_gwaddr;
    struct ih_idseq
    {
      short icd_id;
      short icd_seq;
    } ih_idseq;

    int ih_void;
  } icmp_hun;

  #define icmp_pptr icmp_hun.ih_pptr
  #define icmp_gwaddr icmp_hun.ih_gwaddr
  #define icmp_id icmp_hun.ih_idseq.icd_id
  #define icmp_seq icmp_hun.ih_idseq.icd_seq
  #define icmp_void icmp_hun.ih_void

  union
  {
//    struct id_ts
//    {
//      n_time its_otime;
//      n_time its_rtime;
//      n_time its_ttime;
//    } id_ts;

//    struct id_ip
//    {
//      struct ip idi_ip;
//    } id_ip;

    u_long id_mask;
    char id_data[1];
  } icmp_dun;

//  #define icmp_otime icmp_dun.id_ts.its_otime
//  #define icmp_rtime icmp_dun.id_ts.its_rtime
//  #define icmp_ttime icmp_dun.id_ts.its_ttime
//  #define icmp_ip icmp_dun.id_ip.idi_ip
  #define icmp_mask icmp_dun.id_mask
  #define icmp_data icmp_dun.id_data
};

typedef struct icmp ICMP_HEADER;

#define ICMP_ECHO       8
#define ICMP_ECHOREPLY  0
#define ICMP_DEST_UNREACH 3
