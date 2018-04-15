using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication4
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadLocal<string> local = new ThreadLocal<string>();

            local.Value = "hello world!!!";

            var t = new Thread(() =>
            {
                Console.WriteLine("当前工作线程:{0}", local.Value);
            });

            t.Start();

            Console.WriteLine("主线程:{0}", local.Value);

            Console.Read();
        }
    }
}
