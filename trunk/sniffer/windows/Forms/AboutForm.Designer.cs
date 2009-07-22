namespace PigSniffer.Forms
{
  partial class AboutForm
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
      this.components = new System.ComponentModel.Container();
      System.Windows.Forms.Label programmNameLabel;
      System.Windows.Forms.Label copyrightLabel;
      System.Windows.Forms.Label websiteLabel;
      this.OKButton = new System.Windows.Forms.Button();
      this.websiteLinkLabel = new System.Windows.Forms.LinkLabel();
      this.timer1 = new System.Windows.Forms.Timer(this.components);
      programmNameLabel = new System.Windows.Forms.Label();
      copyrightLabel = new System.Windows.Forms.Label();
      websiteLabel = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // programmNameLabel
      // 
      programmNameLabel.AutoSize = true;
      programmNameLabel.Location = new System.Drawing.Point(22, 21);
      programmNameLabel.Name = "programmNameLabel";
      programmNameLabel.Size = new System.Drawing.Size(171, 13);
      programmNameLabel.TabIndex = 1;
      programmNameLabel.Text = "PigSniffer Version 1.0 (22.07.2009)";
      // 
      // copyrightLabel
      // 
      copyrightLabel.AutoSize = true;
      copyrightLabel.Location = new System.Drawing.Point(22, 50);
      copyrightLabel.Name = "copyrightLabel";
      copyrightLabel.Size = new System.Drawing.Size(107, 13);
      copyrightLabel.TabIndex = 2;
      copyrightLabel.Text = "Programmed by arPm";
      // 
      // websiteLabel
      // 
      websiteLabel.AutoSize = true;
      websiteLabel.Location = new System.Drawing.Point(22, 78);
      websiteLabel.Name = "websiteLabel";
      websiteLabel.Size = new System.Drawing.Size(49, 13);
      websiteLabel.TabIndex = 4;
      websiteLabel.Text = "Website:";
      // 
      // OKButton
      // 
      this.OKButton.Location = new System.Drawing.Point(95, 117);
      this.OKButton.Name = "OKButton";
      this.OKButton.Size = new System.Drawing.Size(75, 23);
      this.OKButton.TabIndex = 0;
      this.OKButton.Text = "OK";
      this.OKButton.UseVisualStyleBackColor = true;
      this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
      // 
      // websiteLinkLabel
      // 
      this.websiteLinkLabel.AutoSize = true;
      this.websiteLinkLabel.Location = new System.Drawing.Point(67, 78);
      this.websiteLinkLabel.Name = "websiteLinkLabel";
      this.websiteLinkLabel.Size = new System.Drawing.Size(179, 13);
      this.websiteLinkLabel.TabIndex = 3;
      this.websiteLinkLabel.TabStop = true;
      this.websiteLinkLabel.Text = "http://code.google.com/p/arpmrep/";
      this.websiteLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.websiteLinkLabel_LinkClicked);
      // 
      // AboutForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(265, 157);
      this.Controls.Add(websiteLabel);
      this.Controls.Add(this.websiteLinkLabel);
      this.Controls.Add(copyrightLabel);
      this.Controls.Add(programmNameLabel);
      this.Controls.Add(this.OKButton);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "AboutForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "About PigSniffer";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button OKButton;
    private System.Windows.Forms.Timer timer1;
    private System.Windows.Forms.LinkLabel websiteLinkLabel;
  }
}