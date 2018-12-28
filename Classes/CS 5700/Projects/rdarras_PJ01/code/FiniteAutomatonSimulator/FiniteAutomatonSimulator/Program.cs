using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FiniteAutomatonSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            int index = 0;
            for (int i = 0; i <= 30; i++)
            {
                Console.WriteLine("Working on m" + String.Format("{0:00}", i) + ".fa");
                FiniteAutomatonManager.Instance.CreateFiniteAutomataFromStrings(ConvertFileToListOfStrings("m" + String.Format("{0:00}", i) + ".fa")); //L = { a*b*a*(b*c*)* }
                List<string> strings = FiniteAutomatonManager.Instance.TestFiniteAutomataAgainstStrings(ConvertFileToListOfStrings("input.txt"));
                WriteToFile("m" + String.Format("{0:00}", i) + ".txt", strings);
                WriteToFile("m" + String.Format("{0:00}", i) + ".log", FiniteAutomatonManager.Instance.GetLogDataFromCurrentMachine());
            }
            //FiniteAutomatonManager.Instance.CreateFiniteAutomataFromStrings(ConvertFileToListOfStrings("m00.txt")); //L = {w | next to last is a 'b' or odd # of a's }
            //FiniteAutomatonManager.Instance.CreateFiniteAutomataFromStrings(ConvertFileToListOfStrings("m01.txt")); //L = { a*b*a*(b*c*)* }
            

            //foreach (string s in strings.Where(t => t[t.Length - 2] == 'b').ToList())
            //{
            //    Console.WriteLine(s);
            //}
            //Console.WriteLine();
            //foreach (string s in strings.Where(t => t.Count(x => x == 'a') % 2 == 1).ToList())
            //{
            //    Console.WriteLine(s);
            //}
        }

        /// <summary>
        /// Opens the machine file and converts it to a list of strings that we can parse through.
        /// </summary>
        /// <param name="fileName">Name of the file. App path will be appended (app path will be location of binary)</param>
        /// <returns>The new list of strings</returns>
        static List<string> ConvertFileToListOfStrings(string fileName)
        {
            string appPath = AppDomain.CurrentDomain.BaseDirectory + "Machines\\" + fileName;
            List<string> strings = new List<string>();

            using (StreamReader sr = new StreamReader(appPath))
            {
                string line = "";
                while((line = sr.ReadLine()) != null && line != "***") //*** is my break character for personal testing.
                {
                    strings.Add(line);
                }
            }
            return strings;
        }

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
    }
}
