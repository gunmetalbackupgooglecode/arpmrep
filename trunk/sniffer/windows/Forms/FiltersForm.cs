using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using PigSniffer.Packets;


namespace PigSniffer.Forms
{
  public partial class FiltersForm : Form
  {
    private Filters filters;


    public FiltersForm(Filters initFilters)
    {
      InitializeComponent();

      srcIPsIncludeTextBox.Text = initFilters.GetSrcIPsIncludeString();
      srcIPsExcludeTextBox.Text = initFilters.GetSrcIPsExcludeString();
      srcPortsIncludeTextBox.Text = initFilters.GetSrcPortsIncludeString();
      srcPortsExcludeTextBox.Text = initFilters.GetSrcPortsExcludeString();
      destIPsIncludeTextBox.Text = initFilters.GetDestIPsIncludeString();
      destIPsExcludeTextBox.Text = initFilters.GetDestIPsExcludeString();
      destPortsIncludeTextBox.Text = initFilters.GetDestPortsIncludeString();
      destPortsExcludeTextBox.Text = initFilters.GetDestPortsExcludeString();
      protocolsListBox.Items.AddRange(Enum.GetNames(typeof(Protocol)));

      var protocolsValues = new ArrayList(Enum.GetValues(typeof(Protocol)));
      
      foreach (Protocol protocol in initFilters.GetProtocols())
      {
        protocolsListBox.SetSelected(protocolsValues.IndexOf(protocol), true);
      }

      selectAllCheckBox.Checked = (protocolsListBox.Items.Count == protocolsListBox.SelectedIndices.Count);
    }


    #region Controls events

    private void protocolsListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      selectAllCheckBox.Checked = (protocolsListBox.Items.Count == protocolsListBox.SelectedIndices.Count);
    }


    private void selectAllCheckBox_Click(object sender, EventArgs e)
    {
      if (selectAllCheckBox.Checked)
      {
        for (int i = 0; i < protocolsListBox.Items.Count; ++i)
        {
          protocolsListBox.SetSelected(i, true);
        }
      }
      else
      {
        protocolsListBox.ClearSelected();
      }
    }

    
    private void OKButton_Click(object sender, EventArgs e)
    {
      filters = new Filters();

      bool isError = false;
      string errorMessage = "";

      if (!filters.SetSrcIPsInclude(srcIPsIncludeTextBox.Text))
      {
        isError = true;
        errorMessage = "Source IPs include value is invalid";
      }
      else if (!filters.SetSrcIPsExclude(srcIPsExcludeTextBox.Text))
      {
        isError = true;
        errorMessage = "Source IPs exclude value is invalid";
      }
      else if (!filters.SetSrcPortsInclude(srcPortsIncludeTextBox.Text))
      {
        isError = true;
        errorMessage = "Source ports include value is invalid";
      }
      else if (!filters.SetSrcPortsExclude(srcPortsExcludeTextBox.Text))
      {
        isError = true;
        errorMessage = "Source ports exclude value is invalid";
      }
      else if (!filters.SetDestIPsInclude(destIPsIncludeTextBox.Text))
      {
        isError = true;
        errorMessage = "Destination IPs include value is invalid";
      }
      else if (!filters.SetDestIPsExclude(destIPsExcludeTextBox.Text))
      {
        isError = true;
        errorMessage = "Destination IPs exclude value is invalid";
      }
      else if (!filters.SetDestPortsInclude(destPortsIncludeTextBox.Text))
      {
        isError = true;
        errorMessage = "Destination ports include value is invalid";
      }
      else if (!filters.SetDestPortsExclude(destPortsExcludeTextBox.Text))
      {
        isError = true;
        errorMessage = "Destination ports exclude value is invalid";
      }
      else
      {
        var selectedProtocols = new List<int>();
        Array protocolValues = Enum.GetValues(typeof(Protocol));

        foreach (int index in protocolsListBox.SelectedIndices)
        {
          selectedProtocols.Add((int)protocolValues.GetValue(index));
        }
        if (!filters.SetProtocols(selectedProtocols))
        {
          isError = true;
          errorMessage = "Destination IPs value is invalid";
        }
      }


      if (isError)
      {
        MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }

      DialogResult = DialogResult.OK;
      Close();
    }


    private void cancelButton_Click(object sender, EventArgs e)
    {
      Close();
    }

    
    private void clearButton_Click(object sender, EventArgs e)
    {
      srcIPsIncludeTextBox.Text = "";
      srcIPsExcludeTextBox.Text = "";
      srcPortsIncludeTextBox.Text = "";
      srcPortsExcludeTextBox.Text = "";
      destIPsIncludeTextBox.Text = "";
      destIPsExcludeTextBox.Text = "";
      destPortsIncludeTextBox.Text = "";
      destPortsExcludeTextBox.Text = "";
      for (int i = 0; i < protocolsListBox.Items.Count; ++ i)
      {
        protocolsListBox.SetSelected(i, true);
      }
      selectAllCheckBox.Checked = true;
    }

    #endregion


    public Filters GetFilters()
    {
      return filters;
    }

  }

}
