using System;
using System.Collections.Generic;

namespace PigSniffer
{
  abstract class Packet
  {
    public const int MAX_PACKET_SIZE = 4096;

    /// <summary>
    /// Contains packet's header and inner data
    /// </summary>
    private readonly byte[] data;
    protected int headerLength;
    protected Packet innerPacket;

    
    protected Packet(byte[] data, int size)
    {
      this.data = new byte[size];
      Array.Copy(data, this.data, size);
    }

    /// <summary>
    /// Returns printable name of the packet
    /// </summary>
    /// <returns></returns>
    public abstract string GetName();
    
    /// <summary>
    /// Returns list of strings described each header value
    /// </summary>
    /// <returns></returns>
    public abstract List<string> GetHeaderValues();


    public Packet GetInnerPacket()
    {
      return innerPacket;
    }

    
    public byte[] GetHeader()
    {
      var header = new byte[headerLength];

      Array.Copy(data, 0, header, 0, headerLength);
      return header;
    }


    public byte[] GetInnerData()
    {
      var innerData = new byte[data.Length - headerLength];

      Array.Copy(data, headerLength, innerData, 0, data.Length - headerLength);
      return innerData;
    }

  }

}
