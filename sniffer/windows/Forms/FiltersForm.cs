using System;
using System.Windows.Forms;


namespace PigSniffer.Forms
{
  public partial class FiltersForm : Form
  {
    private Filters filters;


    public FiltersForm(Filters initFilters)
    {
      InitializeComponent();

      srcPortsTextBox.Text = initFilters.GetSrcPortsString();
      srcIPsTextBox.Text = initFilters.GetSrcIPsString();
      destPortsTextBox.Text = initFilters.GetDestPortsString();
      destIPsTextBox.Text = initFilters.GetDestIPsString();
    }


    #region Controls events

    private void OKButton_Click(object sender, EventArgs e)
    {
      filters = new Filters();

      bool isError = false;
      string errorMessage = "";

      if (!filters.SetSrcPorts(srcPortsTextBox.Text))
      {
        isError = true;
        errorMessage = "Source ports value is invalid";
      }
      if (!filters.SetSrcIPs(srcIPsTextBox.Text))
      {
        isError = true;
        errorMessage = "Source IPs value is invalid";
      }
      if (!filters.SetDestPorts(destPortsTextBox.Text))
      {
        isError = true;
        errorMessage = "Destination ports value is invalid";
      }
      if (!filters.SetDestIPs(destIPsTextBox.Text))
      {
        isError = true;
        errorMessage = "Destination IPs value is invalid";
      }

      if (isError)
      {
        MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }

      DialogResult = DialogResult.OK;
      Close();
    }


    private void CancelButton_Click(object sender, EventArgs e)
    {
      Close();
    }

    #endregion


    public Filters GetFilters()
    {
      return filters;
    }

  }

}
