using System;
using System.Net;
using System.Net.Sockets;
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
    private readonly byte[] socketData = new byte[4096]; // TODO: define constant
    /// <summary>
    /// current packet number
    /// </summary>
    private int packetNumber;


    public Form1()
    {
      InitializeComponent();
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
    }

    
    private void stopButton_Click(object sender, EventArgs e)
    {
      isOnline = false;
    }

    
    private void SocketCallback(IAsyncResult ar)
    {
      try
      {
        int count = socket.EndReceive(ar);
        var ethernetHeader = new IPHeader(socketData, count);

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


    private void ParseData(byte[] data, int size)
    {
      
    }
  }
}
