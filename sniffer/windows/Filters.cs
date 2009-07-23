using System;
using System.Collections.Generic;
using System.Net;
using System.Text;


namespace PigSniffer
{
  /// <summary>
  /// Represents values' pair.
  /// </summary>
  class Range<T>
  {
    public T First { get; private set; }
    public T Second { get; private set; }


    public Range(T first, T second)
    {
      First = first;
      Second = second;
    }


    public override string ToString()
    {
      if (First.Equals(Second))
      {
        return First.ToString();
      }
      return First + " - " + Second;
    }
  }


  /// <summary>
  /// Packets' filters: source port, source IP, destination port, destination IP.
  /// </summary>
  public class Filters
  {
    private readonly List<Range<int>> srcPorts = new List<Range<int>>();
    private readonly List<Range<IPAddress>> srcIPs = new List<Range<IPAddress>>();
    private readonly List<Range<int>> destPorts = new List<Range<int>>();
    private readonly List<Range<IPAddress>> destIPs = new List<Range<IPAddress>>();


    /// <summary>
    /// Returns string representation of source ports values
    /// </summary>
    /// <returns>souce ports as string</returns>
    public string GetSrcPortsString()
    {
      return RangeCollectionToString(srcPorts);
    }


    /// <summary>
    /// Parses input string and updates source port filters.
    /// </summary>
    /// <param name="srcPortsString">sourse ports as string</param>
    /// <returns>true if srcPortsString is valid, false otherwise</returns>
    public bool SetSrcPorts(string srcPortsString)
    {
      srcPorts.Clear();
      if (string.IsNullOrEmpty(srcPortsString))
      {
        return true;
      }

      try
      {
        srcPorts.AddRange(ParsePortsString(srcPortsString));
      }
      catch (ArgumentException)
      {
        return false;
      }
      return true;
    }


    /// <summary>
    /// Checkes if source port is allowed with filters
    /// </summary>
    /// <param name="srcPortString">sourse port as string</param>
    /// <returns>true if allowed</returns>
    public bool IsSrcPortAllowed(string srcPortString)
    {
      if (0 == srcPorts.Count)
      {
        return true;
      }

      int srcPort;
      
      if (!int.TryParse(srcPortString, out srcPort))
      {
        return false;
      }
      foreach (Range<int> srcPortRange in srcPorts)
      {
        if (srcPort >= srcPortRange.First && srcPort <= srcPortRange.Second)
        {
          return true;
        }
      }
      return false;
    }


    /// <summary>
    /// Returns string representation of source IPs values
    /// </summary>
    /// <returns>IPs as string</returns>
    public string GetSrcIPsString()
    {
      return RangeCollectionToString(srcIPs);
    }


    /// <summary>
    /// Parses input string and updates source IP filters.
    /// </summary>
    /// <param name="srcIPsString">sourse IPs as string</param>
    /// <returns>true if srcIPsString is valid, false otherwise</returns>
    public bool SetSrcIPs(string srcIPsString)
    {
      srcIPs.Clear();
      if (string.IsNullOrEmpty(srcIPsString))
      {
        return true;
      }

      try
      {
        srcIPs.AddRange(ParseIPsString(srcIPsString));
      }
      catch (ArgumentException)
      {
        return false;
      }
      return true;
    }


    /// <summary>
    /// Checkes if source IP is allowed with filters
    /// </summary>
    /// <param name="srcIPString">source IP as string</param>
    /// <returns>true if allowed</returns>
    public bool IsSrcIPAllowed(string srcIPString)
    {
      if (0 == srcIPs.Count)
      {
        return true;
      }

      IPAddress srcIP;

      if (!IPAddress.TryParse(srcIPString, out srcIP))
      {
        return false;
      }
      foreach (Range<IPAddress> srcIPRange in srcIPs)
      {
        if (IPAddressCompareTo(srcIP, srcIPRange.First) >= 0 &&
          IPAddressCompareTo(srcIP, srcIPRange.Second) <= 0)
        {
          return true;
        }
      }
      return false;
    }

    
    /// <summary>
    /// Returns destination representation of source ports values
    /// </summary>
    /// <returns>destination ports as string</returns>
    public string GetDestPortsString()
    {
      return RangeCollectionToString(destPorts);
    }

    
    /// <summary>
    /// Parses input string and updates destination port filters.
    /// </summary>
    /// <param name="destPortsString">destination ports as string</param>
    /// <returns>true if destPortsString is valid, false otherwise</returns>
    public bool SetDestPorts(string destPortsString)
    {
      destPorts.Clear();
      if (string.IsNullOrEmpty(destPortsString))
      {
        return true;
      }

      try
      {
        destPorts.AddRange(ParsePortsString(destPortsString));
      }
      catch (ArgumentException)
      {
        return false;
      }
      return true;
    }


    /// <summary>
    /// Checkes if destination port is allowed with filters
    /// </summary>
    /// <param name="destPortString">destination port as string</param>
    /// <returns>true if allowed</returns>
    public bool IsDestPortAllowed(string destPortString)
    {
      if (0 == destPorts.Count)
      {
        return true;
      }

      int destPort;

      if (!int.TryParse(destPortString, out destPort))
      {
        return false;
      }
      foreach (Range<int> destPortRange in destPorts)
      {
        if (destPort >= destPortRange.First && destPort <= destPortRange.Second)
        {
          return true;
        }
      }
      return false;
    }

    
    /// <summary>
    /// Returns string representation of destination IPs values
    /// </summary>
    /// <returns>IPs as string</returns>
    public string GetDestIPsString()
    {
      return RangeCollectionToString(destIPs);
    }


