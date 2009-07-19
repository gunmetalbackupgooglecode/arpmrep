using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace PigSniffer
{
  public partial class Form1 : Form
  {
    /// <summary>
    /// true if PigSniffer is capturing packets right now
    /// </summary>
    private bool isOnline;
    /// <summary>
    /// working socket
    /// </summary>
    private Socket socket;
    /// <summary>
    /// array of received bytes
    /// </summary>
    private readonly byte[] socketData = new byte[Packet.MAX_PACKET_SIZE];
    /// <summary>
    /// current packet number
    /// </summary>
    private int packetNumber;
    /// <summary>
    /// list of received packets
    /// </summary>
    private readonly List<Packet> packets = new List<Packet>();


    public Form1()
    {
      InitializeComponent();

      stopButton.Enabled = false;
    }


    private delegate void AddPacketsListViewItemDelegate(ListViewItem item);
    private void AddPacketsListViewItemCallback(ListViewItem item)
    {
      packetsListView.Items.Add(item);
    }


    private void startButton_Click(object sender, EventArgs e)
    {
      socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);
      socket.Bind(new IPEndPoint(IPAddress.Parse("192.168.61.148"), 0)); // TODO: obtain data
      socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);

      try
      {
        // http://msdn.microsoft.com/en-us/library/ms741621(VS.85).aspx
        // try to enable a socket to receive all IP packets on the network
        // limits: socket cannot be binded to IPAddress.Any
        var inValue = BitConverter.GetBytes(1); // RCVALL_ON
        var outValue = BitConverter.GetBytes(0);

        socket.IOControl(IOControlCode.ReceiveAll, inValue, outValue);
      }
      catch (SocketException ex)
      {
        MessageBox.Show("Could not enable promiscuous mode. Socket exception error code = " + ex.ErrorCode,
          "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      isOnline = true;
      socket.BeginReceive(socketData, 0, socketData.Length, SocketFlags.None,
        new AsyncCallback(SocketCallback), null);
      
      startButton.Enabled = false;
      stopButton.Enabled = true;
    }

    
    private void stopButton_Click(object sender, EventArgs e)
    {
      isOnline = false;

      try
      {
        // disable promiscuous mode
        var inValue = BitConverter.GetBytes(0); // RCVALL_OFF
        var outValue = BitConverter.GetBytes(0);

        socket.IOControl(IOControlCode.ReceiveAll, inValue, outValue);
      }
      catch (SocketException ex)
      {
        MessageBox.Show("Could not disable promiscuous mode. Socket exception error code = " + ex.ErrorCode,
          "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      startButton.Enabled = true;
      stopButton.Enabled = false;
    }

    
    private void SocketCallback(IAsyncResult ar)
    {
      try
      {
        int count = socket.EndReceive(ar);

        ParseData(socketData, count);

        if (isOnline)
        {
          socket.BeginReceive(socketData, 0, socketData.Length, SocketFlags.None,
            new AsyncCallback(SocketCallback), null);
        }
      }
      catch (SocketException ex)
      {
        MessageBox.Show("Error: " + ex);
      }
    }


    private void ParseData(byte[] data, int count)
    {
      var ethernetHeader = new IPPacket(data, count);

      packets.Add(ethernetHeader);
      ++ packetNumber;

      var item = new ListViewItem(new[] { packetNumber.ToString(),
          ethernetHeader.GetSrcIPAddressString(), "",
          ethernetHeader.GetDestIPAddressString(), "",
          ethernetHeader.GetProtocolString()});
      
      if (InvokeRequired)
      {
        var addDelegate = new AddPacketsListViewItemDelegate(AddPacketsListViewItemCallback);
        Invoke(addDelegate, item);
      }
      else
      {
        AddPacketsListViewItemCallback(item);
      }
    }


    private void packetsListView_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (0 == packetsListView.SelectedIndices.Count)
      {
        return;
      }

      Packet packet = packets[packetsListView.SelectedIndices[0]];
      Packet nextPacket = packet;
      var dataStringBuilder = new StringBuilder();

      packetTreeView.BeginUpdate();
      packetTreeView.Nodes.Clear();
      while (nextPacket != null)
      {
        packet = nextPacket;
        TreeNode curTreeNode = packetTreeView.Nodes.Add(packet.GetName());

        foreach (string headerValue in packet.GetHeaderValues())
        {
          curTreeNode.Nodes.Add(headerValue);
        }

        dataStringBuilder.Append(packet.GetName() + ": ");
        foreach (byte b in packet.GetHeader())
        {
          dataStringBuilder.AppendFormat("{0:X}", b);
        }
        dataStringBuilder.AppendLine();
        
        nextPacket = packet.GetInnerPacket();
      }
      packetTreeView.EndUpdate();
      dataStringBuilder.Append("Data: ");
      foreach (byte b in packet.GetInnerData())
      {
        dataStringBuilder.AppendFormat("{0:X}", b);
      }
      packetRichTextBox.Text = dataStringBuilder.ToString();
    }
  }
}
