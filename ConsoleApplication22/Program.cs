using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication22
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

            Task<int> task2 = Task.Factory.StartNew(() =>
            {
                //做一些逻辑运算
                return 2;
            });

            var task = Task.WhenAll<int>(new Task<int>[2] { task1, task2 });

            var result = task.Result;
            Console.WriteLine(result);
            Console.WriteLine(task2.Result);

            Console.Read();
        }
    }
}