    /// <summary>
    /// Parses input string and updates destination IP filters.
    /// </summary>
    /// <param name="destIPsString">destination IPs as string</param>
    /// <returns>true if destIPsString is valid, false otherwise</returns>
    public bool SetDestIPs(string destIPsString)
    {
      destIPs.Clear();
      if (string.IsNullOrEmpty(destIPsString))
      {
        return true;
      }

      try
      {
        destIPs.AddRange(ParseIPsString(destIPsString));
      }
      catch (ArgumentException)
      {
        return false;
      }
      return true;
    }


    /// <summary>
    /// Checkes if destination IP is allowed with filters
    /// </summary>
    /// <param name="destIPString">destination IP as string</param>
    /// <returns>true if allowed</returns>
    public bool IsDestIPAllowed(string destIPString)
    {
      if (0 == destIPs.Count)
      {
        return true;
      }

      IPAddress destIP;

      if (!IPAddress.TryParse(destIPString, out destIP))
      {
        return false;
      }
      foreach (Range<IPAddress> destIPRange in destIPs)
      {
        if (IPAddressCompareTo(destIP, destIPRange.First) >= 0 &&
          IPAddressCompareTo(destIP, destIPRange.Second) <= 0)
        {
          return true;
        }
      }
      return false;
    }
    
    
    /// <summary>
    /// Returns string representation of Ranges values
    /// </summary>
    /// <param name="rangeCollection">Range collection</param>
    /// <returns>Range collection as string</returns>
    private static string RangeCollectionToString<T>(ICollection<Range<T>> rangeCollection)
    {
      if (null == rangeCollection || 0 == rangeCollection.Count)
      {
        return "";
      }

      var portsStringBuilder = new StringBuilder();

      foreach (Range<T> port in rangeCollection)
      {
        portsStringBuilder.Append(port.ToString());
        portsStringBuilder.Append(", ");
      }
      portsStringBuilder.Remove(portsStringBuilder.Length - 2, 2);

      return portsStringBuilder.ToString();
    }


    /// <summary>
    /// Parses string with ports
    /// </summary>
    /// <param name="portsString">ports as string</param>
    /// <returns>list of Range objects</returns>
    /// <exception cref="ArgumentException">ports string is invalid</exception>
    private static List<Range<int>> ParsePortsString(string portsString)
    {
      string[] portsSubstrings = portsString.Split(new[] { ',' });
      var result = new List<Range<int>>(portsSubstrings.Length);

      foreach (string portsSubstring in portsSubstrings)
      {
        Range<int> curRange;

        if (portsSubstring.IndexOf('-') != -1)
        {
          string[] portsRange = portsSubstring.Split('-');
          int first, second;

          if (portsRange.Length != 2 ||
            !int.TryParse(portsRange[0], out first) || !int.TryParse(portsRange[1], out second) ||
            first <= 0 || second <= 0 || first > second)
          {
            throw new ArgumentException("Ports string is invalid");
          }
          curRange = new Range<int>(first, second);
        }
        else
        {
          int port;

          if (!int.TryParse(portsSubstring, out port) || port <= 0)
          {
            throw new ArgumentException("Ports string is invalid");
          }
          curRange = new Range<int>(port, port);
        }
        result.Add(curRange);
      }

      return result;
    }


    /// <summary>
    /// Parses string with IPs
    /// </summary>
    /// <param name="IPsString">IPs as string</param>
    /// <returns>list of Range objects</returns>
    /// <exception cref="ArgumentException">IPs string is invalid</exception>
    private static List<Range<IPAddress>> ParseIPsString(string IPsString)
    {
      string[] IPsSubstrings = IPsString.Split(new[] { ',' });
      var result = new List<Range<IPAddress>>(IPsSubstrings.Length);

      foreach (string IPsSubstring in IPsSubstrings)
      {
        Range<IPAddress> curRange;

        if (IPsSubstring.IndexOf('-') != -1)
        {
          string[] IPsRange = IPsSubstring.Split('-');
          IPAddress first, second;

          if (IPsRange.Length != 2 ||
            !IPAddress.TryParse(IPsRange[0].Trim(), out first) ||
            !IPAddress.TryParse(IPsRange[1].Trim(), out second) ||
            IPAddressCompareTo(first, second) > 0)
          {
            throw new ArgumentException("IPs string is invalid");
          }
          curRange = new Range<IPAddress>(first, second);
        }
        else
        {
          IPAddress address;

          if (!IPAddress.TryParse(IPsSubstring.Trim(), out address))
          {
            throw new ArgumentException("IPs string is invalid");
          }
          curRange = new Range<IPAddress>(address, address);
        }
        result.Add(curRange);
      }

      return result;
    }


    /// <summary>
    /// Compare function for IPAddress objects
    /// </summary>
    /// <param name="address1">first param</param>
    /// <param name="address2">second param</param>
    /// <returns>0, -1 or 1 as usually</returns>
    private static int IPAddressCompareTo(IPAddress address1, IPAddress address2)
    {
      byte[] address1Bytes = address1.GetAddressBytes();
      byte[] address2Bytes = address2.GetAddressBytes();

      for (int i = 0; i < 4; ++ i)
      {
        int byteCompareResult = address1Bytes[i].CompareTo(address2Bytes[i]);

        if (byteCompareResult != 0)
        {
          return byteCompareResult;
        }
      }
      return 0;
    }

  }

}
