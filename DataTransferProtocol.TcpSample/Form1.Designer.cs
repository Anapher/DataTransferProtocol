namespace DataTransferProtocol.TcpSample
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.portNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.ipComboBox = new System.Windows.Forms.ComboBox();
            this.stopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.clientPortNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.clientIpComboBox = new System.Windows.Forms.ComboBox();
            this.disconnectButton = new System.Windows.Forms.Button();
            this.connectButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.clientCommandsPanel = new System.Windows.Forms.Panel();
            this.showMessageBoxTextBox = new System.Windows.Forms.TextBox();
            this.showMessageBoxButton = new System.Windows.Forms.Button();
            this.getServerInformationButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.portNumericUpDown)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clientPortNumericUpDown)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.clientCommandsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.portNumericUpDown);
            this.groupBox1.Controls.Add(this.ipComboBox);
            this.groupBox1.Controls.Add(this.stopButton);
            this.groupBox1.Controls.Add(this.startButton);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(350, 277);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server";
            // 
            // portNumericUpDown
            // 
            this.portNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.portNumericUpDown.Location = new System.Drawing.Point(248, 29);
            this.portNumericUpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.portNumericUpDown.Name = "portNumericUpDown";
            this.portNumericUpDown.Size = new System.Drawing.Size(96, 20);
            this.portNumericUpDown.TabIndex = 3;
            this.portNumericUpDown.Value = new decimal(new int[] {
            10799,
            0,
            0,
            0});
            // 
            // ipComboBox
            // 
            this.ipComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ipComboBox.FormattingEnabled = true;
            this.ipComboBox.Location = new System.Drawing.Point(9, 28);
            this.ipComboBox.Name = "ipComboBox";
            this.ipComboBox.Size = new System.Drawing.Size(233, 21);
            this.ipComboBox.TabIndex = 2;
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(90, 55);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(9, 55);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.clientCommandsPanel);
            this.groupBox2.Controls.Add(this.clientPortNumericUpDown);
            this.groupBox2.Controls.Add(this.clientIpComboBox);
            this.groupBox2.Controls.Add(this.disconnectButton);
            this.groupBox2.Controls.Add(this.connectButton);
            this.groupBox2.Location = new System.Drawing.Point(359, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(351, 277);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Client";
            // 
            // clientPortNumericUpDown
            // 
            this.clientPortNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.clientPortNumericUpDown.Location = new System.Drawing.Point(245, 29);
            this.clientPortNumericUpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.clientPortNumericUpDown.Name = "clientPortNumericUpDown";
            this.clientPortNumericUpDown.Size = new System.Drawing.Size(96, 20);
            this.clientPortNumericUpDown.TabIndex = 7;
            this.clientPortNumericUpDown.Value = new decimal(new int[] {
            10799,
            0,
            0,
            0});
            // 
            // clientIpComboBox
            // 
            this.clientIpComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clientIpComboBox.FormattingEnabled = true;
            this.clientIpComboBox.Location = new System.Drawing.Point(6, 28);
            this.clientIpComboBox.Name = "clientIpComboBox";
            this.clientIpComboBox.Size = new System.Drawing.Size(233, 21);
            this.clientIpComboBox.TabIndex = 6;
            // 
            // disconnectButton
            // 
            this.disconnectButton.Enabled = false;
            this.disconnectButton.Location = new System.Drawing.Point(125, 55);
            this.disconnectButton.Name = "disconnectButton";
            this.disconnectButton.Size = new System.Drawing.Size(114, 23);
            this.disconnectButton.TabIndex = 5;
            this.disconnectButton.Text = "Disconnect";
            this.disconnectButton.UseVisualStyleBackColor = true;
            this.disconnectButton.Click += new System.EventHandler(this.disconnectButton_Click);
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(6, 55);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(114, 23);
            this.connectButton.TabIndex = 4;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(713, 283);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // clientCommandsPanel
            // 
            this.clientCommandsPanel.Controls.Add(this.getServerInformationButton);
            this.clientCommandsPanel.Controls.Add(this.showMessageBoxButton);
            this.clientCommandsPanel.Controls.Add(this.showMessageBoxTextBox);
            this.clientCommandsPanel.Enabled = false;
            this.clientCommandsPanel.Location = new System.Drawing.Point(6, 127);
            this.clientCommandsPanel.Name = "clientCommandsPanel";
            this.clientCommandsPanel.Size = new System.Drawing.Size(336, 141);
            this.clientCommandsPanel.TabIndex = 8;
            // 
            // showMessageBoxTextBox
            // 
            this.showMessageBoxTextBox.Location = new System.Drawing.Point(3, 3);
            this.showMessageBoxTextBox.Name = "showMessageBoxTextBox";
            this.showMessageBoxTextBox.Size = new System.Drawing.Size(330, 20);
            this.showMessageBoxTextBox.TabIndex = 0;
            // 
            // showMessageBoxButton
            // 
            this.showMessageBoxButton.Location = new System.Drawing.Point(3, 29);
            this.showMessageBoxButton.Name = "showMessageBoxButton";
            this.showMessageBoxButton.Size = new System.Drawing.Size(111, 23);
            this.showMessageBoxButton.TabIndex = 1;
            this.showMessageBoxButton.Text = "Show MessageBox";
            this.showMessageBoxButton.UseVisualStyleBackColor = true;
            this.showMessageBoxButton.Click += new System.EventHandler(this.showMessageBoxButton_Click);
            // 
            // getServerInformationButton
            // 
            this.getServerInformationButton.Location = new System.Drawing.Point(3, 72);
            this.getServerInformationButton.Name = "getServerInformationButton";
            this.getServerInformationButton.Size = new System.Drawing.Size(165, 23);
            this.getServerInformationButton.TabIndex = 2;
            this.getServerInformationButton.Text = "Get Server Information";
            this.getServerInformationButton.UseVisualStyleBackColor = true;
            this.getServerInformationButton.Click += new System.EventHandler(this.getServerInformationButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 283);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Data Transfer Protocol Test";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.portNumericUpDown)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.clientPortNumericUpDown)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.clientCommandsPanel.ResumeLayout(false);
            this.clientCommandsPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.NumericUpDown portNumericUpDown;
        private System.Windows.Forms.ComboBox ipComboBox;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.NumericUpDown clientPortNumericUpDown;
        private System.Windows.Forms.ComboBox clientIpComboBox;
        private System.Windows.Forms.Button disconnectButton;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Panel clientCommandsPanel;
        private System.Windows.Forms.Button showMessageBoxButton;
        private System.Windows.Forms.TextBox showMessageBoxTextBox;
        private System.Windows.Forms.Button getServerInformationButton;
    }
}

