namespace Watchman_Client
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
            this.connectButton = new System.Windows.Forms.Button();
            this.dataTextBox = new System.Windows.Forms.TextBox();
            this.sendCommandButton = new System.Windows.Forms.Button();
            this.commandComboBox = new System.Windows.Forms.ComboBox();
            this.dataLabel = new System.Windows.Forms.Label();
            this.screenCapturePictureBox = new System.Windows.Forms.PictureBox();
            this.screenCaptureLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.screenCapturePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(13, 4);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 23);
            this.connectButton.TabIndex = 0;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataTextBox
            // 
            this.dataTextBox.Location = new System.Drawing.Point(13, 92);
            this.dataTextBox.Multiline = true;
            this.dataTextBox.Name = "dataTextBox";
            this.dataTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.dataTextBox.Size = new System.Drawing.Size(259, 216);
            this.dataTextBox.TabIndex = 1;
            // 
            // sendCommandButton
            // 
            this.sendCommandButton.Location = new System.Drawing.Point(13, 33);
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
            this.commandComboBox.Location = new System.Drawing.Point(119, 34);
            this.commandComboBox.Name = "commandComboBox";
            this.commandComboBox.Size = new System.Drawing.Size(153, 21);
            this.commandComboBox.TabIndex = 3;
            // 
            // dataLabel
            // 
            this.dataLabel.AutoSize = true;
            this.dataLabel.Location = new System.Drawing.Point(13, 73);
            this.dataLabel.Name = "dataLabel";
            this.dataLabel.Size = new System.Drawing.Size(30, 13);
            this.dataLabel.TabIndex = 4;
            this.dataLabel.Text = "Data";
            // 
            // screenCapturePictureBox
            // 
            this.screenCapturePictureBox.Location = new System.Drawing.Point(289, 33);
            this.screenCapturePictureBox.Name = "screenCapturePictureBox";
            this.screenCapturePictureBox.Size = new System.Drawing.Size(410, 293);
            this.screenCapturePictureBox.TabIndex = 5;
            this.screenCapturePictureBox.TabStop = false;
            // 
            // screenCaptureLabel
            // 
            this.screenCaptureLabel.AutoSize = true;
            this.screenCaptureLabel.Location = new System.Drawing.Point(286, 9);
            this.screenCaptureLabel.Name = "screenCaptureLabel";
            this.screenCaptureLabel.Size = new System.Drawing.Size(81, 13);
            this.screenCaptureLabel.TabIndex = 6;
            this.screenCaptureLabel.Text = "Screen Capture";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 338);
            this.Controls.Add(this.screenCaptureLabel);
            this.Controls.Add(this.screenCapturePictureBox);
            this.Controls.Add(this.dataLabel);
            this.Controls.Add(this.commandComboBox);
            this.Controls.Add(this.sendCommandButton);
            this.Controls.Add(this.dataTextBox);
            this.Controls.Add(this.connectButton);
            this.Name = "Form1";
            this.Text = "Form1";
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
        private System.Windows.Forms.PictureBox screenCapturePictureBox;
        private System.Windows.Forms.Label screenCaptureLabel;
    }
}

