using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication9
{
    /// <summary>
    /// threadpool 定时器任务
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            ThreadPool.RegisterWaitForSingleObject(new AutoResetEvent(true), new WaitOrTimerCallback((obj, b) =>
            {
                //做逻辑判断，判断是否在否以时刻执行。。。
                Console.WriteLine("obj={0}，tid={1}, datetime={2}", obj, Thread.CurrentThread.ManagedThreadId,
                    DateTime.Now);
            }), "hello world", 1000, false);

            Console.Read();
        }
    }
}
