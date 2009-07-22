using System.Collections.Generic;

namespace PigSniffer
{
  /// <summary>
  /// Represents values' pair.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  class Range<T>
  {
    public T First { get; private set; }
    public T Second { get; private set; }


    public Range(T first, T second)
    {
      First = first;
      Second = second;
    }

  }


  /// <summary>
  /// Packets' filters: source port, source IP, destination port, destination IP.
  /// </summary>
  public class Filters
  {
    private readonly List<Range<int>> srcPorts = new List<Range<int>>();


    /// <summary>
    /// Parses input string and updates source port filters.
    /// </summary>
    /// <param name="srcPortsString">sourse ports as string</param>
    /// <returns>true if srcPortsString is valid, false otherwise</returns>
    public bool SetSrcPorts(string srcPortsString)
    {
      if (string.IsNullOrEmpty(srcPortsString))
      {
        srcPorts.Clear();
        return true;
      }

      string[] srsPortsSubstrings = srcPortsString.Split(new[] {','});

      return true;
    }

  }

}
