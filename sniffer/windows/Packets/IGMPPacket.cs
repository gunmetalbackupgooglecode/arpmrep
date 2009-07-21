using System.Collections.Generic;
using System.IO;
using System.Net;

namespace PigSniffer.Packets
{
  // // http://www.networksorcery.com/enp/protocol/igmp.htm
  class IGMPPacket : Packet
  {
    private readonly byte type;             // 0 (8)
    private readonly byte maxResponseTime;  // 8 (8)
    private readonly ushort checksum;       // 16 (16)
    private readonly uint groupAddress;     // 32 (32)


    public IGMPPacket(byte[] data, int size) : base(data, size)
    {
      using (var memoryStream = new MemoryStream(data, 0, size))
      {
        var binaryReader = new BinaryReader(memoryStream);

        type = binaryReader.ReadByte();
        maxResponseTime = binaryReader.ReadByte();
        checksum = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
        groupAddress = (uint)IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());
      }

      headerLength = 8;
      innerPacket = null;
    }


    public override string GetName()
    {
      return "IGMP";
    }


    public override List<string> GetHeaderValues()
    {
      var headerValues = new List<string>();

      string typeDescription;
      switch (type)
      {
        case 0x11: typeDescription = "Group Membership Query"; break;
        case 0x12: typeDescription = "IGMPv1 Membership Report"; break;
        case 0x13: typeDescription = "DVMRP"; break;
        case 0x14: typeDescription = "PIMv1"; break;
        case 0x15: typeDescription = "Cisco Trace Messages"; break;
        case 0x16: typeDescription = "IGMPv2 Membership Report"; break;
        case 0x17: typeDescription = "IGMPv2 Leave Group"; break;
        case 0x1e: typeDescription = "Multicast Traceroute Response"; break;
        case 0x1f: typeDescription = "Multicast Traceroute"; break;
        case 0x22: typeDescription = "IGMPv3 Membership Report"; break;
        case 0x30: typeDescription = "MRD, Multicast Router Advertisement"; break;
        case 0x31: typeDescription = "MRD, Multicast Router Solicitation"; break;
        case 0x32: typeDescription = "	MRD, Multicast Router Termination"; break;
        default: typeDescription = "unknown"; break;
      }
      headerValues.Add(string.Format("Type: 0x{0:X} ({1})", type, typeDescription));

      headerValues.Add(string.Format("Max Response Time: 0x{0:X}", maxResponseTime));
      headerValues.Add(string.Format("IGMP Checksum: 0x{0:X}", checksum));
      headerValues.Add(string.Format("Group Address: 0x{0:X} ({1})",
                                     groupAddress, GetGroupAddressString()));

      return headerValues;
    }


    private string GetGroupAddressString()
    {
      if (0 == groupAddress)
      {
        return "0";
      }

      return ((groupAddress >> 24) & 0xff) + "." +
             ((groupAddress >> 16) & 0xff) + "." +
             ((groupAddress >> 8) & 0xff) + "." +
             (groupAddress & 0xff);
    }

  }

}
