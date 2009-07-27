namespace PigSniffer.Forms
{
  partial class FiltersForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.Windows.Forms.GroupBox srcPortsGroupBox;
      System.Windows.Forms.Label srcPortsExcludeLabel;
      System.Windows.Forms.Label srcPortsIncludeLabel;
      System.Windows.Forms.GroupBox srcIPsGroupBox;
      System.Windows.Forms.Label srcIPsExcludeLabel;
      System.Windows.Forms.Label srcIPsIncludeLabel;
      System.Windows.Forms.GroupBox destPortsGroupBox;
      System.Windows.Forms.Label destPortsExcludeLabel;
      System.Windows.Forms.Label destPortsIncludeLabel;
      System.Windows.Forms.GroupBox destIPsGroupBox;
      System.Windows.Forms.Label destIPsExcludeLabel;
      System.Windows.Forms.Label destIPsIncludeLabel;
      this.srcPortsExcludeTextBox = new System.Windows.Forms.TextBox();
      this.srcPortsIncludeTextBox = new System.Windows.Forms.TextBox();
      this.srcIPsExcludeTextBox = new System.Windows.Forms.TextBox();
      this.srcIPsIncludeTextBox = new System.Windows.Forms.TextBox();
      this.destPortsExcludeTextBox = new System.Windows.Forms.TextBox();
      this.destPortsIncludeTextBox = new System.Windows.Forms.TextBox();
      this.destIPsExcludeTextBox = new System.Windows.Forms.TextBox();
      this.destIPsIncludeTextBox = new System.Windows.Forms.TextBox();
      this.OKButton = new System.Windows.Forms.Button();
      this.cancelButton = new System.Windows.Forms.Button();
      this.protocolsListBox = new System.Windows.Forms.ListBox();
      this.protocolsGroupBox = new System.Windows.Forms.GroupBox();
      this.clearButton = new System.Windows.Forms.Button();
      this.selectAllCheckBox = new System.Windows.Forms.CheckBox();
      srcPortsGroupBox = new System.Windows.Forms.GroupBox();
      srcPortsExcludeLabel = new System.Windows.Forms.Label();
      srcPortsIncludeLabel = new System.Windows.Forms.Label();
      srcIPsGroupBox = new System.Windows.Forms.GroupBox();
      srcIPsExcludeLabel = new System.Windows.Forms.Label();
      srcIPsIncludeLabel = new System.Windows.Forms.Label();
      destPortsGroupBox = new System.Windows.Forms.GroupBox();
      destPortsExcludeLabel = new System.Windows.Forms.Label();
      destPortsIncludeLabel = new System.Windows.Forms.Label();
      destIPsGroupBox = new System.Windows.Forms.GroupBox();
      destIPsExcludeLabel = new System.Windows.Forms.Label();
      destIPsIncludeLabel = new System.Windows.Forms.Label();
      srcPortsGroupBox.SuspendLayout();
      srcIPsGroupBox.SuspendLayout();
      destPortsGroupBox.SuspendLayout();
      destIPsGroupBox.SuspendLayout();
      this.protocolsGroupBox.SuspendLayout();
      this.SuspendLayout();
      // 
      // srcPortsGroupBox
      // 
      srcPortsGroupBox.Controls.Add(this.srcPortsExcludeTextBox);
      srcPortsGroupBox.Controls.Add(srcPortsExcludeLabel);
      srcPortsGroupBox.Controls.Add(srcPortsIncludeLabel);
      srcPortsGroupBox.Controls.Add(this.srcPortsIncludeTextBox);
      srcPortsGroupBox.Location = new System.Drawing.Point(376, 12);
      srcPortsGroupBox.Name = "srcPortsGroupBox";
      srcPortsGroupBox.Size = new System.Drawing.Size(348, 81);
      srcPortsGroupBox.TabIndex = 1;
      srcPortsGroupBox.TabStop = false;
      srcPortsGroupBox.Text = "Source port";
      // 
      // srcPortsExcludeTextBox
      // 
      this.srcPortsExcludeTextBox.Location = new System.Drawing.Point(67, 45);
      this.srcPortsExcludeTextBox.Name = "srcPortsExcludeTextBox";
      this.srcPortsExcludeTextBox.Size = new System.Drawing.Size(267, 20);
      this.srcPortsExcludeTextBox.TabIndex = 1;
      // 
      // srcPortsExcludeLabel
      // 
      srcPortsExcludeLabel.AutoSize = true;
      srcPortsExcludeLabel.Location = new System.Drawing.Point(13, 48);
      srcPortsExcludeLabel.Name = "srcPortsExcludeLabel";
      srcPortsExcludeLabel.Size = new System.Drawing.Size(48, 13);
      srcPortsExcludeLabel.TabIndex = 3;
      srcPortsExcludeLabel.Text = "Exclude:";
      // 
      // srcPortsIncludeLabel
      // 
      srcPortsIncludeLabel.AutoSize = true;
      srcPortsIncludeLabel.Location = new System.Drawing.Point(16, 22);
      srcPortsIncludeLabel.Name = "srcPortsIncludeLabel";
      srcPortsIncludeLabel.Size = new System.Drawing.Size(45, 13);
      srcPortsIncludeLabel.TabIndex = 2;
      srcPortsIncludeLabel.Text = "Include:";
      // 
      // srcPortsIncludeTextBox
      // 
      this.srcPortsIncludeTextBox.Location = new System.Drawing.Point(67, 19);
      this.srcPortsIncludeTextBox.Name = "srcPortsIncludeTextBox";
      this.srcPortsIncludeTextBox.Size = new System.Drawing.Size(267, 20);
      this.srcPortsIncludeTextBox.TabIndex = 0;
      // 
      // srcIPsGroupBox
      // 
      srcIPsGroupBox.Controls.Add(this.srcIPsExcludeTextBox);
      srcIPsGroupBox.Controls.Add(srcIPsExcludeLabel);
      srcIPsGroupBox.Controls.Add(srcIPsIncludeLabel);
      srcIPsGroupBox.Controls.Add(this.srcIPsIncludeTextBox);
      srcIPsGroupBox.Location = new System.Drawing.Point(13, 12);
      srcIPsGroupBox.Name = "srcIPsGroupBox";
      srcIPsGroupBox.Size = new System.Drawing.Size(348, 81);
      srcIPsGroupBox.TabIndex = 0;
      srcIPsGroupBox.TabStop = false;
      srcIPsGroupBox.Text = "Source IP";
      // 
      // srcIPsExcludeTextBox
      // 
      this.srcIPsExcludeTextBox.Location = new System.Drawing.Point(67, 45);
      this.srcIPsExcludeTextBox.Name = "srcIPsExcludeTextBox";
      this.srcIPsExcludeTextBox.Size = new System.Drawing.Size(267, 20);
      this.srcIPsExcludeTextBox.TabIndex = 1;
      // 
      // srcIPsExcludeLabel
      // 
      srcIPsExcludeLabel.AutoSize = true;
      srcIPsExcludeLabel.Location = new System.Drawing.Point(13, 48);
      srcIPsExcludeLabel.Name = "srcIPsExcludeLabel";
      srcIPsExcludeLabel.Size = new System.Drawing.Size(48, 13);
      srcIPsExcludeLabel.TabIndex = 3;
      srcIPsExcludeLabel.Text = "Exclude:";
      // 
      // srcIPsIncludeLabel
      // 
      srcIPsIncludeLabel.AutoSize = true;
      srcIPsIncludeLabel.Location = new System.Drawing.Point(16, 22);
      srcIPsIncludeLabel.Name = "srcIPsIncludeLabel";
      srcIPsIncludeLabel.Size = new System.Drawing.Size(45, 13);
      srcIPsIncludeLabel.TabIndex = 2;
      srcIPsIncludeLabel.Text = "Include:";
      // 
      // srcIPsIncludeTextBox
      // 
      this.srcIPsIncludeTextBox.Location = new System.Drawing.Point(67, 19);
      this.srcIPsIncludeTextBox.Name = "srcIPsIncludeTextBox";
      this.srcIPsIncludeTextBox.Size = new System.Drawing.Size(267, 20);
      this.srcIPsIncludeTextBox.TabIndex = 0;
      // 
      // destPortsGroupBox
      // 
      destPortsGroupBox.Controls.Add(this.destPortsExcludeTextBox);
      destPortsGroupBox.Controls.Add(destPortsExcludeLabel);
      destPortsGroupBox.Controls.Add(destPortsIncludeLabel);
      destPortsGroupBox.Controls.Add(this.destPortsIncludeTextBox);
      destPortsGroupBox.Location = new System.Drawing.Point(376, 113);
      destPortsGroupBox.Name = "destPortsGroupBox";
      destPortsGroupBox.Size = new System.Drawing.Size(348, 81);
      destPortsGroupBox.TabIndex = 3;
      destPortsGroupBox.TabStop = false;
      destPortsGroupBox.Text = "Destination port";
      // 
      // destPortsExcludeTextBox
      // 
      this.destPortsExcludeTextBox.Location = new System.Drawing.Point(67, 45);
      this.destPortsExcludeTextBox.Name = "destPortsExcludeTextBox";
      this.destPortsExcludeTextBox.Size = new System.Drawing.Size(267, 20);
      this.destPortsExcludeTextBox.TabIndex = 1;
      // 
      // destPortsExcludeLabel
      // 
      destPortsExcludeLabel.AutoSize = true;
      destPortsExcludeLabel.Location = new System.Drawing.Point(13, 48);
      destPortsExcludeLabel.Name = "destPortsExcludeLabel";
      destPortsExcludeLabel.Size = new System.Drawing.Size(48, 13);
      destPortsExcludeLabel.TabIndex = 3;
      destPortsExcludeLabel.Text = "Exclude:";
      // 
      // destPortsIncludeLabel
      // 
      destPortsIncludeLabel.AutoSize = true;
      destPortsIncludeLabel.Location = new System.Drawing.Point(16, 22);
      destPortsIncludeLabel.Name = "destPortsIncludeLabel";
      destPortsIncludeLabel.Size = new System.Drawing.Size(45, 13);
      destPortsIncludeLabel.TabIndex = 2;
      destPortsIncludeLabel.Text = "Include:";
      // 
      // destPortsIncludeTextBox
      // 
      this.destPortsIncludeTextBox.Location = new System.Drawing.Point(67, 19);
      this.destPortsIncludeTextBox.Name = "destPortsIncludeTextBox";
      this.destPortsIncludeTextBox.Size = new System.Drawing.Size(267, 20);
      this.destPortsIncludeTextBox.TabIndex = 0;
      // 
      // destIPsGroupBox
      // 
      destIPsGroupBox.Controls.Add(this.destIPsExcludeTextBox);
      destIPsGroupBox.Controls.Add(destIPsExcludeLabel);
      destIPsGroupBox.Controls.Add(destIPsIncludeLabel);
      destIPsGroupBox.Controls.Add(this.destIPsIncludeTextBox);
      destIPsGroupBox.Location = new System.Drawing.Point(13, 113);
      destIPsGroupBox.Name = "destIPsGroupBox";
      destIPsGroupBox.Size = new System.Drawing.Size(348, 81);
      destIPsGroupBox.TabIndex = 2;
      destIPsGroupBox.TabStop = false;
      destIPsGroupBox.Text = "Destination IP";
      // 
      // destIPsExcludeTextBox
      // 
      this.destIPsExcludeTextBox.Location = new System.Drawing.Point(67, 45);
      this.destIPsExcludeTextBox.Name = "destIPsExcludeTextBox";
      this.destIPsExcludeTextBox.Size = new System.Drawing.Size(267, 20);
      this.destIPsExcludeTextBox.TabIndex = 1;
      // 
      // destIPsExcludeLabel
      // 
      destIPsExcludeLabel.AutoSize = true;
      destIPsExcludeLabel.Location = new System.Drawing.Point(13, 48);
      destIPsExcludeLabel.Name = "destIPsExcludeLabel";
      destIPsExcludeLabel.Size = new System.Drawing.Size(48, 13);
      destIPsExcludeLabel.TabIndex = 3;
      destIPsExcludeLabel.Text = "Exclude:";
      // 
      // destIPsIncludeLabel
      // 
      destIPsIncludeLabel.AutoSize = true;
      destIPsIncludeLabel.Location = new System.Drawing.Point(16, 22);
      destIPsIncludeLabel.Name = "destIPsIncludeLabel";
      destIPsIncludeLabel.Size = new System.Drawing.Size(45, 13);
      destIPsIncludeLabel.TabIndex = 2;
      destIPsIncludeLabel.Text = "Include:";
      // 
      // destIPsIncludeTextBox
      // 
      this.destIPsIncludeTextBox.Location = new System.Drawing.Point(67, 19);
      this.destIPsIncludeTextBox.Name = "destIPsIncludeTextBox";
      this.destIPsIncludeTextBox.Size = new System.Drawing.Size(267, 20);
      this.destIPsIncludeTextBox.TabIndex = 0;
      // 
      // OKButton
      // 
      this.OKButton.Location = new System.Drawing.Point(223, 407);
      this.OKButton.Name = "OKButton";
      this.OKButton.Size = new System.Drawing.Size(75, 23);
      this.OKButton.TabIndex = 5;
      this.OKButton.Text = "OK";
      this.OKButton.UseVisualStyleBackColor = true;
      this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
      // 
      // cancelButton
      // 
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(331, 407);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(75, 23);
      this.cancelButton.TabIndex = 6;
      this.cancelButton.Text = "Cancel";
      this.cancelButton.UseVisualStyleBackColor = true;
      this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
      // 
      // protocolsListBox
      // 
      this.protocolsListBox.FormattingEnabled = true;
      this.protocolsListBox.Location = new System.Drawing.Point(15, 19);
      this.protocolsListBox.Name = "protocolsListBox";
      this.protocolsListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
      this.protocolsListBox.Size = new System.Drawing.Size(263, 134);
      this.protocolsListBox.TabIndex = 0;
      this.protocolsListBox.SelectedIndexChanged += new System.EventHandler(this.protocolsListBox_SelectedIndexChanged);
      // 
      // protocolsGroupBox
      // 
      this.protocolsGroupBox.Controls.Add(this.selectAllCheckBox);
      this.protocolsGroupBox.Controls.Add(this.protocolsListBox);
      this.protocolsGroupBox.Location = new System.Drawing.Point(222, 213);
      this.protocolsGroupBox.Name = "protocolsGroupBox";
      this.protocolsGroupBox.Size = new System.Drawing.Size(292, 183);
      this.protocolsGroupBox.TabIndex = 4;
      this.protocolsGroupBox.TabStop = false;
      this.protocolsGroupBox.Text = "Protocol";
      // 
      // clearButton
      // 
      this.clearButton.Location = new System.Drawing.Point(439, 407);
      this.clearButton.Name = "clearButton";
      this.clearButton.Size = new System.Drawing.Size(75, 23);
      this.clearButton.TabIndex = 7;
      this.clearButton.Text = "Clear";
      this.clearButton.UseVisualStyleBackColor = true;
      this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
      // 
      // selectAllCheckBox
      // 
      this.selectAllCheckBox.AutoSize = true;
      this.selectAllCheckBox.Location = new System.Drawing.Point(15, 159);
      this.selectAllCheckBox.Name = "selectAllCheckBox";
      this.selectAllCheckBox.Size = new System.Drawing.Size(70, 17);
      this.selectAllCheckBox.TabIndex = 1;
      this.selectAllCheckBox.Text = "Select All";
      this.selectAllCheckBox.UseVisualStyleBackColor = true;
      this.selectAllCheckBox.Click += new System.EventHandler(this.selectAllCheckBox_Click);
      // 
      // FiltersForm
      // 
      this.AcceptButton = this.OKButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(736, 443);
      this.Controls.Add(this.clearButton);
      this.Controls.Add(this.protocolsGroupBox);
      this.Controls.Add(destIPsGroupBox);
      this.Controls.Add(destPortsGroupBox);
      this.Controls.Add(srcIPsGroupBox);
      this.Controls.Add(srcPortsGroupBox);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.OKButton);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FiltersForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Filters";
      srcPortsGroupBox.ResumeLayout(false);
      srcPortsGroupBox.PerformLayout();
      srcIPsGroupBox.ResumeLayout(false);
      srcIPsGroupBox.PerformLayout();
      destPortsGroupBox.ResumeLayout(false);
      destPortsGroupBox.PerformLayout();
      destIPsGroupBox.ResumeLayout(false);
      destIPsGroupBox.PerformLayout();
      this.protocolsGroupBox.ResumeLayout(false);
      this.protocolsGroupBox.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TextBox srcPortsIncludeTextBox;
    private System.Windows.Forms.TextBox srcIPsIncludeTextBox;
    private System.Windows.Forms.TextBox destIPsIncludeTextBox;
    private System.Windows.Forms.TextBox destPortsIncludeTextBox;
    private System.Windows.Forms.Button OKButton;
    private new System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.TextBox srcPortsExcludeTextBox;
    private System.Windows.Forms.TextBox srcIPsExcludeTextBox;
    private System.Windows.Forms.TextBox destPortsExcludeTextBox;
    private System.Windows.Forms.TextBox destIPsExcludeTextBox;
    private System.Windows.Forms.ListBox protocolsListBox;
    private System.Windows.Forms.GroupBox protocolsGroupBox;
    private System.Windows.Forms.Button clearButton;
    private System.Windows.Forms.CheckBox selectAllCheckBox;
  }
}