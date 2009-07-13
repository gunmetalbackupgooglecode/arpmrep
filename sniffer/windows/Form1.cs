using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace PigSniffer
{
  public partial class Form1 : Form
  {
    private Thread snifferThread;


    public Form1()
    {
      InitializeComponent();
    }


    private void startButton_Click(object sender, EventArgs e)
    {
      // obtain data

      // set promiscuous mode

      snifferThread = new Thread(SnifferThread);
      snifferThread.Start();
    }

    
    private void stopButton_Click(object sender, EventArgs e)
    {
      snifferThread.Abort();
    }

    
    private void SnifferThread()
    {
      try
      {
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);
        byte[] byTrue = new byte[4]{1, 0, 0, 0};
        byte[] byOut = new byte[4];

        socket.Bind(new IPEndPoint(IPAddress.Any, 0));
        socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);
//        socket.IOControl(IOControlCode.ReceiveAll, byTrue, byOut);

        byte[] data = new byte[255];
        int count = 0;

        while (true)
        {
          try
          {
            count = socket.Receive(data);
            IPHeader ethernetHeader = new IPHeader(data, count);

            ListViewItem item = new ListViewItem(new []{"a"});
            packetsListView.Items.Add(item);
            
          }
          catch (Exception ex)
          {
            MessageBox.Show("Error");
          }
          MessageBox.Show("Get " + count + "bytes");
        }

      }
      catch (ThreadAbortException)
      {
        // unset promiscuous mode
      }
    }


    private void ParseData(byte[] data, int size)
    {
      
    }
  }
}
