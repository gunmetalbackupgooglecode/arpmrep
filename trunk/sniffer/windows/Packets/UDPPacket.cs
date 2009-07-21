using System.Collections.Generic;
using System.IO;
using System.Net;


namespace PigSniffer.Packets
{
  // http://www.networksorcery.com/enp/protocol/udp.htm
  class UDPPacket : Packet
  {
    private readonly ushort srcPort;  // 0 (16)
    private readonly ushort destPort; // 16 (16)
    /// <summary>
    /// Length of header and data in bytes
    /// </summary>
    private readonly ushort length;   // 32 (16)
    private readonly ushort checksum; // 48 (16)


    public UDPPacket(byte[] data, int size) : base(data, size)
    {
      using (var memoryStream = new MemoryStream(data, 0, size))
      {
        var binaryReader = new BinaryReader(memoryStream);

        srcPort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
        destPort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
        length = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
        checksum = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
      }

      headerLength = 8;
      innerPacket = null;
    }


    public override string GetName()
    {
      return "UDP";
    }


    public override List<string> GetHeaderValues()
    {
      var headerValues = new List<string>();

      headerValues.Add(string.Format("Source Port: {0}", srcPort));
      headerValues.Add(string.Format("Destination Port: {0}", destPort));
      headerValues.Add(string.Format("Length: {0}", length));
      headerValues.Add(string.Format("Checksum: 0x{0:X}", checksum));

      return headerValues;
    }


    public ushort GetSrcPort()
    {
      return srcPort;
    }


    public ushort GetDestPort()
    {
      return destPort;
    }

  }

}
