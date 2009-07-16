namespace PigSniffer
{
  partial class Form1
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
      this.ipTextBox = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.startButton = new System.Windows.Forms.Button();
      this.stopButton = new System.Windows.Forms.Button();
      this.packetsListView = new System.Windows.Forms.ListView();
      this.numberColumn = new System.Windows.Forms.ColumnHeader();
      this.srcIPColumn = new System.Windows.Forms.ColumnHeader();
      this.srcPorCcolumn = new System.Windows.Forms.ColumnHeader();
      this.destIPColumn = new System.Windows.Forms.ColumnHeader();
      this.destPortColumn = new System.Windows.Forms.ColumnHeader();
      this.protocolColumn = new System.Windows.Forms.ColumnHeader();
      this.packetTreeView = new System.Windows.Forms.TreeView();
      this.packetRichTextBox = new System.Windows.Forms.RichTextBox();
      this.SuspendLayout();
      // 
      // ipTextBox
      // 
      this.ipTextBox.Location = new System.Drawing.Point(85, 34);
      this.ipTextBox.Name = "ipTextBox";
      this.ipTextBox.Size = new System.Drawing.Size(100, 20);
      this.ipTextBox.TabIndex = 0;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(59, 37);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(20, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "IP:";
      // 
      // startButton
      // 
      this.startButton.Location = new System.Drawing.Point(125, 84);
      this.startButton.Name = "startButton";
      this.startButton.Size = new System.Drawing.Size(75, 23);
      this.startButton.TabIndex = 2;
      this.startButton.Text = "Start";
      this.startButton.UseVisualStyleBackColor = true;
      this.startButton.Click += new System.EventHandler(this.startButton_Click);
      // 
      // stopButton
      // 
      this.stopButton.Location = new System.Drawing.Point(280, 84);
      this.stopButton.Name = "stopButton";
      this.stopButton.Size = new System.Drawing.Size(75, 23);
      this.stopButton.TabIndex = 3;
      this.stopButton.Text = "Stop";
      this.stopButton.UseVisualStyleBackColor = true;
      this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
      // 
      // packetsListView
      // 
      this.packetsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.numberColumn,
            this.srcIPColumn,
            this.srcPorCcolumn,
            this.destIPColumn,
            this.destPortColumn,
            this.protocolColumn});
      this.packetsListView.FullRowSelect = true;
      this.packetsListView.GridLines = true;
      this.packetsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.packetsListView.HideSelection = false;
      this.packetsListView.Location = new System.Drawing.Point(27, 137);
      this.packetsListView.MultiSelect = false;
      this.packetsListView.Name = "packetsListView";
      this.packetsListView.Size = new System.Drawing.Size(748, 178);
      this.packetsListView.TabIndex = 4;
      this.packetsListView.UseCompatibleStateImageBehavior = false;
      this.packetsListView.View = System.Windows.Forms.View.Details;
      this.packetsListView.SelectedIndexChanged += new System.EventHandler(this.packetsListView_SelectedIndexChanged);
      // 
      // numberColumn
      // 
      this.numberColumn.Text = "№";
      this.numberColumn.Width = 30;
      // 
      // srcIPColumn
      // 
      this.srcIPColumn.Text = "Source IP";
      this.srcIPColumn.Width = 118;
      // 
      // srcPorCcolumn
      // 
      this.srcPorCcolumn.Text = "Source Port";
      this.srcPorCcolumn.Width = 152;
      // 
      // destIPColumn
      // 
      this.destIPColumn.Text = "Destination IP";
      this.destIPColumn.Width = 122;
      // 
      // destPortColumn
      // 
      this.destPortColumn.Text = "Destination Port";
      this.destPortColumn.Width = 123;
      // 
      // protocolColumn
      // 
      this.protocolColumn.Text = "Protocol";
      this.protocolColumn.Width = 132;
      // 
      // packetTreeView
      // 
      this.packetTreeView.Location = new System.Drawing.Point(27, 335);
      this.packetTreeView.Name = "packetTreeView";
      this.packetTreeView.Size = new System.Drawing.Size(748, 140);
      this.packetTreeView.TabIndex = 5;
      // 
      // packetRichTextBox
      // 
      this.packetRichTextBox.Location = new System.Drawing.Point(27, 493);
      this.packetRichTextBox.Name = "packetRichTextBox";
      this.packetRichTextBox.ReadOnly = true;
      this.packetRichTextBox.Size = new System.Drawing.Size(748, 96);
      this.packetRichTextBox.TabIndex = 6;
      this.packetRichTextBox.Text = "";
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(807, 611);
      this.Controls.Add(this.packetRichTextBox);
      this.Controls.Add(this.packetTreeView);
      this.Controls.Add(this.packetsListView);
      this.Controls.Add(this.stopButton);
      this.Controls.Add(this.startButton);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.ipTextBox);
      this.Name = "Form1";
      this.Text = "PigSniffer";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox ipTextBox;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button startButton;
    private System.Windows.Forms.Button stopButton;
    private System.Windows.Forms.ListView packetsListView;
    private System.Windows.Forms.ColumnHeader numberColumn;
    private System.Windows.Forms.ColumnHeader srcIPColumn;
    private System.Windows.Forms.ColumnHeader srcPorCcolumn;
    private System.Windows.Forms.ColumnHeader destIPColumn;
    private System.Windows.Forms.ColumnHeader destPortColumn;
    private System.Windows.Forms.ColumnHeader protocolColumn;
    private System.Windows.Forms.TreeView packetTreeView;
    private System.Windows.Forms.RichTextBox packetRichTextBox;
  }
}

