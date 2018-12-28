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
            StartTimer();
            StartGameTimer();
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

        static void StartTimer()
        {
            delayTimer = new Timer(10);//random.Next(30 * scalar, 50 * scalar));
            delayTimer.Elapsed += PressKey;
            delayTimer.Enabled = true;
            delayTimer.AutoReset = false;
        }

        static void StartGameTimer()
        {
            gameTimer = new Timer(gameTime);
            gameTimer.Elapsed += StartMenuOperations;
            gameTimer.Enabled = true;
            gameTimer.AutoReset = false;
            startTime = DateTime.Now;
        }

        static void ReleaseKey(Object source, ElapsedEventArgs e)
        {
            form.ReleaseKey();
            StartTimer();
        }

        static void PressKey(Object source, ElapsedEventArgs e)
        {
            int index = random.Next(0, 10);
            switch (index)
            {
                case 0:
                    form.HoldKey(W_KEY);
                    break;
                case 1:
                    form.HoldKey(A_KEY);
                    break;
                case 2:
                    form.HoldKey(S_KEY);
                    break;
                case 3:
                    form.HoldKey(D_KEY);
                    break;
                case 4:
                    form.HoldKey(K_KEY);
                    break;
                default:
                    form.HoldKey(J_KEY);
                    break;
            }

            durationTimer = new Timer(200);//random.Next(1 * scalar, 3 * scalar));
            durationTimer.Elapsed += ReleaseKey;
            durationTimer.Enabled = true;
            durationTimer.AutoReset = false;
        }

        //Below will handle the transition between menu and game.

        static void PressCRepeating(Object source, ElapsedEventArgs e)
        {
            form.PressKey(C_KEY);

            menuTimerSmall = new Timer(100);//random.Next(1 * scalar, 3 * scalar));
            menuTimerSmall.Elapsed += PressCRepeating;
            menuTimerSmall.Enabled = true;
            menuTimerSmall.AutoReset = false;
        }

        static void StartMenuOperations(Object source, ElapsedEventArgs e)
        {
            PressCRepeating(null, null);

            stopMenuTimer = new Timer(menuTime);
            stopMenuTimer.Elapsed += StopMenuOperations;
            stopMenuTimer.Enabled = true;
            stopMenuTimer.AutoReset = false;

            gameTimer.Enabled = false;
            gameTimer.Dispose();
        }

        static void StopMenuOperations(Object source, ElapsedEventArgs e)
        {
            menuTimerSmall.Enabled = false;
            menuTimerSmall.Dispose();
            Start();
        }
    }
}
