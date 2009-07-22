using System;
using System.Windows.Forms;


namespace PigSniffer.Forms
{
  public partial class FiltersForm : Form
  {
    /// <summary>
    /// packets filters from PigSnifferForm object
    /// </summary>
    private readonly Filters filters;


    public FiltersForm(Filters filters)
    {
      this.filters = filters;

      InitializeComponent();
    }


    #region Controls events

    private void OKButton_Click(object sender, EventArgs e)
    {
      // TODO: save
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
