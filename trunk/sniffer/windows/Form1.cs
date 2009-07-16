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
      socket.Bind(new IPEndPoint(IPAddress.Any, 0)); // TODO: obtain data
      socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);

//      var byTrue = new byte[]{1, 0, 0, 0};
//      var byOut = new byte[4];

//      socket.IOControl(IOControlCode.ReceiveAll, byTrue, byOut);

      isOnline = true;
      socket.BeginReceive(socketData, 0, socketData.Length, SocketFlags.None,
        new AsyncCallback(SocketCallback), null);
      
      startButton.Enabled = false;
      stopButton.Enabled = true;
    }

    
    private void stopButton_Click(object sender, EventArgs e)
    {
      isOnline = false;

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
      var ethernetHeader = new IPHeader(data, count);

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
      var dataStringBuilder = new StringBuilder();

      packetTreeView.BeginUpdate();
      packetTreeView.Nodes.Clear();
      while (packet != null)
      {
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
        
        packet = packet.GetInnerPacket();
      }
      packetTreeView.EndUpdate();
      packetRichTextBox.Text = dataStringBuilder.ToString();
    }
  }
}
