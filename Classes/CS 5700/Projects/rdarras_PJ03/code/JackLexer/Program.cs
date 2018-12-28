using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackLexer
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteToFile("Main.tok", new JackTokenizer(LoadJackFileAsString("Main.jack")).TokenizeString());
            WriteToFile("Ball.tok", new JackTokenizer(LoadJackFileAsString("Ball.jack")).TokenizeString());
            WriteToFile("Bat.tok", new JackTokenizer(LoadJackFileAsString("Bat.jack")).TokenizeString());
            WriteToFile("PongGame.tok", new JackTokenizer(LoadJackFileAsString("PongGame.jack")).TokenizeString());
            WriteToFile("Square.tok", new JackTokenizer(LoadJackFileAsString("Square.jack")).TokenizeString());
            WriteToFile("SquareGame.tok", new JackTokenizer(LoadJackFileAsString("SquareGame.jack")).TokenizeString());

            //TESTING CODE BELOW: this code was to help generate transitions in my DFAs that were along the lines of, "2 - 3, all ascii characters"
            //List<string> strings = new List<string>();
            //for (int i = 0; i < 127; i++)
            //{
            //    if ((char)i != '"')
            //    {
            //        strings.Add("2," + (char)i + ",2");
            //        if ((char)i != '/')
            //        {
            //            strings.Add("3," + (char)i + ",2");
            //        }
            //        if ((char)i != '*')
            //            strings.Add("6," + (char)i + ",2");
            //    }
            //}

            //WriteToFile("COMMENT_BLK.fa", strings);
        }


        /// <summary>
        /// Gets a single string from the jack file.
        /// </summary>
        /// <param name="fileName">Name of the file. "Bat.jack" App path will be appended (app path will be location of binary)</param>
        /// <returns>The file as a string</returns>
        static string LoadJackFileAsString(string fileName)
        {
            string appPath = AppDomain.CurrentDomain.BaseDirectory + "JackFiles\\" + fileName;
            string retVal;

            using (StreamReader sr = new StreamReader(appPath))
            {
                retVal = sr.ReadToEnd();
            }
            return retVal;
        }

        /// <summary>
        /// Creates a new file with the specified lines and file name
        /// </summary>
        /// <param name="fileName">The name of the file you want, include the extension</param>
        /// <param name="lines">The lines you want in this file.</param>
        static void WriteToFile(string fileName, List<string> lines)
        {
            string appPath = AppDomain.CurrentDomain.BaseDirectory + "Output\\" + fileName;

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
