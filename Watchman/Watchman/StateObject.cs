using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace Watchman
{
    public class StateObject
    {
        #region Constants

        public const int BufferSize = 1024; // Size of the receiver buffer

        #endregion

        #region Fields
        public readonly byte[] Buffer = new byte[BufferSize];               // Receiver buffer
        public readonly StringBuilder StringBuilder = new StringBuilder();  // Received data string.
        public byte[] SendBuffer = new byte[0];                             // Sending Buffer
        public Socket WorkSocket = null;                                    // Socket Client
        #endregion
    }
}
