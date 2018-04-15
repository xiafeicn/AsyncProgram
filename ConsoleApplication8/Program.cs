using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication8
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadPool.QueueUserWorkItem((obj) =>
            {
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine("work:{0}，tid={1}", i, Thread.CurrentThread.ManagedThreadId);
                }
            });

            Console.WriteLine("主线程ID：{0}", Thread.CurrentThread.ManagedThreadId);

            Console.Read();
        }
    }
}
