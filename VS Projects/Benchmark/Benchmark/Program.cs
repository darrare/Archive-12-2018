using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            Bench(50000);
        }

        public static void Bench(int y)
        {
            int[] myList = new int[y];
            

            Random rand = new Random();

            for (int i = 0; i < y; i++)
            {
                myList[i] = rand.Next(1000000000) + 1;
            }

            int temp;
            bool flag = true;
            Console.WriteLine(DateTime.Now);
            while (flag)
            {
                flag = false;
                for (int i = 0; i < myList.Length - 1; i++)
                {
                    if (myList[i] < myList[i + 1])
                    {
                        temp = myList[i];
                        myList[i] = myList[i + 1];
                        myList[i + 1] = temp;
                        flag = true;
                    }
                }
            }

            Console.WriteLine(DateTime.Now);
            Console.ReadLine();
        }
    }
}
