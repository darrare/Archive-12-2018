using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Timers;

namespace Swagger
{
    public partial class Form1 : Form
    {
        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr point);

        [DllImport("User32.dll")]
        static extern int ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        #region constants

        const int KEY_DOWN_EVENT = 0x0001; //Key down flag
        const int KEY_UP_EVENT = 0x0002; //Key up flag

        #endregion

        #region fields

        int currentHeldKey;
        Process wow;

        #endregion

        public Form1()
        {
            InitializeComponent();
            Utilities.form = this;

            System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();
            myTimer.Interval = 500; //.5 seconds
            myTimer.Tick += new System.EventHandler(Updater);
            myTimer.Start();
        }

        private void Updater(object sender, EventArgs e)
        {
            if (Utilities.startTime != null)
            {
                CurrentStatus.Text = "Time: " + ((TimeSpan)(DateTime.Now - Utilities.startTime)).Minutes + ":" + ((TimeSpan)(DateTime.Now - Utilities.startTime)).Seconds + " / " + Utilities.gameTime / 1000;
            }
        }

        private void StartAntiAfk_Click(object sender, EventArgs e)
        {
            //HoldKey(W_KEY, 3000);

            //Utilities.Start();
            //CurrentStatus.Text = "Anti-Afk active";

            wow = Process.GetProcessesByName("Notepad").FirstOrDefault(); //Wow-64
            if (wow != null)
            {
                IntPtr h = wow.MainWindowHandle;
                SetForegroundWindow(h);
                Utilities.Start();
                CurrentStatus.Text = "Anti-Afk active";
            }
            else
            {
                CurrentStatus.Text = "Brawlhalla not found.";
            }
        }

        private void StopAntiAfk_Click(object sender, EventArgs e)
        {
            //ReleaseKey();
            Utilities.Stop();
            CurrentStatus.Text = "Stopped Operation.";
        }

        public void HoldKey(int key)
        {
            if (GetForegroundWindow() == wow.MainWindowHandle)
            {
                keybd_event((byte)key, 0, KEY_DOWN_EVENT, 0);
                currentHeldKey = key;
            }
            else
            {
                Utilities.Stop();
                keybd_event((byte)currentHeldKey, 0, KEY_UP_EVENT, 0);
                currentHeldKey = 0;
                //CurrentStatus.Text = "Stopped Operation.";
            }

            //if (Utilities.startTime != null)
            //{
            //    CurrentStatus.Text = "Time: " + ((TimeSpan)(DateTime.Now - Utilities.startTime)).Minutes + ":" + ((TimeSpan)(DateTime.Now - Utilities.startTime)).Seconds + " / " + Utilities.gameTime / 1000;
            //}


        }

        public void ReleaseKey()
        {
            if (currentHeldKey != 0 && GetForegroundWindow() == wow.MainWindowHandle)
            {
                keybd_event((byte)currentHeldKey, 0, KEY_UP_EVENT, 0);
            }
            //if (Utilities.startTime != null)
            //{
            //    CurrentStatus.Text = "Time: " + ((TimeSpan)(DateTime.Now - Utilities.startTime)).Minutes + ":" + ((TimeSpan)(DateTime.Now - Utilities.startTime)).Seconds + " / " + Utilities.gameTime / 1000;
            //}
        }

        public void PressKey(int key)
        {
            if (GetForegroundWindow() == wow.MainWindowHandle)
            {
                keybd_event((byte)key, 0, KEY_DOWN_EVENT, 0);
                currentHeldKey = key;
                keybd_event((byte)currentHeldKey, 0, KEY_UP_EVENT, 0);
                currentHeldKey = 0;
            }
            //if (Utilities.startTime != null)
            //{
            //    CurrentStatus.Text = "Time: " + ((TimeSpan)(DateTime.Now - Utilities.startTime)).Minutes + ":" + ((TimeSpan)(DateTime.Now - Utilities.startTime)).Seconds + " / " + Utilities.gameTime / 1000;
            //}
        }
    }
}
