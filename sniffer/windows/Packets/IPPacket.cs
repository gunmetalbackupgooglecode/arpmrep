﻿using System.Collections.Generic;
using System.IO;
using System.Net;


namespace PigSniffer.Packets
{
  // // after edit don't forget to update IPPacket.GetProtocolString()
  public enum Protocol
  {
    ICMP = 1,
    IGMP = 2,
    TCP = 6,
    UDP = 17,
//    Reserved = -1
  };

  // // http://www.networksorcery.com/enp/protocol/ip.htm
  class IPPacket : Packet
  {
    /// <summary>
    /// 4 - IP
    /// </summary>
    private readonly byte version;                 // 0 (4)
    /// <summary>
    /// IP header length in dwords
    /// </summary>
    private readonly byte IHL;                     // 4 (4)
    private readonly byte differentiatedServices;  // 8 (8)
    /// <summary>
    /// Whole packet's lenght in bytes
    /// </summary>
    private readonly ushort totalLength;           // 16 (16)
    private readonly ushort identification;        // 32 (16)
    private readonly byte flags;                   // 48 (3)
    private readonly ushort fragmentOffset;        // 51 (13)
    /// <summary>
    /// time to live
    /// </summary>
    private readonly byte TTL;                     // 64 (8)
    private readonly byte protocol;                // 72 (8)
    private readonly ushort checksum;              // 80 (16)
    private readonly uint srcIPAddress;            // 96 (32)
    private readonly uint destIPAddress;           // 128 (32)

    // not IP packet's fields, but it's handy to store them here
    private readonly ushort srcPort;
    private readonly ushort destPort;


    public IPPacket(byte[] data, int size) : base(data, size)
    {
      using (var memoryStream = new MemoryStream(data, 0, size))
      {
        var binaryReader = new BinaryReader(memoryStream);

        byte versionAndIHL = binaryReader.ReadByte();
        version = (byte)(versionAndIHL >> 4);
        IHL = (byte)(versionAndIHL & 0xf);
        differentiatedServices = binaryReader.ReadByte();
        totalLength = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
        identification = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
        var flagsAndOffset = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
        flags = (byte)(flagsAndOffset >> 13);
        fragmentOffset = (byte)(flagsAndOffset & 0x1fff);
        TTL = binaryReader.ReadByte();
        protocol = binaryReader.ReadByte();
        checksum = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
        srcIPAddress = (uint)IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());
        destIPAddress = (uint)IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());
      }

      headerLength = IHL * 4;

      byte[] innerData = GetInnerData();
      
