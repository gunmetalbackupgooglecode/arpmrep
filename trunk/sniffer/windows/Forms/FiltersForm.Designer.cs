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
      System.Windows.Forms.Label srcPortsLabel;
      System.Windows.Forms.Label srcIPsLabel;
      System.Windows.Forms.Label destIPsLabel;
      System.Windows.Forms.Label destPortsLabel;
      this.srcPortsTextBox = new System.Windows.Forms.TextBox();
      this.srcIPsTextBox = new System.Windows.Forms.TextBox();
      this.destIPsTextBox = new System.Windows.Forms.TextBox();
      this.destPortsTextBox = new System.Windows.Forms.TextBox();
      this.OKButton = new System.Windows.Forms.Button();
      this.CancelButton = new System.Windows.Forms.Button();
      srcPortsLabel = new System.Windows.Forms.Label();
      srcIPsLabel = new System.Windows.Forms.Label();
      destIPsLabel = new System.Windows.Forms.Label();
      destPortsLabel = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // srcPortsLabel
      // 
      srcPortsLabel.AutoSize = true;
      srcPortsLabel.Location = new System.Drawing.Point(40, 41);
      srcPortsLabel.Name = "srcPortsLabel";
      srcPortsLabel.Size = new System.Drawing.Size(71, 13);
      srcPortsLabel.TabIndex = 0;
      srcPortsLabel.Text = "Source Ports:";
      // 
      // srcIPsLabel
      // 
      srcIPsLabel.AutoSize = true;
      srcIPsLabel.Location = new System.Drawing.Point(49, 82);
      srcIPsLabel.Name = "srcIPsLabel";
      srcIPsLabel.Size = new System.Drawing.Size(62, 13);
      srcIPsLabel.TabIndex = 2;
      srcIPsLabel.Text = "Source IPs:";
      // 
      // destIPsLabel
      // 
      destIPsLabel.AutoSize = true;
      destIPsLabel.Location = new System.Drawing.Point(30, 167);
      destIPsLabel.Name = "destIPsLabel";
      destIPsLabel.Size = new System.Drawing.Size(81, 13);
      destIPsLabel.TabIndex = 6;
      destIPsLabel.Text = "Destination IPs:";
      // 
      // destPortsLabel
      // 
      destPortsLabel.AutoSize = true;
      destPortsLabel.Location = new System.Drawing.Point(21, 126);
      destPortsLabel.Name = "destPortsLabel";
      destPortsLabel.Size = new System.Drawing.Size(90, 13);
      destPortsLabel.TabIndex = 4;
      destPortsLabel.Text = "Destination Ports:";
      // 
      // srcPortsTextBox
      // 
      this.srcPortsTextBox.Location = new System.Drawing.Point(117, 38);
      this.srcPortsTextBox.Name = "srcPortsTextBox";
      this.srcPortsTextBox.Size = new System.Drawing.Size(267, 20);
      this.srcPortsTextBox.TabIndex = 1;
      // 
      // srcIPsTextBox
      // 
      this.srcIPsTextBox.Location = new System.Drawing.Point(117, 79);
      this.srcIPsTextBox.Name = "srcIPsTextBox";
      this.srcIPsTextBox.Size = new System.Drawing.Size(267, 20);
      this.srcIPsTextBox.TabIndex = 3;
      // 
      // destIPsTextBox
      // 
      this.destIPsTextBox.Location = new System.Drawing.Point(117, 164);
      this.destIPsTextBox.Name = "destIPsTextBox";
      this.destIPsTextBox.Size = new System.Drawing.Size(267, 20);
      this.destIPsTextBox.TabIndex = 7;
      // 
      // destPortsTextBox
      // 
      this.destPortsTextBox.Location = new System.Drawing.Point(117, 123);
      this.destPortsTextBox.Name = "destPortsTextBox";
      this.destPortsTextBox.Size = new System.Drawing.Size(267, 20);
      this.destPortsTextBox.TabIndex = 5;
      // 
      // OKButton
      // 
      this.OKButton.Location = new System.Drawing.Point(163, 291);
      this.OKButton.Name = "OKButton";
      this.OKButton.Size = new System.Drawing.Size(75, 23);
      this.OKButton.TabIndex = 8;
      this.OKButton.Text = "OK";
      this.OKButton.UseVisualStyleBackColor = true;
      this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
      // 
      // CancelButton
      // 
      this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.CancelButton.Location = new System.Drawing.Point(279, 291);
      this.CancelButton.Name = "CancelButton";
      this.CancelButton.Size = new System.Drawing.Size(75, 23);
      this.CancelButton.TabIndex = 9;
      this.CancelButton.Text = "Cancel";
      this.CancelButton.UseVisualStyleBackColor = true;
      this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
      // 
      // FiltersForm
      // 
      this.AcceptButton = this.OKButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(516, 346);
      this.Controls.Add(this.CancelButton);
      this.Controls.Add(this.OKButton);
      this.Controls.Add(this.destIPsTextBox);
      this.Controls.Add(destIPsLabel);
      this.Controls.Add(this.destPortsTextBox);
      this.Controls.Add(destPortsLabel);
      this.Controls.Add(this.srcIPsTextBox);
      this.Controls.Add(srcIPsLabel);
      this.Controls.Add(this.srcPortsTextBox);
      this.Controls.Add(srcPortsLabel);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FiltersForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Filters";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox srcPortsTextBox;
    private System.Windows.Forms.TextBox srcIPsTextBox;
    private System.Windows.Forms.TextBox destIPsTextBox;
    private System.Windows.Forms.TextBox destPortsTextBox;
    private System.Windows.Forms.Button OKButton;
    private new System.Windows.Forms.Button CancelButton;
  }
}