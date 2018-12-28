using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GooglesYoutubeVideo
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> intArr = new List<int>() { 1, 2, 4, 3 };
            Console.WriteLine(IsMatchingPair(intArr, 8));
            Console.ReadLine();
        }

        static bool IsMatchingPair(List<int> arr, int sum)
        {
            //return arr.Where(t => arr.Where(y => t + y == sum).ToList().Count == 0).ToList().Count != 0;
            //return arr.Any(t => arr.Where(y => (int)t + (int)y == sum && t != y).ToList().Count != 0);
            return arr.Any(x => arr.Count(i => i == x) > 1 && arr.Any(y => x + y == sum));
        }
    }
}
