using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    class Program
    {
        [ThreadStatic]
        static string username = string.Empty;

        static void Main(string[] args)
        {
            username = "hello world!!!";

            var t = new Thread(() =>
            {
                Console.WriteLine("当前工作线程:{0}", username);
            });

            t.Start();

            Console.WriteLine("主线程:{0}", username);

            Console.Read();
        }
    }
}
