using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication7
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                Thread thread = new Thread(() =>
                {
                    for (int j = 0; j < 10; j++)
                    {
                        Console.WriteLine("work:{0}，tid={1}", i, Thread.CurrentThread.ManagedThreadId);
                    }
                });

                thread.Name = "main" + i;

                thread.Start();
            }

            Console.Read();
        }
    }
}
