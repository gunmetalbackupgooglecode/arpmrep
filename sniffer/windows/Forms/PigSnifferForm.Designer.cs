namespace PigSniffer.Forms
{
  partial class PigSnifferForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components;

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
      System.Windows.Forms.Label nicsLabel;
      System.Windows.Forms.SplitContainer bottomSplitContainer;
      System.Windows.Forms.SplitContainer topSplitContainer;
      this.nicsComboBox = new System.Windows.Forms.ComboBox();
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
      this.pigSnifferMenuStrip = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.filtersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      nicsLabel = new System.Windows.Forms.Label();
      bottomSplitContainer = new System.Windows.Forms.SplitContainer();
      topSplitContainer = new System.Windows.Forms.SplitContainer();
      bottomSplitContainer.Panel1.SuspendLayout();
      bottomSplitContainer.Panel2.SuspendLayout();
      bottomSplitContainer.SuspendLayout();
      topSplitContainer.Panel1.SuspendLayout();
      topSplitContainer.Panel2.SuspendLayout();
      topSplitContainer.SuspendLayout();
      this.pigSnifferMenuStrip.SuspendLayout();
      this.SuspendLayout();
      // 
      // nicsLabel
      // 
      nicsLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
      nicsLabel.AutoSize = true;
      nicsLabel.Location = new System.Drawing.Point(76, 34);
      nicsLabel.Name = "nicsLabel";
      nicsLabel.Size = new System.Drawing.Size(100, 13);
      nicsLabel.TabIndex = 7;
      nicsLabel.Text = "Network Interfaces:";
      // 
      // nicsComboBox
      // 
      this.nicsComboBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.nicsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.nicsComboBox.FormattingEnabled = true;
      this.nicsComboBox.Location = new System.Drawing.Point(178, 31);
      this.nicsComboBox.Name = "nicsComboBox";
      this.nicsComboBox.Size = new System.Drawing.Size(341, 21);
      this.nicsComboBox.TabIndex = 9;
      // 
      // startButton
      // 
      this.startButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.startButton.Location = new System.Drawing.Point(559, 31);
      this.startButton.Name = "startButton";
      this.startButton.Size = new System.Drawing.Size(75, 23);
      this.startButton.TabIndex = 2;
      this.startButton.Text = "Start";
      this.startButton.UseVisualStyleBackColor = true;
      this.startButton.Click += new System.EventHandler(this.startButton_Click);
      // 
      // stopButton
      // 
      this.stopButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.stopButton.Location = new System.Drawing.Point(655, 31);
      this.stopButton.Name = "stopButton";
      this.stopButton.Size = new System.Drawing.Size(75, 23);
      this.stopButton.TabIndex = 3;
      this.stopButton.Text = "Stop";
      this.stopButton.UseVisualStyleBackColor = true;
      this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
      // 
      // bottomSplitContainer
      // 
      bottomSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      bottomSplitContainer.Location = new System.Drawing.Point(12, 61);
      bottomSplitContainer.Name = "bottomSplitContainer";
      bottomSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // bottomSplitContainer.Panel1
      // 
      bottomSplitContainer.Panel1.Controls.Add(topSplitContainer);
      bottomSplitContainer.Panel1MinSize = 110;
      // 
      // bottomSplitContainer.Panel2
      // 
      bottomSplitContainer.Panel2.Controls.Add(this.packetRichTextBox);
      bottomSplitContainer.Panel2MinSize = 50;
      bottomSplitContainer.Size = new System.Drawing.Size(783, 541);
      bottomSplitContainer.SplitterDistance = 407;
      bottomSplitContainer.TabIndex = 11;
      // 
      // topSplitContainer
      // 
      topSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
      topSplitContainer.Location = new System.Drawing.Point(0, 0);
      topSplitContainer.Name = "topSplitContainer";
      topSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // topSplitContainer.Panel1
      // 
      topSplitContainer.Panel1.Controls.Add(this.packetsListView);
      topSplitContainer.Panel1MinSize = 50;
      // 
      // topSplitContainer.Panel2
      // 
      topSplitContainer.Panel2.Controls.Add(this.packetTreeView);
      topSplitContainer.Panel2MinSize = 50;
      topSplitContainer.Size = new System.Drawing.Size(783, 407);
      topSplitContainer.SplitterDistance = 189;
      topSplitContainer.TabIndex = 12;
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
      this.packetsListView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.packetsListView.FullRowSelect = true;
      this.packetsListView.GridLines = true;
      this.packetsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.packetsListView.HideSelection = false;
      this.packetsListView.Location = new System.Drawing.Point(0, 0);
      this.packetsListView.MultiSelect = false;
      this.packetsListView.Name = "packetsListView";
      this.packetsListView.Size = new System.Drawing.Size(783, 189);
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
      this.packetTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.packetTreeView.Location = new System.Drawing.Point(0, 0);
      this.packetTreeView.Name = "packetTreeView";
      this.packetTreeView.Size = new System.Drawing.Size(783, 214);
      this.packetTreeView.TabIndex = 5;
      // 
      // packetRichTextBox
      // 
      this.packetRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.packetRichTextBox.Location = new System.Drawing.Point(0, 0);
      this.packetRichTextBox.Name = "packetRichTextBox";
      this.packetRichTextBox.ReadOnly = true;
      this.packetRichTextBox.Size = new System.Drawing.Size(783, 130);
      this.packetRichTextBox.TabIndex = 6;
      this.packetRichTextBox.Text = "";
      // 
      // pigSnifferMenuStrip
      // 
      this.pigSnifferMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.filtersToolStripMenuItem,
            this.helpToolStripMenuItem});
      this.pigSnifferMenuStrip.Location = new System.Drawing.Point(0, 0);
      this.pigSnifferMenuStrip.Name = "pigSnifferMenuStrip";
      this.pigSnifferMenuStrip.Size = new System.Drawing.Size(807, 24);
      this.pigSnifferMenuStrip.TabIndex = 12;
      this.pigSnifferMenuStrip.Text = "pigSnifferMenuStrip";
      // 
      // fileToolStripMenuItem
      // 
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
      this.fileToolStripMenuItem.Text = "&File";
      // 
      // editToolStripMenuItem
      // 
      this.editToolStripMenuItem.Name = "editToolStripMenuItem";
      this.editToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
      this.editToolStripMenuItem.Text = "&Edit";
      // 
      // filtersToolStripMenuItem
      // 
      this.filtersToolStripMenuItem.Name = "filtersToolStripMenuItem";
      this.filtersToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
      this.filtersToolStripMenuItem.Text = "Filte&rs";
      // 
      // helpToolStripMenuItem
      // 
      this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
      this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
      this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
      this.helpToolStripMenuItem.Text = "&Help";
      // 
      // aboutToolStripMenuItem
      // 
      this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
      this.aboutToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
      this.aboutToolStripMenuItem.Text = "&About PigSniffer";
      this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
      // 
      // PigSnifferForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(807, 614);
      this.Controls.Add(this.stopButton);
      this.Controls.Add(this.startButton);
      this.Controls.Add(this.nicsComboBox);
      this.Controls.Add(nicsLabel);
      this.Controls.Add(bottomSplitContainer);
      this.Controls.Add(this.pigSnifferMenuStrip);
      this.MainMenuStrip = this.pigSnifferMenuStrip;
      this.Name = "PigSnifferForm";
      this.Text = "PigSniffer";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.pigSnifferForm_FormClosing);
      bottomSplitContainer.Panel1.ResumeLayout(false);
      bottomSplitContainer.Panel2.ResumeLayout(false);
      bottomSplitContainer.ResumeLayout(false);
      topSplitContainer.Panel1.ResumeLayout(false);
      topSplitContainer.Panel2.ResumeLayout(false);
      topSplitContainer.ResumeLayout(false);
      this.pigSnifferMenuStrip.ResumeLayout(false);
      this.pigSnifferMenuStrip.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

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
    private System.Windows.Forms.ComboBox nicsComboBox;
    private System.Windows.Forms.MenuStrip pigSnifferMenuStrip;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem filtersToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
  }
}