using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace BrawlhallaAfker
{
    public partial class Form1 : Form
    {
        string NAME_OF_PROCESS = "Brawlhalla";
        
        
        #region constants

        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr point);

        [DllImport("User32.dll")]
        static extern int ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]

        private static extern IntPtr GetForegroundWindow();

        const int KEY_DOWN_EVENT = 0x0001; //Key down flag
        const int KEY_UP_EVENT = 0x0002; //Key up flag

        const int W_KEY = 0x57;
        const int A_KEY = 0x41;
        const int S_KEY = 0x53;
        const int D_KEY = 0x44;
        const int J_KEY = 0x4A; //for brawlhalla
        //const int K_KEY = 0x4B; //for brawlhalla
        const int C_KEY = 0x43; //for brawlhalla

        #endregion

        #region tuning variables

        //int gameTime = 1200 * 60; //1 minutes
        int gameTime = int.MaxValue;
        int menuTime = 500 * 60; //.5 minutes
        int timeToHoldAttackInMilliseconds = 100; //.1 seconds
        int timeToHoldMovementButtonInMilliseconds = 100; //.2 second
        
        #endregion

        int currentHeldKey;
        Process curProcess;
        DateTime storedDate;

        //Timers
        Timer attackHoldTimer;
        Timer movementButtonHoldTimer;
        Timer updaterTimer;

        //State
        enum State { Ready, InGame }
        State curState = State.Ready;

        Random rand = new Random();

        public Form1()
        {
            InitializeComponent();
            updaterTimer = new Timer();
            updaterTimer.Interval = 100; //10 times a second
            updaterTimer.Tick += new EventHandler(Updater);
            updaterTimer.Start();
        }

        /// <summary>
        /// Do things here that should happen all the time no matter what.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Updater(object sender, EventArgs e)
        {
            switch (curState)
            {
                case State.Ready:
                    StateTextBox.Text = "Ready";
                    TimerTextBox.Text = "0:00 / 0:00";
                    LastPressedKeyTextBox.Text = "";
                    break;
                case State.InGame:
                    TimerTextBox.Text = (DateTime.Now - storedDate).Minutes + ":" + (DateTime.Now - storedDate).Seconds; // + " / " + TimeSpan.FromSeconds(((float)gameTime) / 1000).ToString(@"mm\:ss");

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Handles stuff whenever we need to switch state
        /// </summary>
        /// <param name="newState"></param>
        private void SwitchState(State newState)
        {
            switch (newState)
            {
                case State.Ready:
                    StateTextBox.Text = "Ready";
                    TimerTextBox.Text = "0:00 / 0:00";
                    LastPressedKeyTextBox.Text = "";

                    if (attackHoldTimer != null && movementButtonHoldTimer != null)
                    {
                        attackHoldTimer.Enabled = false;
                        attackHoldTimer.Dispose();
                        movementButtonHoldTimer.Enabled = false;
                        movementButtonHoldTimer.Dispose();
                    }



                    break;
                case State.InGame:
                    StateTextBox.Text = "In Game";
                    PickAndPressAttackKey();
                    PickAndPressRandomMovementKey();

                    storedDate = DateTime.Now;
                    break;
                default:
                    break;
            }
            curState = newState;
        }

        /// <summary>
        /// Called whenever the start button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartButton_Click(object sender, EventArgs e)
        {
            curProcess = Process.GetProcessesByName(NAME_OF_PROCESS).FirstOrDefault();
            if (curProcess != null)
            {
                IntPtr handle = curProcess.MainWindowHandle;
                SetForegroundWindow(handle);
                SwitchState(State.InGame);
            }
            else
            {
                DebugTextBox.Text = "No process";
            }
        }

        /// <summary>
        /// Called whenever the stop button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopButton_Click(object sender, EventArgs e)
        {
            SwitchState(State.Ready);
        }


        /// <summary>
        /// Tells the computer to press the key down, and start a timer to release it
        /// </summary>
        /// <param name="key"></param>
        private void HoldKey(int key)
        {
            if (GetForegroundWindow() == curProcess.MainWindowHandle)
            {
                keybd_event((byte)key, 0, KEY_DOWN_EVENT, 0);
                LastPressedKeyTextBox.Text = ((byte)key).ToString();
            }
        }

        /// <summary>
        /// Tells the computer to release a key immediately
        /// </summary>
        /// <param name="key"></param>
        private void ReleaseKey(int key)
        {
            if (GetForegroundWindow() == curProcess.MainWindowHandle)
            {
                keybd_event((byte)key, 0, KEY_UP_EVENT, 0);
            }
        }

        /// <summary>
        /// Tells the computer to press, and release a key immediately
        /// </summary>
        /// <param name="key"></param>
        public void PressKey(int key)
        {
            if (GetForegroundWindow() == curProcess.MainWindowHandle)
            {
                keybd_event((byte)key, 0, KEY_DOWN_EVENT, 0);
                currentHeldKey = key;
                keybd_event((byte)currentHeldKey, 0, KEY_UP_EVENT, 0);
                currentHeldKey = 0;
                LastPressedKeyTextBox.Text = ((byte)key).ToString();
            }
        }

        /// <summary>
        /// Selects a random key and presses it down
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void PickAndPressAttackKey()
        {
            if (attackHoldTimer != null)
                attackHoldTimer.Stop();

            if (rand.Next(0, 2) == 0)
            {
                HoldKey(J_KEY);
            }
            attackHoldTimer = new Timer();
            attackHoldTimer.Interval = timeToHoldAttackInMilliseconds;
            attackHoldTimer.Tick += new EventHandler(ReleaseCurrentHeldKeyAndSelectNew);
            attackHoldTimer.Start();
        }

        /// <summary>
        /// Selects a random key and presses it down
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void PickAndPressRandomMovementKey()
        {
            if (movementButtonHoldTimer != null)
                movementButtonHoldTimer.Stop();
            int index = rand.Next(0, 4);
            switch (index)
            {
                case 0:
                    currentHeldKey = (byte)W_KEY;
                    HoldKey(W_KEY);
                    break;
                case 1:
                    currentHeldKey = (byte)A_KEY;
                    HoldKey(A_KEY);
                    break;
                case 2:
                    currentHeldKey = (byte)S_KEY;
                    HoldKey(S_KEY);
                    break;
                case 3:
                    currentHeldKey = (byte)D_KEY;
                    HoldKey(D_KEY);
                    break;
                default:
                    break;
            }

            movementButtonHoldTimer = new Timer();
            movementButtonHoldTimer.Interval = timeToHoldMovementButtonInMilliseconds;
            movementButtonHoldTimer.Tick += new EventHandler(ReleaseCurrentHeldMovementKeyAndSelectNew);
            movementButtonHoldTimer.Start();
        }

        /// <summary>
        /// Lets go of the currently held key, and picks a new one to hold for a duration.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void ReleaseCurrentHeldKeyAndSelectNew(object o, EventArgs e)
        {
            ReleaseKey(C_KEY);
            PickAndPressAttackKey();
        }

        private void ReleaseCurrentHeldMovementKeyAndSelectNew(object o, EventArgs e)
        {
            ReleaseKey(currentHeldKey);
            PickAndPressRandomMovementKey();
        }
    }
}
