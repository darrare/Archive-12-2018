using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TuringMachineGeneratingTool
{
    class Program
    {
        static int zero = '0';
        static int one = '1';
        static int x = 'x';
        static int y = 'y';
        static int blank = 0;
        static int pound = 1;
        //0 is blank symbol, 1 is our symbol for separation
        static void Main(string[] args)
        {
            List<string> lines = new List<string>();

            lines = Generatep01TM();
            WriteToFile("p01.tm", lines);

            lines = Generatep02TM();
            WriteToFile("p02.tm", lines);

        }

        /// <summary>
        /// Write to a file a list of strings.
        /// </summary>
        /// <param name="fileName">file to write to</param>
        /// <param name="lines">lines to write</param>
        static void WriteToFile(string fileName, List<string> lines)
        {
            string appPath = AppDomain.CurrentDomain.BaseDirectory + "Outputs\\" + fileName;

            using (StreamWriter sw = new StreamWriter(appPath))
            {
                foreach (string s in lines)
                {
                    sw.WriteLine(s);
                }
            }
        }

        static List<string> Generatep01TM()
        {
            List<string> lines = new List<string>();
            lines.Add(0 + "," + blank + "," + 254 + "," + zero + "," + "R");

            //sigma transitions from 0-1 and 1-1
            for (int i = 32; i < 255; i++)
            {
                lines.Add(1 + "," + i + "," + 1 + "," + x + "," + "R");
                lines.Add(0 + "," + i + "," + 1 + "," + pound + "," + "R");
            }


            lines.Add(1 + "," + blank + "," + 2 + "," + x + "," + "R");

            //Generates 2-33 and the transition from 33 to 34
            for (int i = 2; i < 33; i += 4)
            {
                lines.Add((i) + "," + blank + "," + (i) + "," + blank + "," + "L");
                lines.Add((i) + "," + x + "," + (i) + "," + x + "," + "L");
                lines.Add((i) + "," + pound + "," + (i + 1) + "," + zero + "," + "R");
                lines.Add((i + 1) + "," + x + "," + (i + 2) + "," + pound + "," + "R");
                lines.Add((i + 2) + "," + x + "," + (i + 2) + "," + x + "," + "R");
                lines.Add((i + 2) + "," + blank + "," + (i + 3) + "," + x + "," + "R");
                lines.Add((i + 3) + "," + blank + "," + (i + 1) + "," + blank + "," + "R");
                lines.Add((i + 3) + "," + x + "," + (i + 1) + "," + x + "," + "R");
                lines.Add((i + 1) + "," + blank + "," + (i + 4) + "," + blank + "," + "L");
                lines.Add((i + 1) + "," + x + "," + (i + 4) + "," + x + "," + "L");
            }

            //34 transitions
            lines.Add(34 + "," + blank + "," + 34 + "," + blank + "," + "L");
            lines.Add(34 + "," + x + "," + 34 + "," + x + "," + "L");
            lines.Add(34 + "," + pound + "," + 35 + "," + pound + "," + "R");



            lines.Add(35 + "," + y + "," + 35 + "," + y + "," + "R");
            lines.Add(35 + "," + x + "," + 36 + "," + y + "," + "L");
            //lines.Add(35 + "," + blank + "," + 254 + "," + y + "," + "L");
            lines.Add(35 + "," + blank + "," + 39 + "," + blank + "," + "L");

            lines.Add(36 + "," + y + "," + 36 + "," + y + "," + "L");
            lines.Add(36 + "," + pound + "," + 37 + "," + pound + "," + "L");

            lines.Add(37 + "," + one + "," + 37 + "," + one + "," + "L");
            lines.Add(37 + "," + zero + "," + 38 + "," + one + "," + "R");

            lines.Add(38 + "," + one + "," + 38 + "," + zero + "," + "R");
            lines.Add(38 + "," + pound + "," + 35 + "," + pound + "," + "R");

            //39 transition
            lines.Add(39 + "," + y + "," + 39 + "," + blank + "," + "L");
            lines.Add(39 + "," + blank + "," + 39 + "," + blank + "," + "L");
            lines.Add(39 + "," + pound + "," + 40 + "," + blank + "," + "L");
            //lines.Add(39 + "," + pound + "," + 254 + "," + pound + "," + "L");

            //40-48 transitions
            for (int i = 40; i < 48; i++)
            {
                lines.Add(i + "," + zero + "," + (i + 1) + "," + zero + "," + "L");
                lines.Add(i + "," + one + "," + (i + 1) + "," + one + "," + "L");
            }

            //48 transition
            lines.Add(48 + "," + zero + "," + 49 + "," + y + "," + "R");
            lines.Add(48 + "," + 1 + "," + 254 + "," + 1 + "," + "R");
            //lines.Add(48 + "," + zero + "," + 254 + "," + zero + "," + "R");
            //lines.Add(40 + "," + zero + "," + 254 + "," + zero + "," + "L");
            //lines.Add(40 + "," + one + "," + 254 + "," + zero + "," + "L");

            //begin remove leading zero module
            lines.Add(49 + "," + zero + "," + 49 + "," + x + "," + "R");
            lines.Add(49 + "," + one + "," + 50 + "," + x + "," + "L");

            lines.Add(50 + "," + x + "," + 50 + "," + x + "," + "L");
            lines.Add(50 + "," + y + "," + 51 + "," + one + "," + "R");

            //51 transitions
            lines.Add(51 + "," + x + "," + 51 + "," + x + "," + "R");
            lines.Add(51 + "," + one + "," + 52 + "," + x + "," + "L");
            lines.Add(51 + "," + zero + "," + 54 + "," + x + "," + "L");
            lines.Add(51 + "," + blank + "," + 56 + "," + blank + "," + "L");

            lines.Add(52 + "," + x + "," + 52 + "," + x + "," + "L");
            lines.Add(52 + "," + one + "," + 53 + "," + one + "," + "R");
            lines.Add(52 + "," + zero + "," + 53 + "," + zero + "," + "R");

            lines.Add(53 + "," + x + "," + 51 + "," + one + "," + "R");

            lines.Add(54 + "," + x + "," + 54 + "," + x + "," + "L");
            lines.Add(54 + "," + one + "," + 55 + "," + one + "," + "R");
            lines.Add(54 + "," + zero + "," + 55 + "," + zero + "," + "R");

            lines.Add(55 + "," + x + "," + 51 + "," + zero + "," + "R");

            lines.Add(56 + "," + x + "," + 56 + "," + blank + "," + "L");
            lines.Add(56 + "," + one + "," + 254 + "," + one + "," + "L");
            lines.Add(56 + "," + zero + "," + 254 + "," + zero + "," + "L");
            return lines;
        }


        static List<string> Generatep02TM()
        {
            List<string> lines = new List<string>();
            lines.Add(0 + "," + blank + "," + 254 + "," + zero + "," + "R");

            //sigma transitions from 0-1 and 1-1
            for (int i = 32; i < 255; i++)
            {
                lines.Add(1 + "," + i + "," + 1 + "," + x + "," + "R");
                lines.Add(0 + "," + i + "," + 1 + "," + pound + "," + "R");
            }


            lines.Add(1 + "," + blank + "," + 2 + "," + x + "," + "R");

            //Generates leading 0's
            for (int i = 2; i < 17; i += 4)
            {
                lines.Add((i) + "," + blank + "," + (i) + "," + blank + "," + "L");
                lines.Add((i) + "," + x + "," + (i) + "," + x + "," + "L");
                lines.Add((i) + "," + pound + "," + (i + 1) + "," + zero + "," + "R");
                lines.Add((i + 1) + "," + x + "," + (i + 2) + "," + pound + "," + "R");
                lines.Add((i + 2) + "," + x + "," + (i + 2) + "," + x + "," + "R");
                lines.Add((i + 2) + "," + blank + "," + (i + 3) + "," + x + "," + "R");
                lines.Add((i + 3) + "," + blank + "," + (i + 1) + "," + blank + "," + "R");
                lines.Add((i + 3) + "," + x + "," + (i + 1) + "," + x + "," + "R");
                lines.Add((i + 1) + "," + blank + "," + (i + 4) + "," + blank + "," + "L");
                lines.Add((i + 1) + "," + x + "," + (i + 4) + "," + x + "," + "L");
            }

            lines.Add(18 + "," + pound + "," + 19 + "," + pound + "," + "R");
            lines.Add(18 + "," + x + "," + 18 + "," + x + "," + "L");
            lines.Add(18 + "," + blank + "," + 18 + "," + blank + "," + "L");

            lines.Add(19 + "," + y + "," + 19 + "," + y + "," + "R");
            lines.Add(19 + "," + x + "," + 20 + "," + y + "," + "L");
            lines.Add(19 + "," + blank + "," + 32 + "," + blank + "," + "R");

            lines.Add(20 + "," + y + "," + 20 + "," + y + "," + "L");
            lines.Add(20 + "," + pound + "," + 21 + "," + pound + "," + "L");

            lines.Add(21 + "," + (int)(9.ToString()[0]) + "," + 21 + "," + zero + "," + "L");
            for (int i = 0; i <= 8; i++)
                lines.Add(21 + "," + (int)(i.ToString()[0]) + "," + 22 + "," + (int)((i + 1).ToString()[0]) + "," + "R");

            lines.Add(22 + "," + pound + "," + 19 + "," + pound + "," + "R");
            for (int i = 0; i <= 9; i++)
                lines.Add(22 + "," + (int)(i.ToString()[0]) + "," + 22 + "," + (int)((i).ToString()[0]) + "," + "R");

            lines.Add(32 + "," + y + "," + 32 + "," + blank + "," + "L");
            lines.Add(32 + "," + blank + "," + 32 + "," + blank + "," + "L");
            lines.Add(32 + "," + pound + "," + 33 + "," + blank + "," + "L");

            for (int i = 33; i < 37; i++)
            {
                for (int c = 0; c <= 9; c++)
                {
                    lines.Add(i + "," + (int)(c.ToString()[0]) + "," + (i + 1) + "," + (int)(c.ToString()[0]) + "," + "L");
                }
            }

            lines.Add(37 + "," + zero + "," + 38 + "," + y + "," + "R");
            for (int i = 1; i <= 9; i++)
                lines.Add(37 + "," + (int)(i.ToString()[0]) + "," + 254 + "," + (int)(i.ToString()[0]) + "," + "R");

            for (int i = 0; i <= 9; i++)
            {
                lines.Add(38 + "," + (int)(i.ToString()[0]) + "," + (39+i) + "," + y + "," + "L");
                for (int j = 0; j <= 9; j++)
                    lines.Add((39 + i) + "," + (int)(j.ToString()[0]) + "," + (39 + i) + "," + (int)(j.ToString()[0]) + "," + "L");
                lines.Add((39 + i) + "," + y + "," + (39 + i + 10) + "," + (int)(i.ToString()[0]) + "," + "R");
                lines.Add((39 + i + 10) + "," + y + "," + 38 + "," + y + "," + "R");
            }

            lines.Add(38 + "," + blank + "," + 200 + "," + blank + "," + "L");
            lines.Add(200 + "," + y + "," + 33 + "," + blank + "," + "L");

            return lines;
        }
    }
}
