namespace Watchman_Client
{
    partial class MainForm
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
            this.connectButton = new System.Windows.Forms.Button();
            this.dataTextBox = new System.Windows.Forms.TextBox();
            this.sendCommandButton = new System.Windows.Forms.Button();
            this.commandComboBox = new System.Windows.Forms.ComboBox();
            this.dataLabel = new System.Windows.Forms.Label();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ipAddressTextBox = new System.Windows.Forms.TextBox();
            this.portTextBox = new System.Windows.Forms.TextBox();
            this.screenCapturePictureBox = new System.Windows.Forms.PictureBox();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.screenCapturePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(197, 65);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 23);
            this.connectButton.TabIndex = 0;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataTextBox
            // 
            this.dataTextBox.Location = new System.Drawing.Point(13, 154);
            this.dataTextBox.Multiline = true;
            this.dataTextBox.Name = "dataTextBox";
            this.dataTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.dataTextBox.Size = new System.Drawing.Size(259, 216);
            this.dataTextBox.TabIndex = 1;
            // 
            // sendCommandButton
            // 
            this.sendCommandButton.Location = new System.Drawing.Point(15, 103);
            this.sendCommandButton.Name = "sendCommandButton";
            this.sendCommandButton.Size = new System.Drawing.Size(99, 23);
            this.sendCommandButton.TabIndex = 2;
            this.sendCommandButton.Text = "Send Command";
            this.sendCommandButton.UseVisualStyleBackColor = true;
            this.sendCommandButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // commandComboBox
            // 
            this.commandComboBox.FormattingEnabled = true;
            this.commandComboBox.Items.AddRange(new object[] {
            "Screen Capture"});
            this.commandComboBox.Location = new System.Drawing.Point(119, 103);
            this.commandComboBox.Name = "commandComboBox";
            this.commandComboBox.Size = new System.Drawing.Size(153, 21);
            this.commandComboBox.TabIndex = 3;
            // 
            // dataLabel
            // 
            this.dataLabel.AutoSize = true;
            this.dataLabel.Location = new System.Drawing.Point(12, 138);
            this.dataLabel.Name = "dataLabel";
            this.dataLabel.Size = new System.Drawing.Size(30, 13);
            this.dataLabel.TabIndex = 4;
            this.dataLabel.Text = "Data";
            // 
            // mainPanel
            // 
            this.mainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainPanel.AutoScroll = true;
            this.mainPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mainPanel.Controls.Add(this.screenCapturePictureBox);
            this.mainPanel.Location = new System.Drawing.Point(278, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(448, 377);
            this.mainPanel.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "IP Address:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Port:";
            // 
            // ipAddressTextBox
            // 
            this.ipAddressTextBox.Location = new System.Drawing.Point(119, 11);
            this.ipAddressTextBox.Name = "ipAddressTextBox";
            this.ipAddressTextBox.Size = new System.Drawing.Size(153, 20);
            this.ipAddressTextBox.TabIndex = 10;
            this.ipAddressTextBox.Text = "192.168.1.23";
            // 
            // portTextBox
            // 
            this.portTextBox.Location = new System.Drawing.Point(119, 39);
            this.portTextBox.Name = "portTextBox";
            this.portTextBox.Size = new System.Drawing.Size(153, 20);
            this.portTextBox.TabIndex = 11;
            this.portTextBox.Text = "9000";
            // 
            // screenCapturePictureBox
            // 
            this.screenCapturePictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.screenCapturePictureBox.Location = new System.Drawing.Point(0, 0);
            this.screenCapturePictureBox.Name = "screenCapturePictureBox";
            this.screenCapturePictureBox.Size = new System.Drawing.Size(446, 375);
            this.screenCapturePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.screenCapturePictureBox.TabIndex = 6;
            this.screenCapturePictureBox.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(738, 377);
            this.Controls.Add(this.portTextBox);
            this.Controls.Add(this.ipAddressTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.dataLabel);
            this.Controls.Add(this.commandComboBox);
            this.Controls.Add(this.sendCommandButton);
            this.Controls.Add(this.dataTextBox);
            this.Controls.Add(this.connectButton);
            this.Name = "MainForm";
            this.Text = "Watchman - Desktop Monitor Client";
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.screenCapturePictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.TextBox dataTextBox;
        private System.Windows.Forms.Button sendCommandButton;
        private System.Windows.Forms.ComboBox commandComboBox;
        private System.Windows.Forms.Label dataLabel;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ipAddressTextBox;
        private System.Windows.Forms.TextBox portTextBox;
        private System.Windows.Forms.PictureBox screenCapturePictureBox;
    }
}

