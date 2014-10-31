using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;

namespace Watchman_Client
{
    public partial class MainForm : Form
    {
        private string _command;
        private readonly Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConnectionLoop();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(x =>
            {
                //_command = commandComboBox.GetItemText(commandComboBox.SelectedItem);
                SendLoop();
            });
        }

        static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        #region Functions
        private void SendLoop()
        {
            try
            {
                TcpClient client = new TcpClient();
                client.Connect(_serverEndPoint);
                Send(client, "<ScreenCapture><EOF>");

                Thread.Sleep(1000);
                NetworkStream netStream = client.GetStream();
                netStream.ReadTimeout = 5000;
                byte[] myReadBuffer = new byte[1024];
                byte[] newBytes = new byte[0];

                string a;
                string b;
                string aa;
                string ab;
                string aaa;
                string aab;
                string aaaa;
                string aaab;
                try
                {
                    

                    do
                    {
                        if (!netStream.CanRead) break;
                        myReadBuffer = new byte[1024];
                        int read = netStream.Read(myReadBuffer, 0, myReadBuffer.Length);
                        if (read == 0) break;
                        newBytes = CombineBytes(newBytes, myReadBuffer, read);
                        Thread.Sleep(300);
                    } while (netStream.DataAvailable);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("---Image Receive exception---");
                    Console.WriteLine(ex.ToString());
                }

                byte[] toConvert = new byte[newBytes.Length - 4];
                Array.Copy(newBytes, 4, toConvert, 0, toConvert.Length);

                string base64 = GetString(toConvert);
                byte[] data = Convert.FromBase64String(base64);
              
                 a = data[0].ToString();
                 b = data[1].ToString();
                 aa = data[2].ToString();
                 ab = data[3].ToString();
                 aaa = data[4].ToString();
                 aab = data[6].ToString();
                 aaaa = data[6].ToString();
                 aaab = data[7].ToString();
                Console.WriteLine("B HEAD: {0}", (a + "-" + b + "-" + aa + "-" + ab + "-" + aaa + "-" + aab + "-" + aaaa + "-" + aaab));
                a = data[data.Length - 1].ToString();
                b = data[data.Length - 2].ToString();
                aa = data[data.Length - 3].ToString();
                ab = data[data.Length - 4].ToString();
                aaa = data[data.Length - 5].ToString();
                aab = data[data.Length - 6].ToString();
                aaaa = data[data.Length - 7].ToString();
                aaab = data[data.Length - 8].ToString();
                Console.WriteLine("B TAIL: {0}", (a + "-" + b + "-" + aa + "-" + ab + "-" + aaa + "-" + aab + "-" + aaaa + "-" + aaab));

                MemoryStream memoryStream = new MemoryStream(data);
                memoryStream.Seek(0, SeekOrigin.Begin);
                Bitmap img = new Bitmap(memoryStream);
          
                if (screenCapturePictureBox.Image != null)screenCapturePictureBox.Image.Dispose();
                screenCapturePictureBox.Image = img;
                screenCapturePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                screenCapturePictureBox.Image = null;
            }
        }

        private byte[] Send(TcpClient socket, string message)
        {
            byte[] buffer = new ASCIIEncoding().GetBytes(message);
            ManualResetEvent resetEvent = new ManualResetEvent(false);
            ThreadPool.QueueUserWorkItem(x =>
            {
                try
                {
                    if (socket == null) return;
                    if (!socket.Connected) return;
                    NetworkStream netStream = socket.GetStream();
                    netStream.WriteTimeout = 5000;
                    netStream.Write(buffer, 0, buffer.Length);
                    netStream.Flush();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("--- Message Send ---");
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    resetEvent.Set();
                }
            });
            resetEvent.WaitOne();
            return buffer;
        }

        private List<string> Receive(TcpClient socket)
        {
            List<string> returnBuffer = new List<string>();
            ManualResetEvent resetEvent = new ManualResetEvent(false);
            ThreadPool.QueueUserWorkItem(x =>
            {
                try
                {
                    if (socket == null) return;
                    if (!socket.Connected) return;
                    NetworkStream netStream = socket.GetStream();
                    netStream.ReadTimeout = 5000;
                    StringBuilder fullStringBuilder = new StringBuilder();
                    try
                    {
                        byte[] myReadBuffer = new byte[1024];
                        if (!netStream.CanRead) return;
                        do
                        {
                            byte[] newBytes = new byte[0];
                            if (!netStream.CanRead) break;
                            int read = netStream.Read(myReadBuffer, 0, myReadBuffer.Length);
                            newBytes = CombineBytes(newBytes, myReadBuffer, read);
                            string stringData = Encoding.ASCII.GetString(newBytes);
                            fullStringBuilder.Append(stringData);
                            Thread.Sleep(350);
                        } while (netStream.DataAvailable);
                        //============================================================================
                        string[] responseStringSplit = fullStringBuilder.ToString()
                                .Split(new[] {"#" }, StringSplitOptions.RemoveEmptyEntries);
                        returnBuffer = new List<string>();
                        returnBuffer.AddRange(responseStringSplit);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("--- Message Receive ---");
                        Console.WriteLine(ex.ToString());
                    }
                    finally
                    {
                        resetEvent.Set();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("--- Master Message Receive ---");
                    Console.WriteLine(ex.ToString());
                }
            });
            resetEvent.WaitOne();
            return returnBuffer;
        }
        private static byte[] CombineBytes(byte[] first, byte[] second, int length)
        {
            try
            {
                int array1OriginalLength = first.Length;
                Array.Resize(ref first, array1OriginalLength + length);
                Array.Copy(second, 0, first, array1OriginalLength, length);
                return first;
            }
            catch (Exception ex)
            {
                Console.WriteLine("--- Combine Bytes ---");
                Console.WriteLine(ex.ToString());
                return first;
            }
        }

        private IPEndPoint _serverEndPoint;
        private void ConnectionLoop()
        {
            _serverEndPoint = new IPEndPoint(
                IPAddress.Parse(ipAddressTextBox.Text), int.Parse(portTextBox.Text));
        }
        #endregion

    }
}
