// http://www.networksorcery.com/enp/protocol/ip.htm

using System;
using System.IO;
using System.Net;

namespace PigSniffer
{
  public enum Protocol
  {
    ICMP = 1,
    TCP = 6,
    UDP = 17,
    Reserved = -1
  };

  class IPHeader
  {
    /// <summary>
    /// 4 - IP
    /// </summary>
    private byte version;                 // 0 (4)
    /// <summary>
    /// IP header length in dwords
    /// </summary>
    private byte IHL;                     // 4 (4)
    private byte differentiatedServices;  // 8 (8)
    /// <summary>
    /// Whole packet's lenght in bytes
    /// </summary>
    private ushort totalLength;           // 16 (16)
    private ushort identification;        // 32 (16)
    private byte flags;                   // 48 (3)
    private ushort fragmentOffset;        // 51 (13)
    /// <summary>
    /// time to live
    /// </summary>
    private byte TTL;                     // 64 (8)
    private byte protocol;                // 72 (8)
    private ushort checksum;              // 80 (16)
    private uint srcIPAddress;            // 96 (32)
    private uint destIPAddress;           // 128 (32)

    private byte[] ipData = new byte[4096];
    private byte[] innerData = new byte[4096];
    private int length;


    public IPHeader(byte[] data, int size)
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
        srcIPAddress = (uint)(binaryReader.ReadInt32());
        destIPAddress = (uint)(binaryReader.ReadInt32());

        length = IHL * 4;
        Array.Copy(data, 0, ipData, 0, length);
        Array.Copy(data, length, innerData, 0, totalLength - length);
      }
    }


    public string GetSrcIPAddressString()
    {
      return (srcIPAddress & 0xff) + "." +
             ((srcIPAddress >> 8) & 0xff) + "." +
             ((srcIPAddress >> 16) & 0xff) + "." +
             ((srcIPAddress >> 24) & 0xff);
    }


    public string GetDestIPAddressString()
    {
      return (destIPAddress & 0xff) + "." +
             ((destIPAddress >> 8) & 0xff) + "." +
             ((destIPAddress >> 16) & 0xff) + "." +
             ((destIPAddress >> 24) & 0xff);
    }


    public string GetProtocolString()
    {
      switch (protocol)
      {
        case (byte)Protocol.ICMP:
          return "ICMP";
        case (byte)Protocol.TCP:
          return "TCP";
        case (byte)Protocol.UDP:
          return "UDP";
      }
      return "unknown";
    }
  }
}
