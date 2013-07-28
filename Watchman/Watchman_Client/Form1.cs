using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Watchman_Client
{
    public partial class Form1 : Form
    {
        private string _command;
        private readonly Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConnectionLoop();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            _command = commandComboBox.GetItemText(commandComboBox.SelectedItem);
            SendLoop();
        }

        #region Functions
        private void SendLoop()
        {
            try
            {
                //Covert string command to bytes
                byte[] buffer = Encoding.ASCII.GetBytes(_command);
                _clientSocket.Send(buffer);

                //Wait 1 sec after sending command for proper reply
                Thread.Sleep(1000);
                //Peek into the socket for the data size
                byte[] size = new byte[4];
                _clientSocket.Receive(size, 4, SocketFlags.Peek);
                //Covert the size to integer
                int intSize = BitConverter.ToInt32(size, 0);

                //Initialize a buffer for incoming data
                byte[] receiveBuffer = new byte[intSize+size.Length];
                _clientSocket.Receive(receiveBuffer);
                byte[] data = new byte[intSize];
                Array.Copy(receiveBuffer, 4, data, 0, intSize);

                if (_command == "Screen Capture")
                {
                    TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
                    //parameter not valid
                    using (Image image = (Bitmap)tc.ConvertFrom(data))
                    {
                        if (image != null)
                        {
                            Image clonedImg = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);
                            using (var copy = Graphics.FromImage(clonedImg))
                            {
                                copy.DrawImage(image, 0, 0);
                            }
                            screenCapturePictureBox.Width = image.Width;
                            screenCapturePictureBox.Height = image.Height;
                            screenCapturePictureBox.InitialImage = null;
                            screenCapturePictureBox.Image = clonedImg;
                        }
                    }

                }
                else
                {
                    string text = Encoding.ASCII.GetString(data);
                    dataTextBox.Text = text;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                screenCapturePictureBox.Image = null;
            }
        }

        private void ConnectionLoop()
        {
            while(!_clientSocket.Connected)
            {
                try
                {
                    _clientSocket.Connect(IPAddress.Parse(ipAddressTextBox.Text), 9000);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connectButton.Text = "Connected";
        }
        #endregion 

    }
}
