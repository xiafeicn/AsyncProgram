using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication11
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread t = new Thread(() =>
            {
                System.Threading.Thread.Sleep(100);

                Console.WriteLine("我是工作线程1");
            });

            Thread t2 = new Thread(() =>
            {
                System.Threading.Thread.Sleep(100);

                Console.WriteLine("我是工作线程2");
            });

            t.Start();
            t2.Start();

            t.Join();   // t1 && t2 都完成了 WaitAll操作。。。  WaitAny  t1 ||  t2 
            t2.Join();

            Console.WriteLine("我是主线程");

            Console.Read();
        }
    }
}
