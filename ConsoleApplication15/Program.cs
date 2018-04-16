using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication15
{
    class Program
    {
        static void Main(string[] args)
        {
            Task task = new Task(() =>
            {
                Task task1 = new Task(() =>
                {
                    Thread.Sleep(100);
                    Console.WriteLine("task1");
                }, TaskCreationOptions.AttachedToParent);

                Task task2 = new Task(() =>
                {
                    Thread.Sleep(10);
                    Console.WriteLine("task2");
                }, TaskCreationOptions.AttachedToParent);

                task1.Start();

                task2.Start();
            }/*,TaskCreationOptions.DenyChildAttach*/);

            task.Start();

            task.Wait();  //task.WaitAll(task1,task2);

            Console.WriteLine("我是主线程！！！！");

            Console.Read();

        }
    }
}
