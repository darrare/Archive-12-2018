using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileLocker
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = "";
            do
            {
                Console.WriteLine("Enter the name (including the file extension) of the file");
                Console.WriteLine("you want to lock located in the same location as this executable.");
                fileName = Console.ReadLine();

                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\" + fileName))
                {
                    Console.WriteLine("File name doesn't exist. Try again.");
                }

            } while (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\" + fileName));


            StreamWriter sr = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\" + fileName, true);
            Console.WriteLine("Stream open, file locked.");
            Console.WriteLine("NOTE: PRESS ANY KEY TO FREE THE FILE.");
            Console.WriteLine("SIMPLY CLOSING THIS WINDOW WILL KEEP THE FILE LOCKED, WHICH WILL REQUIRE A RESTART TO FIX. (maybe)");

            Console.ReadKey();
            sr.Close();
        }
    }
}
