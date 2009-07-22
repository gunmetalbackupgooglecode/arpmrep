using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using PigSniffer.Packets;
using Timer=System.Threading.Timer;


namespace PigSniffer.Forms
{
  public partial class PigSnifferForm : Form
  {
    private const int PACKETS_LIST_VIEW_UPDATE_TIMEOUT = 1000;

    /// <summary>
    /// list of network interfaces
    /// </summary>
    private readonly List<NetworkInterface> networkInterfacesList = new List<NetworkInterface>();
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
    /// <summary>
    /// Temporary (or not) solution of ListView flicker problem.
    /// System updates ListView every PACKETS_LIST_VIEW_UPDATE_TIMEOUT.
    /// </summary>
    private readonly List<ListViewItem> updatesForPacketsListView = new List<ListViewItem>();
    /// <summary>
    /// Timer for ListView update
    /// </summary>
    private readonly Timer updatePacketsListViewTimer;


    public PigSnifferForm()
    {
      updatePacketsListViewTimer = new Timer(UpdatePacketsListViewCallback, null, Timeout.Infinite, Timeout.Infinite);

      InitializeComponent();

      MinimumSize = new Size(Size.Width, Size.Height);

      foreach(NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
      {
        networkInterfacesList.Add(networkInterface);

        string addresses;
        UnicastIPAddressInformationCollection addressInfoCollection =
          networkInterface.GetIPProperties().UnicastAddresses;

        if (0 == addressInfoCollection.Count)
        {
          addresses = "no IPs";
        }
        else
        {
          var addressedStringBuilder = new StringBuilder();

          foreach (UnicastIPAddressInformation addressInfo in addressInfoCollection)
          {
            addressedStringBuilder.Append(addressInfo.Address.ToString());
            addressedStringBuilder.Append(", ");
          }
          addressedStringBuilder.Remove(addressedStringBuilder.Length - 2, 2);
          addresses = addressedStringBuilder.ToString();
        }

        nicsComboBox.Items.Add(string.Format("{0} ({1})", networkInterface.Name, addresses));
      }

      stopButton.Enabled = false;
    }


    #region Form events

    private void pigSnifferForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (isOnline)
      {
        stopButton.PerformClick();
      }
    }

    #endregion


    #region Controls events

    private void startButton_Click(object sender, EventArgs e)
    {
      // get bind IP
      int networkInterfaceIndex = nicsComboBox.SelectedIndex;

      if (-1 == networkInterfaceIndex)
      {
        MessageBox.Show("Choose network interface please", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }

      NetworkInterface networkInterface = networkInterfacesList[networkInterfaceIndex];
      UnicastIPAddressInformationCollection addressInfoCollection =
        networkInterface.GetIPProperties().UnicastAddresses;

      if (0 == addressInfoCollection.Count)
      {
        MessageBox.Show("Selected network interface does not have any IP", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }

      IPAddress bindIP = addressInfoCollection[0].Address;

      socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);
      socket.Bind(new IPEndPoint(bindIP, 0));
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
      updatePacketsListViewTimer.Change(PACKETS_LIST_VIEW_UPDATE_TIMEOUT, PACKETS_LIST_VIEW_UPDATE_TIMEOUT);
      socket.BeginReceive(socketData, 0, socketData.Length, SocketFlags.None,
                          new AsyncCallback(SocketCallback), null);

      startButton.Enabled = false;
      stopButton.Enabled = true;
    }


    private void stopButton_Click(object sender, EventArgs e)
    {
      isOnline = false;
      updatePacketsListViewTimer.Change(Timeout.Infinite, Timeout.Infinite);

      try
      {
        // disable promiscuous mode
        var inValue = BitConverter.GetBytes(0); // RCVALL_OFF
        var outValue = BitConverter.GetBytes(0);

        socket.IOControl(IOControlCode.ReceiveAll, inValue, outValue);
        socket.Close();
      }
      catch (SocketException ex)
      {
        MessageBox.Show("Could not disable promiscuous mode. Socket exception error code = " + ex.ErrorCode,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      startButton.Enabled = true;
      stopButton.Enabled = false;
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

      byte[] packetData = packet.GetInnerData();
      
      if (packetData.Length != 0)
      {
        dataStringBuilder.AppendLine();
        dataStringBuilder.Append("Data: ");
        foreach (byte b in packetData)
        {
          dataStringBuilder.AppendFormat("{0:X}", b);
        }
        dataStringBuilder.AppendLine();
        dataStringBuilder.AppendLine();
        dataStringBuilder.Append("Data text view: ");
        foreach (byte b in packetData)
        {
          char c = (0 == b) ? ' ' : (char)b;
          dataStringBuilder.Append(c);
        }
      }

      packetRichTextBox.Text = dataStringBuilder.ToString();
    }


    private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
      var aboutForm = new AboutForm();

      aboutForm.ShowDialog();
      aboutForm.Dispose();
    }
    
    #endregion


    private delegate void AddPacketsListViewItemDelegate(List<ListViewItem> items);
    private void AddPacketsListViewItemCallback(List<ListViewItem> items)
    {
      packetsListView.Items.AddRange(items.ToArray());
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
        Close();
      }
      catch (ObjectDisposedException)
      {
        // stop button was clicked - do nothing
      }
    }


    private void ParseData(byte[] data, int count)
    {
      var ethernetHeader = new IPPacket(data, count);

      packets.Add(ethernetHeader);
      ++ packetNumber;

      var item = new ListViewItem(new[] { packetNumber.ToString(),
                                          ethernetHeader.GetSrcIPAddressString(), ethernetHeader.GetSrcPortString(),
                                          ethernetHeader.GetDestIPAddressString(), ethernetHeader.GetDestPortString(),
                                          ethernetHeader.GetProtocolString()});
      
      lock (updatesForPacketsListView)
      {
        updatesForPacketsListView.Add(item);
      }
    }


    private void UpdatePacketsListViewCallback(object stateInfo)
    {
      lock (updatesForPacketsListView)
      {
        if (InvokeRequired)
        {
          var addDelegate = new AddPacketsListViewItemDelegate(AddPacketsListViewItemCallback);
          Invoke(addDelegate, updatesForPacketsListView);
        }
        else
        {
          AddPacketsListViewItemCallback(updatesForPacketsListView);
        }
        updatesForPacketsListView.Clear();
      }
    }

  }

}
