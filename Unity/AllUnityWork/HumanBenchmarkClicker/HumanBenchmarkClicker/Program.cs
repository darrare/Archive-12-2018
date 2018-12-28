using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Timers;

//https://www.humanbenchmark.com/tests/reactiontime/
namespace HumanBenchmarkClicker
{
    class Program
    {
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        [DllImport("user32.dll")]
        static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

        [DllImport("user32.dll")]
        internal static extern uint SendInput(uint nInputs, [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs, int cbSize);

        static Color red, green, blue, prevColor;
        static float millisecondsToWait = 80;
        static float timer = 0;

        static DateTime prevFrameTime;

        static void Main(string[] args)
        {
            red = Color.FromArgb(206, 38, 54);
            green = Color.FromArgb(75, 219, 106);
            blue = Color.FromArgb(43, 135, 209);
            prevColor = blue;
            prevFrameTime = DateTime.Now;

            //System.Timers.Timer aTimer = new System.Timers.Timer();
            //aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            //aTimer.Interval = .001;
            //aTimer.Enabled = true;
            OnTimedEvent(null, null);

            while (true) ;
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            while(true)
            {
                Point cursor = new Point();
                GetCursorPos(ref cursor);
                Color curColor = GetColorAt(cursor);

                if (curColor == green && prevColor == red)
                {
                    prevFrameTime = DateTime.Now;
                }

                if (curColor == green)
                {
                    timer += (DateTime.Now - prevFrameTime).Milliseconds;
                    prevFrameTime = DateTime.Now;
                }

                if (timer >= millisecondsToWait)
                {
                    Console.WriteLine("Clicked");
                    timer = 0;
                    ClickOnPoint(cursor);
                }
                prevColor = curColor;
            }

        }

        public static void ClickOnPoint(Point clientPoint)
        {
            var inputMouseDown = new INPUT();
            inputMouseDown.Type = 0; /// input type mouse
            inputMouseDown.Data.Mouse.Flags = 0x0002; /// left button down

            var inputMouseUp = new INPUT();
            inputMouseUp.Type = 0; /// input type mouse
            inputMouseUp.Data.Mouse.Flags = 0x0004; /// left button up

            var inputs = new INPUT[] { inputMouseDown, inputMouseUp };
            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        internal struct INPUT
        {
            public UInt32 Type;
            public MOUSEKEYBDHARDWAREINPUT Data;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct MOUSEKEYBDHARDWAREINPUT
        {
            [FieldOffset(0)]
            public MOUSEINPUT Mouse;
        }

        internal struct MOUSEINPUT
        {
            public Int32 X;
            public Int32 Y;
            public UInt32 MouseData;
            public UInt32 Flags;
            public UInt32 Time;
            public IntPtr ExtraInfo;
        }


        static Bitmap screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
        public static Color GetColorAt(Point location)
        {
            using (Graphics gdest = Graphics.FromImage(screenPixel))
            {
                using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
                {
                    IntPtr hSrcDC = gsrc.GetHdc();
                    IntPtr hDC = gdest.GetHdc();
                    int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, location.X, location.Y, (int)CopyPixelOperation.SourceCopy);
                    gdest.ReleaseHdc();
                    gsrc.ReleaseHdc();
                }
            }

            return screenPixel.GetPixel(0, 0);
        }
    }
}