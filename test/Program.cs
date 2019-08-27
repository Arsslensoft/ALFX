using Al.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test
{
   

    class Program
    {
        static void Main(string[] args)
        {
        

         //   sbloc y = Encoding.UTF8.GetBytes("len");
         //   sbloc x = Encoding.UTF8.GetBytes("idadi");
         //   //Console.WriteLine(x);
         ////arsslen idadi
         //   //x = x-1;
         //   //Console.WriteLine(x);
         //   x = x + " arsslen";
         //   Console.WriteLine(x.ToString());
         //   x = x - y;
         //   y.StartIndex = 1;
         //   sbloc z = x * y;
         //   Console.WriteLine(x.ToString());

         //   Console.WriteLine(z.ToString());
         //   sbloc[] s = x / 6;
         //   foreach (sbloc a in s)
         //       Console.WriteLine(a.ToString());

         //   usbloc alpha = (usbloc)BitConverter.GetBytes((int)1995) + BitConverter.GetBytes((int)11);
         // alpha = alpha + (byte)11;
         //   int[] arr = alpha;
            //binary a = 45978.789;
            //double b = a;
            //Console.WriteLine(a.ToString());
            //Mathexpr ex = "@(Variable1) + $pi()+$sin(90) + 5 + 6";
            //ex.Define("Variable1",12);
            ////ex += eval_AdditionalFunctionEventHandler;
            //long a = ex;
            numeric a = "1234";
            LReal x = 1;
            x += 2;
            x--;
            Console.WriteLine(x * 2);
            Console.WriteLine((x * 8));

            //varchar b = "idadi";
            //string asd = a +" "+ b;
            //a = "idadi";
            //Console.WriteLine((a == b));
      
            Console.Read();
        }

    }
}
