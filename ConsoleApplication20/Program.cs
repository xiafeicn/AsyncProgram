using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication20
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

            task1.Wait();

            Console.WriteLine(task1.Result);

            Console.Read();
        }
    }
}
