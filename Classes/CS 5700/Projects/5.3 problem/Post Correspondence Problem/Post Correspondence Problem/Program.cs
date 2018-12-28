using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post_Correspondence_Problem
{
    class Program
    {
        static List<string[]> tiles = new List<string[]>();
        static List<string[]> stringsToRun = new List<string[]>();

        static List<string[]> results = new List<string[]>();

        static void Main(string[] args)
        {
            tiles.Add(new string[] { "ab", "abab" });
            tiles.Add(new string[] { "b", "a" });
            tiles.Add(new string[] { "aba", "b" });
            tiles.Add(new string[] { "aa", "a" });

            stringsToRun.Add(tiles[0]);
            stringsToRun.Add(tiles[1]);
            stringsToRun.Add(tiles[2]);
            stringsToRun.Add(tiles[3]);

            while(results.Count < 14)
            {
                for (int i = stringsToRun.Count - 1; i > 0; i--)
                {
                    List<string[]> newAdditions = RecursivelySolvePostCorrespondenceProblem(stringsToRun[i][0], stringsToRun[i][1]);
                    foreach (string[] na in newAdditions)
                    {
                        stringsToRun.Add(na);
                    }
                    stringsToRun.RemoveAt(i);
                }
            }

            foreach (string[] t in results)
            {
                Console.WriteLine(t[0]);
                Console.WriteLine(t[1]);
                Console.WriteLine();
            }

            while (true) ;
        }

        static List<string[]> RecursivelySolvePostCorrespondenceProblem(string top, string bottom)
        {
            List<string[]> returnVal = new List<string[]>();

            foreach(string[] t in tiles)
            {
                string tempTop = top.Replace("|", "");
                string tempBottom = bottom.Replace("|", "");
                tempTop += t[0];
                tempBottom += t[1];
                if (tempTop == tempBottom)
                {
                    results.Add(new string[] { top + "|" + t[0], bottom + "|" + t[1] });
                    return returnVal;
                }

                //otherwise
                //if top is bigger than bottom
                if (tempTop.Length > tempBottom.Length)
                {
                    if (tempTop.Substring(0, tempBottom.Length) == tempBottom)
                    {
                        returnVal.Add(new string[] { top + "|" + t[0], bottom + "|" + t[1] });

                    }
                }
                else //bottom is bigger than top.
                {
                    if (tempBottom.Substring(0, tempTop.Length) == tempTop)
                    {
                        returnVal.Add(new string[] { top + "|" + t[0], bottom + "|" + t[1] });
                    }
                }

            }
            return returnVal;
        }
    }
}
