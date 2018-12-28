using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAbuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            int b = 5;
            int d = 3;
            bool acceptEmptyString = true;
            bool mostSignificantDigitFirst = true;
            string fileName = "p22.fa";

            List<TransitionState> states = GetAutomatonForBaseBDivisibleByD(b, d);

            GenerateFAFile(states, acceptEmptyString, b, d, fileName);




            //The below commented code was just there to do some busywork for me for a few of the problems.

            //List<string> lines = new List<string>();
            ////lines.Add("{5,10}");

            //for (int i = 0; i < 256; i++)
            //{
            //    lines.Add("2," + (char)i + ",2");
            //}

            //for (int i = 0; i < 15; i++)
            //{
            //    for (int j = 0; j < 3; j++)
            //    {
            //        lines.Add(i + "," + j + ",");
            //    }
            //}

            //string appPath = AppDomain.CurrentDomain.BaseDirectory + "Machines\\" + fileName;

            //using (StreamWriter sw = new StreamWriter(appPath))
            //{
            //    foreach (string s in lines)
            //    {
            //        sw.WriteLine(s);
            //    }
            //}
        }

        public static List<TransitionState> GetAutomatonForBaseBDivisibleByD(int b, int d)
        {
            List<TransitionState> states = new List<TransitionState>();
            for (int from = 0; from < d; from++) //n
            {
                for (int input = 0; input < b; input++) //d
                {
                    states.Add(new TransitionState(from, input, ((from * b) + input) % d));
                }
            }

            return states;
        }

        public static void GenerateFAFile(List<TransitionState> states, bool acceptEmptyString, int b, int d, string fileName)
        {
            int acceptState = 0;
            if (!acceptEmptyString)
            {
                foreach(TransitionState s in states)
                {
                    s.from += 1;
                    s.to += 1;
                }
                for (int i = 0; i < b; i++)
                {
                    states.Add(new TransitionState(0, i, (i % d) + 1));
                }
                acceptState = 1;
            }

            List<string> lines = new List<string>();
            lines.Add("{" + acceptState + "}");

            foreach (TransitionState s in states)
            {
                lines.Add(s.from + "," + s.input + "," + s.to);
            }

            string appPath = AppDomain.CurrentDomain.BaseDirectory + "Machines\\" + fileName;

            using (StreamWriter sw = new StreamWriter(appPath))
            {
                foreach (string s in lines)
                {
                    sw.WriteLine(s);
                }
            }
        }
    }

    public class TransitionState
    {
        public int from, input, to;

        public TransitionState(int from, int input, int to)
        {
            this.from = from;
            this.input = input;
            this.to = to;

            Console.WriteLine("From " + from + ", with " + input + ", to " + to);
        }
    }
}
