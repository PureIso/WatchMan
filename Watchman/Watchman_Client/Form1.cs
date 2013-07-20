﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Watchman_Client
{
    public partial class Form1 : Form
    {
        private string command = null;
        private int maxDataBuffer = int.MaxValue ;
        private Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        
        public Form1()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConnectionLoop();
        }

        private void SendLoop()
        {
            try
            {
                byte[] buffer = Encoding.ASCII.GetBytes(command);
                _clientSocket.Send(buffer);


                byte[] receiveBuffer = new byte[500000];
                
                //Size is first received
                int length = _clientSocket.Receive(receiveBuffer);
                byte[] data = new byte[length];
                Array.Copy(receiveBuffer, data, data.Length);

                if (command == "Screen Capture")
                {
                    TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
                    //parameter not valid
                    using (Image image = (Bitmap)tc.ConvertFrom(data))
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
                else
                {
                    string text = Encoding.ASCII.GetString(data);
                    dataTextBox.Text = text;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ConnectionLoop()
        {
            while(!_clientSocket.Connected)
            {
                try
                {
                    _clientSocket.Connect(IPAddress.Loopback, 9000);
                }
                catch (SocketException)
                {
                }
            }
            connectButton.Text = "Connected";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            command = commandComboBox.GetItemText(commandComboBox.SelectedItem);
            SendLoop();
        }
    }
}