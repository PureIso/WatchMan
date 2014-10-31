namespace Watchman
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
            this.timer1 = new System.Windows.Forms.Timer();
            this.timer2 = new System.Windows.Forms.Timer();
            this.timer3 = new System.Windows.Forms.Timer();
            this.timer4 = new System.Windows.Forms.Timer();
            this.mainPictureBox = new System.Windows.Forms.PictureBox();
            this.startRecordButton = new System.Windows.Forms.Button();
            this.stopRecordButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 60000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // timer3
            // 
            this.timer3.Interval = 300000;
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // mainPictureBox
            // 
            this.mainPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mainPictureBox.Location = new System.Drawing.Point(13, 12);
            this.mainPictureBox.Name = "mainPictureBox";
            this.mainPictureBox.Size = new System.Drawing.Size(320, 221);
            this.mainPictureBox.TabIndex = 0;
            this.mainPictureBox.TabStop = false;
            // 
            // startRecordButton
            // 
            this.startRecordButton.Location = new System.Drawing.Point(6, 19);
            this.startRecordButton.Name = "startRecordButton";
            this.startRecordButton.Size = new System.Drawing.Size(75, 23);
            this.startRecordButton.TabIndex = 1;
            this.startRecordButton.Text = "Start";
            this.startRecordButton.UseVisualStyleBackColor = true;
            this.startRecordButton.Click += new System.EventHandler(this.startRecordButton_Click);
            // 
            // stopRecordButton
            // 
            this.stopRecordButton.Location = new System.Drawing.Point(6, 48);
            this.stopRecordButton.Name = "stopRecordButton";
            this.stopRecordButton.Size = new System.Drawing.Size(75, 23);
            this.stopRecordButton.TabIndex = 2;
            this.stopRecordButton.Text = "Stop";
            this.stopRecordButton.UseVisualStyleBackColor = true;
            this.stopRecordButton.Click += new System.EventHandler(this.stopRecordButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.startRecordButton);
            this.groupBox1.Controls.Add(this.stopRecordButton);
            this.groupBox1.Location = new System.Drawing.Point(339, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(88, 80);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Recording";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 244);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.mainPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Watchman - Desktop Monitor Server";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Timer timer3;
        private System.Windows.Forms.Timer timer4;
        private System.Windows.Forms.Button startRecordButton;
        private System.Windows.Forms.Button stopRecordButton;
        public System.Windows.Forms.PictureBox mainPictureBox;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

