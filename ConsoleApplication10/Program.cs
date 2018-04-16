using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication10
{
    class Program
    {
        static void Main(string[] args)
        {
            Task task = new Task(() =>
            {
                Console.WriteLine("我是工作线程： tid={0}", Thread.CurrentThread.ManagedThreadId);
            });

            task.Start();


            //var task = Task.Factory.StartNew(() =>
            //{
            //    Console.WriteLine("我是工作线程： tid={0}", Thread.CurrentThread.ManagedThreadId);
            //});


            ////使用Task的Run方法
            //var task = Task.Run(() =>
            //{
            //    Console.WriteLine("我是工作线程： tid={0}", Thread.CurrentThread.ManagedThreadId);
            //});

            ////这个是同步执行。。。。也就是阻塞执行。。。
            //var task = new Task(() =>
            //{
            //    Console.WriteLine("我是工作线程： tid={0}", Thread.CurrentThread.ManagedThreadId);
            //});

            //task.RunSynchronously();
            Console.Read();

        }
    }
}
