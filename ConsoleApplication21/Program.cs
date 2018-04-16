using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication21
{
    class Program
    {
        static void Main(string[] args)
        {
            Task<int> task1 = Task.Factory.StartNew(() =>
            {
                //做一些逻辑运算
                return 1;
            });

            var task2 = task1.ContinueWith<string>(t =>
            {
                int num = t.Result;

                var sum = num + 10;

                return sum.ToString();
            });

            Console.WriteLine(task2.Result);

            Console.Read();
        }
    }
}
