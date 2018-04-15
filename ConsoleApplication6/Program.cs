using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication6
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadPool.QueueUserWorkItem((obj) =>
            {
                var func = obj as Func<string>;

                Console.WriteLine("我是工作线程:{0}, content={1}", Thread.CurrentThread.ManagedThreadId,
                                                                   func());
            }, new Func<string>(() => "hello world"));

            Console.WriteLine("主线程ID：{0}", Thread.CurrentThread.ManagedThreadId);

            Console.Read();
        }
    }
}
