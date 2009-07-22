using System.Diagnostics;
using System.Windows.Forms;


namespace PigSniffer.Forms
{
  public partial class AboutForm : Form
  {
    public AboutForm()
    {
      InitializeComponent();
    }


    #region Controls events

    private void OKButton_Click(object sender, System.EventArgs e)
    {
      Close();
    }

    
    private void websiteLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Process.Start(websiteLinkLabel.Text);
    }

    #endregion
    
  }

}
