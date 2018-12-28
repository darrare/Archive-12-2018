using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace TuringMachineSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            //Store the input as a list of strings
            List<string> input = ConvertFileToListOfStrings("input.txt");

            TuringMachine tm = new TuringMachine(ConvertFileToListOfStrings("p02.tm"));

            //Run the machine on each of the input values, store the results in output.
            List<string> output = new List<string>();
            foreach (string s in input)
            {
                output.Add(tm.RunMachineOnInput(s));
            }
            WriteToFile("p02.txt", output);


            //Loop through each of the 4 given machines
            //for (int i = 0; i <= 3; i++)
            //{
            //    //Create a new turing machine and send it the list of transitions
            //    TuringMachine tm = new TuringMachine(ConvertFileToListOfStrings("m" + String.Format("{0:00}", i) + ".tm"));

            //    //Run the machine on each of the input values, store the results in output.
            //    List<string> output = new List<string>();
            //    foreach (string s in input)
            //    {
            //        Console.WriteLine("On TM" + i + ". Running string " + s);
            //        output.Add(tm.RunMachineOnInput(s));
            //    }
            //    Console.WriteLine("Done with input on TM" + i);
            //    WriteToFile("m" + String.Format("{0:00}", i) + ".txt", output);
            //}

            Console.WriteLine("Done");
           
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
                while ((line = sr.ReadLine()) != null && line != "***") //*** is my break character for personal testing.
                {
                    strings.Add(line);
                }
            }
            return strings;
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
    }
}
