using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using PigSniffer.Packets;


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
    private readonly List<Range<IPAddress>> srcIPsInclude = new List<Range<IPAddress>>();
    private readonly List<Range<IPAddress>> srcIPsExclude = new List<Range<IPAddress>>();
    private readonly List<Range<int>> srcPortsInclude = new List<Range<int>>();
    private readonly List<Range<int>> srcPortsExclude = new List<Range<int>>();
    private readonly List<Range<IPAddress>> destIPsInclude = new List<Range<IPAddress>>();
    private readonly List<Range<IPAddress>> destIPsExclude = new List<Range<IPAddress>>();
    private readonly List<Range<int>> destPortsInclude = new List<Range<int>>();
    private readonly List<Range<int>> destPortsExclude = new List<Range<int>>();
    private readonly List<Protocol> protocols = new List<Protocol>();


    public Filters()
    {
      foreach (int protocol in Enum.GetValues(typeof(Protocol)))
      {
        protocols.Add((Protocol)protocol);
      }
    }


    /// <summary>
    /// Returns string representation of source IPs include values
    /// </summary>
    /// <returns>IPs as string</returns>
    public string GetSrcIPsIncludeString()
    {
      return RangeCollectionToString(srcIPsInclude);
    }


    /// <summary>
    /// Parses input string and updates source IP include filters.
    /// </summary>
    /// <param name="srcIPsIncludeString">sourse IPs include as string</param>
    /// <returns>true if srcIPsIncludeString is valid, false otherwise</returns>
    public bool SetSrcIPsInclude(string srcIPsIncludeString)
    {
      srcIPsInclude.Clear();
      if (string.IsNullOrEmpty(srcIPsIncludeString))
      {
        return true;
      }

      try
      {
        srcIPsInclude.AddRange(ParseIPsString(srcIPsIncludeString));
      }
      catch (ArgumentException)
      {
        return false;
      }
      return true;
    }


    /// <summary>
    /// Returns string representation of source IPs exclude values
    /// </summary>
    /// <returns>IPs as string</returns>
    public string GetSrcIPsExcludeString()
    {
      return RangeCollectionToString(srcIPsExclude);
    }


    /// <summary>
    /// Parses input string and updates source IP exclude filters.
    /// </summary>
    /// <param name="srcIPsExcludeString">sourse IPs exclude as string</param>
    /// <returns>true if srcIPsExcludeString is valid, false otherwise</returns>
    public bool SetSrcIPsExclude(string srcIPsExcludeString)
    {
      srcIPsExclude.Clear();
      if (string.IsNullOrEmpty(srcIPsExcludeString))
      {
        return true;
      }

      try
      {
        srcIPsExclude.AddRange(ParseIPsString(srcIPsExcludeString));
      }
      catch (ArgumentException)
      {
        return false;
      }
      return true;
    }


    /// <summary>
    /// Returns string representation of source ports include values
    /// </summary>
    /// <returns>source ports include as string</returns>
    public string GetSrcPortsIncludeString()
    {
      return RangeCollectionToString(srcPortsInclude);
    }


    /// <summary>
    /// Parses input string and updates source port include filters.
    /// </summary>
    /// <param name="srcPortsIncludeString">source ports include as string</param>
    /// <returns>true if srcPortsIncludeString is valid, false otherwise</returns>
    public bool SetSrcPortsInclude(string srcPortsIncludeString)
    {
      srcPortsInclude.Clear();
      if (string.IsNullOrEmpty(srcPortsIncludeString))
      {
        return true;
      }

      try
      {
        srcPortsInclude.AddRange(ParsePortsString(srcPortsIncludeString));
      }
      catch (ArgumentException)
      {
        return false;
      }
      return true;
    }


    /// <summary>
    /// Returns string representation of source ports exclude values
    /// </summary>
    /// <returns>source ports exclude as string</returns>
    public string GetSrcPortsExcludeString()
    {
      return RangeCollectionToString(srcPortsExclude);
    }


    /// <summary>
    /// Parses input string and updates source port exclude filters.
    /// </summary>
    /// <param name="srcPortsExcludeString">source ports exclude as string</param>
    /// <returns>true if srcPortsExcludeString is valid, false otherwise</returns>
    public bool SetSrcPortsExclude(string srcPortsExcludeString)
    {
      srcPortsExclude.Clear();
      if (string.IsNullOrEmpty(srcPortsExcludeString))
      {
        return true;
      }

      try
      {
        srcPortsExclude.AddRange(ParsePortsString(srcPortsExcludeString));
      }
      catch (ArgumentException)
      {
        return false;
      }
      return true;
    }


    /// <summary>
    /// Returns string representation of destination IPs include values
    /// </summary>
    /// <returns>destination IPs include as string</returns>
    public string GetDestIPsIncludeString()
    {
      return RangeCollectionToString(destIPsInclude);
    }


    /// <summary>
    /// Parses input string and updates destination IPs include filters.
    /// </summary>
    /// <param name="destIPsIncludeString">destination IPs include as string</param>
    /// <returns>true if destIPsIncludeString is valid, false otherwise</returns>
    public bool SetDestIPsInclude(string destIPsIncludeString)
    {
      destIPsInclude.Clear();
      if (string.IsNullOrEmpty(destIPsIncludeString))
      {
        return true;
      }

      try
      {
        destIPsInclude.AddRange(ParseIPsString(destIPsIncludeString));
      }
      catch (ArgumentException)
      {
        return false;
      }
      return true;
    }


    /// <summary>
    /// Returns string representation of destination IPs exclude values
    /// </summary>
    /// <returns>destination IPs exclude as string</returns>
    public string GetDestIPsExcludeString()
    {
      return RangeCollectionToString(destIPsExclude);
    }


    /// <summary>
    /// Parses input string and updates destination IPs exclude filters.
    /// </summary>
    /// <param name="destIPsExcludeString">destination IPs exclude as string</param>
    /// <returns>true if destIPsExcludeString is valid, false otherwise</returns>
    public bool SetDestIPsExclude(string destIPsExcludeString)
    {
      destIPsExclude.Clear();
      if (string.IsNullOrEmpty(destIPsExcludeString))
      {
        return true;
      }

      try
      {
        destIPsExclude.AddRange(ParseIPsString(destIPsExcludeString));
      }
      catch (ArgumentException)
      {
        return false;
      }
      return true;
    }


    /// <summary>
    /// Returns string representation of destination ports include values
    /// </summary>
    /// <returns>destination ports include as string</returns>
    public string GetDestPortsIncludeString()
    {
      return RangeCollectionToString(destPortsInclude);
    }

    
    /// <summary>
    /// Parses input string and updates destination port include filters.
    /// </summary>
    /// <param name="destPortsIncludeString">destination ports include as string</param>
    /// <returns>true if destPortsIncludeString is valid, false otherwise</returns>
    public bool SetDestPortsInclude(string destPortsIncludeString)
    {
      destPortsInclude.Clear();
      if (string.IsNullOrEmpty(destPortsIncludeString))
      {
        return true;
      }

      try
      {
        destPortsInclude.AddRange(ParsePortsString(destPortsIncludeString));
      }
      catch (ArgumentException)
      {
        return false;
      }
      return true;
    }


    /// <summary>
    /// Returns string representation of destination ports exclude values
    /// </summary>
    /// <returns>destination ports exclude as string</returns>
    public string GetDestPortsExcludeString()
    {
      return RangeCollectionToString(destPortsExclude);
    }

    
    /// <summary>
    /// Parses input string and updates destination port exclude filters.
    /// </summary>
    /// <param name="destPortsExcludeString">destination ports exclude as string</param>
    /// <returns>true if destPortsExcludeString is valid, false otherwise</returns>
    public bool SetDestPortsExclude(string destPortsExcludeString)
    {
      destPortsExclude.Clear();
      if (string.IsNullOrEmpty(destPortsExcludeString))
      {
        return true;
      }

      try
      {
        destPortsExclude.AddRange(ParsePortsString(destPortsExcludeString));
      }
      catch (ArgumentException)
      {
        return false;
      }
      return true;
    }


    /// <summary>
    /// Returns allowed protocols list
    /// </summary>
    /// <returns>list of protocols</returns>
    public ICollection<Protocol> GetProtocols()
    {
      return protocols.ToArray();
    }


    public bool SetProtocols(ICollection<int> protocolsCollection)
    {
      protocols.Clear();
      if (null == protocolsCollection)
      {
        return true;
      }
      foreach (int protocol in protocolsCollection)
      {
        protocols.Add((Protocol)protocol);
      }
      return true;
    }


    /// <summary>
    /// Checkes if filters allow the packet
    /// </summary>
    /// <param name="srcPortString">source port as string</param>
    /// <param name="srcIPString">source IP as string</param>
    /// <param name="destPortString">destination port as string</param>
    /// <param name="destIPString">destination IP as string</param>
    /// <param name="protocol">protocol</param>
    /// <returns>true if allow</returns>
    public bool IsPacketAllowed(string srcIPString, string srcPortString,
      string destIPString, string destPortString, Protocol protocol)
    {
      // source IP
      if (!string.IsNullOrEmpty(srcIPString))
      {
        IPAddress srcIP;

        if (!IPAddress.TryParse(srcIPString, out srcIP))
        {
          throw new ArgumentException("Source IP param in not valid");
        }

        if (srcIPsInclude.Count != 0)
        {
          bool isSrcIPAllowed = false;

          foreach (Range<IPAddress> srcIPRange in srcIPsInclude)
          {
            if (IPAddressCompareTo(srcIP, srcIPRange.First) >= 0 &&
              IPAddressCompareTo(srcIP, srcIPRange.Second) <= 0)
            {
              isSrcIPAllowed = true;
              break;
            }
          }
          if (!isSrcIPAllowed)
          {
            return false;
          }
        }
        if (srcIPsExclude.Count != 0)
        {
          foreach (Range<IPAddress> srcIPRange in srcIPsExclude)
          {
            if (IPAddressCompareTo(srcIP, srcIPRange.First) >= 0 &&
              IPAddressCompareTo(srcIP, srcIPRange.Second) <= 0)
            {
              return false;
            }
          }
        }
      }

      // source port
      if (!string.IsNullOrEmpty(srcPortString))
      {
        int srcPort;

        if (!int.TryParse(srcPortString, out srcPort))
        {
          throw new ArgumentException("Source port param in not valid");
        }

        if (srcPortsInclude.Count != 0)
        {
          bool isSrcPortAllowed = false;

          foreach (Range<int> srcPortRange in srcPortsInclude)
          {
            if (srcPort >= srcPortRange.First && srcPort <= srcPortRange.Second)
            {
              isSrcPortAllowed = true;
              break;
            }
          }
          if (!isSrcPortAllowed)
          {
            return false;
          }
        }
        if (srcPortsExclude.Count != 0)
        {
          foreach (Range<int> srcPortRange in srcPortsExclude)
          {
            if (srcPort >= srcPortRange.First && srcPort <= srcPortRange.Second)
            {
              return false;
            }
          }
        }
      }

      // destination IP
      if (!string.IsNullOrEmpty(destIPString))
      {
        IPAddress destIP;

        if (!IPAddress.TryParse(destIPString, out destIP))
        {
          throw new ArgumentException("Destination IP param in not valid");
        }

        if (destIPsInclude.Count != 0)
        {
          bool isDestIPAllowed = false;

          foreach (Range<IPAddress> destIPRange in destIPsInclude)
          {
            if (IPAddressCompareTo(destIP, destIPRange.First) >= 0 &&
              IPAddressCompareTo(destIP, destIPRange.Second) <= 0)
            {
              isDestIPAllowed = true;
              break;
            }
          }
          if (!isDestIPAllowed)
          {
            return false;
          }
        }
        if (destIPsExclude.Count != 0)
        {
          foreach (Range<IPAddress> destIPRange in destIPsExclude)
          {
            if (IPAddressCompareTo(destIP, destIPRange.First) >= 0 &&
              IPAddressCompareTo(destIP, destIPRange.Second) <= 0)
            {
              return false;
            }
          }
        }
      }

      // destination port
      if (!string.IsNullOrEmpty(destPortString))
      {
        int destPort;

        if (!int.TryParse(destPortString, out destPort))
        {
          throw new ArgumentException("Destination port param in not valid");
        }

        if (destPortsInclude.Count != 0)
        {
          bool isDestPortAllowed = false;

          foreach (Range<int> destPortRange in destPortsInclude)
          {
            if (destPort >= destPortRange.First && destPort <= destPortRange.Second)
            {
              isDestPortAllowed = true;
              break;
            }
          }
          if (!isDestPortAllowed)
          {
            return false;
          }
        }
        if (destPortsExclude.Count != 0)
        {
          foreach (Range<int> destPortRange in destPortsExclude)
          {
            if (destPort >= destPortRange.First && destPort <= destPortRange.Second)
            {
              return false;
            }
          }
        }
      }

      // protocol
      if (!protocols.Contains(protocol))
      {
        return false;
      }

      return true;
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
