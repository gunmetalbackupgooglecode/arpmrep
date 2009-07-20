using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace PigSniffer
{
  // http://www.networksorcery.com/enp/protocol/tcp.htm
  class TCPPacket : Packet
  {
    private readonly ushort srcPort;        // 0 (16)
    private readonly ushort destPort;       // 16 (16)
    private readonly uint sequenceNumber;   // 32 (32)
    private readonly uint ackNumber;        // 64 (32)
    private readonly byte dataOffset;       // 96 (4)
//    private readonly byte reserved;         // 100 (3)
    private readonly byte ECN;              // 103 (3)
    private readonly byte controlBits;      // 106 (6)
    private readonly ushort window;         // 112 (16)
    private readonly ushort checksum;       // 128 (16)
    private readonly ushort urgentPointer;  // 144 (16)


    public TCPPacket(byte[] data, int size) : base(data, size)
    {
      using (var memoryStream = new MemoryStream(data, 0, size))
      {
        var binaryReader = new BinaryReader(memoryStream);

        srcPort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
        destPort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
        sequenceNumber = (uint)IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());
        ackNumber = (uint)IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());

        ushort temp = binaryReader.ReadUInt16();
        dataOffset = (byte)(temp >> 12);
        ECN = (byte)((temp >> 6) & 7);
        controlBits = (byte)(temp & 0x3f);

        window = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
        checksum = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
        urgentPointer = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
      }

      headerLength = dataOffset * 4;
      innerPacket = null;
    }

    public override string GetName()
    {
      return "TCP";
    }

    public override List<string> GetHeaderValues()
    {
      var headerValues = new List<string>();

      headerValues.Add(string.Format("Source Port: {0}", srcPort));
      headerValues.Add(string.Format("Destination Port: {0}", destPort));
      headerValues.Add(string.Format("Sequence Number: 0x{0:X}", sequenceNumber));
      headerValues.Add(string.Format("Acknowledgment Number: 0x{0:X}", ackNumber));
      headerValues.Add(string.Format("Data Offset: {0}", dataOffset));
      headerValues.Add(string.Format("Explicit Congestion Notification: {0}", ECN));

      var controlBitsDescription = new StringBuilder();
      if (1 == (controlBits & 1))
      {
        controlBitsDescription.Append("End of data, ");
      }
      if (1 == ((controlBits >> 1) & 1))
      {
        controlBitsDescription.Append("Synchronize sequence numbers, ");
      }
      if (1 == ((controlBits >> 2) & 1))
      {
        controlBitsDescription.Append("Reset connection, ");
      }
      if (1 == ((controlBits >> 3) & 1))
      {
        controlBitsDescription.Append("Push, ");
      }
      if (1 == ((controlBits >> 4) & 1))
      {
        controlBitsDescription.Append("Acknowledgment number valid, ");
      }
      if (1 == ((controlBits >> 5) & 1))
      {
        controlBitsDescription.Append("Urgent pointer valid , ");
      }
      if (controlBitsDescription.Length != 0)
      {
        controlBitsDescription.Remove(controlBitsDescription.Length - 2, 2);
      }
      headerValues.Add(string.Format("Control Bits: 0x{0:X} ({1})", controlBits, controlBitsDescription));

      headerValues.Add(string.Format("Window: 0x{0:X}", window));
      headerValues.Add(string.Format("Checksum: 0x{0:X}", checksum));
      headerValues.Add(string.Format("Urgent Pointer: 0x{0:X}", urgentPointer));

      // TODO: parse options

      return headerValues;
    }
  }
}