      switch (protocol)
      {
        case (byte)Protocol.ICMP:
          innerPacket = new ICMPPacket(innerData, innerData.Length);
          break;
        case (byte)Protocol.IGMP:
          innerPacket = new IGMPPacket(innerData, innerData.Length);
          break;
        case (byte)Protocol.TCP:
          var tcpPacket = new TCPPacket(innerData, innerData.Length);
          innerPacket = tcpPacket;
          srcPort = tcpPacket.GetSrcPort();
          destPort = tcpPacket.GetDestPort();
          break;
        case (byte)Protocol.UDP:
          var udpPacket = new UDPPacket(innerData, innerData.Length);
          innerPacket = udpPacket;
          srcPort = udpPacket.GetSrcPort();
          destPort = udpPacket.GetDestPort();
          break;
      }
    }


    public override string GetName()
    {
      return "IP";
    }


    public override List<string> GetHeaderValues()
    {
      var headerValues = new List<string>();

      string versionDescription;
      switch (version)
      {
        case 0: 
        case 15: versionDescription = "reserved"; break;
        case 4: versionDescription = "IP"; break;
        case 5: versionDescription = "ST"; break;
        case 6: versionDescription = "SIP, SIPP"; break;
        case 7: versionDescription = "TP/IX"; break;
        case 8: versionDescription = "PIP"; break;
        case 9: versionDescription = "TUBA"; break;
        default: versionDescription = "unknown"; break;
      }
      headerValues.Add(string.Format("Version: {0} ({1})", version, versionDescription));

      headerValues.Add(string.Format("Internet Header Length: 0x{0:X}", IHL));

      string precedence;
      switch (differentiatedServices >> 5)
      {
        case 0: precedence = "Routine"; break;
        case 1: precedence = "Priority"; break;
        case 2: precedence = "Immediate"; break;
        case 3: precedence = "Flash"; break;
        case 4: precedence = "Flash Override"; break;
        case 5: precedence = "CRITIC/ECP"; break;
        case 6: precedence = "Internetwork Control"; break;
        case 7: precedence = "Network Control"; break;
        default: precedence = "unknown"; break;
      }
      string delay = (1 == (differentiatedServices & 0x10)) ? "Normal Delay" : "Low Delay";
      string throughput = (1 == (differentiatedServices & 8)) ? "Normal Throughput" : "High Throughput";
      string relibility = (1 == (differentiatedServices & 4)) ? "Normal Relibility" : "High Relibility";
      headerValues.Add(string.Format("Differentiated Services: 0x{0:X} ({1}, {2}, {3}, {4})",
                                     differentiatedServices, precedence, delay, throughput, relibility));

      headerValues.Add(string.Format("Total length: 0x{0:X}", totalLength));
      headerValues.Add(string.Format("Identification: 0x{0:X}", identification));

      string flagsDF = (0 == (flags & 2)) ? "May Fragment" : "Don't Fragment";
      string flagsMF = (0 == (flags & 1)) ? "Last Fragment" : "More fragments follow this fragment";
      headerValues.Add(string.Format("Flags: 0x{0:X} ({1}, {2})",
                                     flags, flagsDF, flagsMF));

      headerValues.Add(string.Format("Fragment Offset: 0x{0:X}", fragmentOffset));
      headerValues.Add(string.Format("Time to Live: 0x{0:X}", TTL));
      headerValues.Add(string.Format("Protocol: 0x{0:X} ({1})",
                                     protocol, GetProtocolString()));
      headerValues.Add(string.Format("Checksum: 0x{0:X}", checksum));

      string srcIPAddressString = GetSrcIPAddressString();
      string srcHostName = Dns.GetHostEntry(srcIPAddressString).HostName;
      if (srcHostName.Equals(srcIPAddressString))
      {
        srcHostName = "not resolved";
      }
      headerValues.Add(string.Format("Source IP address: 0x{0:X} ({1}, {2})",
                                     srcIPAddress, srcIPAddressString, srcHostName));

      string destIPAddressString = GetDestIPAddressString();
      string destHostName = Dns.GetHostEntry(destIPAddressString).HostName;
      if (destHostName.Equals(destIPAddressString))
      {
        destHostName = "not resolved";
      }
      headerValues.Add(string.Format("Destination IP address: 0x{0:X} ({1}, {2})",
                                     destIPAddress, destIPAddressString, destHostName));

      // TODO: parse options

      return headerValues;
    }


    public string GetSrcIPAddressString()
    {
      return ((srcIPAddress >> 24) & 0xff) + "." +
             ((srcIPAddress >> 16) & 0xff) + "." +
             ((srcIPAddress >> 8) & 0xff) + "." +
             (srcIPAddress & 0xff);
    }


    public string GetDestIPAddressString()
    {
      return ((destIPAddress >> 24) & 0xff) + "." +
             ((destIPAddress >> 16) & 0xff) + "." +
             ((destIPAddress >> 8) & 0xff) + "." +
             (destIPAddress & 0xff);
    }


    public Protocol GetProtocol()
    {
      return (Protocol)protocol;
    }


    public string GetProtocolString()
    {
      switch (protocol)
      {
        case (byte)Protocol.ICMP: return "ICMP";
        case (byte)Protocol.IGMP: return "IGMP";
        case (byte)Protocol.TCP: return "TCP";
        case (byte)Protocol.UDP: return "UDP";
      }
      return "unknown";
    }


    public string GetSrcPortString()
    {
      if (0 == srcPort)
      {
        return "";
      }
      return srcPort.ToString();
    }


    public string GetDestPortString()
    {
      if (0 == destPort)
      {
        return "";
      }
      return destPort.ToString();
    }

  }

}
