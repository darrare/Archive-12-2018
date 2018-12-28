using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Swagger
{
    static class Utilities
    {
        static Timer durationTimer;
        static Timer delayTimer;
        static Timer menuTimerSmall;
        static Timer stopMenuTimer;
        static Timer gameTimer;
        public static float gameTime = 1500 * 60; //1.5 minutes
        static float menuTime = 1500 * 60; //1.5 minutes
        static Random random = new Random();
        static int scalar = 100; //adjust for time.
        public static Form1 form;

        const int W_KEY = 0x57;
        const int A_KEY = 0x41;
        const int S_KEY = 0x53;
        const int D_KEY = 0x44;
        const int J_KEY = 0x4A; //for brawlhalla
        const int K_KEY = 0x4B; //for brawlhalla
        const int C_KEY = 0x43; //for brawlhalla

        //for displaying time
        public static DateTime startTime;

        public static void Start()
        {
            durationTimer = new Timer();
            PressKey(null, null);
        }

        public static void Stop()
        {
            if (durationTimer != null)
            {
                durationTimer.Enabled = false;
                durationTimer.Dispose();
            }

            if (delayTimer != null)
            {
                delayTimer.Enabled = false;
                delayTimer.Dispose();
            }
        }

        static void ReleaseKey(Object source, ElapsedEventArgs e)
        {
            form.ReleaseKey();
            durationTimer.Enabled = false;
            durationTimer.Dispose();

            durationTimer = new Timer(1000);//random.Next(1 * scalar, 3 * scalar));
            durationTimer.Elapsed += PressKey;
            durationTimer.Enabled = true;
            durationTimer.AutoReset = false;
        }

        static void PressKey(Object source, ElapsedEventArgs e)
        {
            form.HoldKey(J_KEY);
            durationTimer.Enabled = false;
            durationTimer.Dispose();

            durationTimer = new Timer(random.Next(10, 100));//random.Next(1 * scalar, 3 * scalar));
            durationTimer.Elapsed += ReleaseKey;
            durationTimer.Enabled = true;
            durationTimer.AutoReset = false;
        }
    }
}
