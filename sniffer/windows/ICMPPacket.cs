using System.Collections.Generic;
using System.IO;
using System.Net;


namespace PigSniffer
{
  // http://security.maruhn.com/iptables-tutorial/x1078.html
  class ICMPPacket : Packet
  {
    private readonly byte type;       // 0 (8)
    private readonly byte code;       // 8 (8)
    private readonly ushort checksum; // 16 (16)


    public ICMPPacket(byte[] data, int size) : base(data, size)
    {
      using (var memoryStream = new MemoryStream(data, 0, size))
      {
        var binaryReader = new BinaryReader(memoryStream);

        type = binaryReader.ReadByte();
        code = binaryReader.ReadByte();
        checksum = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
      }

      switch (type)
      {
        case 0:
        case 8: headerLength = 8; break;
        default: headerLength = 4; break;
      }
      innerPacket = null;
    }


    public override string GetName()
    {
      return "ICMP";
    }


    public override List<string> GetHeaderValues()
    {
      var headerValues = new List<string>();

      string typeDescription;
      switch (type)
      {
        case 0: typeDescription = "Echo reply"; break;
        case 3: typeDescription = "Destination unreachable"; break;
        case 8: typeDescription = "Echo request"; break;
        default: typeDescription = "unknown"; break;
      }
      headerValues.Add(string.Format("Type: 0x{0:X} ({1})", type, typeDescription));

      headerValues.Add(string.Format("Code: 0x{0:X}", code));
      headerValues.Add(string.Format("Checksum: 0x{0:X}", checksum));

      byte[] header = GetHeader();

      using (var memoryStream = new MemoryStream(header, 0, header.Length))
      {
        var binaryReader = new BinaryReader(memoryStream);
        
        if (0 == type || 8 == type)
        {
          headerValues.Add(string.Format("Identifier: 0x{0:X}",
            (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16())));
          headerValues.Add(string.Format("Sequence number: 0x{0:X}",
            (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16())));
        }
      }

      return headerValues;
    }
  }
}
