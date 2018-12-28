using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuCalculation
{
    class Program
    {
        static void Main(string[] args)
        {
            float salesTax = .07125f;
            float salesTax2 = .08438f;

            Console.WriteLine("Price of first item");
            float firstItem = float.Parse(Console.ReadLine());

            Console.WriteLine("Price on credit card statement");
            float ccStatement = float.Parse(Console.ReadLine());

            Console.WriteLine("Price of second item: " + (ccStatement / (1 + salesTax) - firstItem));
            Console.WriteLine("Price of second item: " + (ccStatement / (1 + salesTax2) - firstItem));

           
            while (true) ;
        }
    }
}
