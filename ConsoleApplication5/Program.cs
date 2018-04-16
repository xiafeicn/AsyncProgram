using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication5
{
    class Program
    {
        static void Main(string[] args)
        {
            var isStop = 0;

            var t = new Thread(() =>
            {
                var isSuccess = false;

                while (isStop == 0)
                {
                    //不要从CPU cache读取，而是从memory读取
                    Thread.MemoryBarrier();
                    //Thread.VolatileRead(ref isStop);
                    isSuccess = !isSuccess;
                }
            });

            t.Start();

            Thread.Sleep(1000);
            isStop = 1;
            t.Join();

            Console.WriteLine("主线程执行结束！");
            Console.ReadLine();
        }
    }
}
