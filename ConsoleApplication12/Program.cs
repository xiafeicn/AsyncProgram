﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication12
{
    class Program
    {
        static void Main(string[] args)
        {
            Task task1 = new Task(() =>
            {
                System.Threading.Thread.Sleep(1000);

                Console.WriteLine("我是工作线程1:{0}", DateTime.Now);
            });

            task1.Start();

            Task task2 = new Task(() =>
            {
                System.Threading.Thread.Sleep(2000);

                Console.WriteLine("我是工作线程2:{0}", DateTime.Now);
            });

            task2.Start();

            Task.WhenAll(task1, task2).ContinueWith(t =>
            {
                //执行“工作线程3”的内容
                Console.WriteLine("我是工作线程 {0}", DateTime.Now);
            });
            Console.WriteLine("等带readkey");
            Console.Read();
        }
    }
}
