using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication17
{
    class Program
    {
        static void Main(string[] args)
        {
            CancellationTokenSource source = new CancellationTokenSource();

            source.Token.Register(() =>
            {
                //如果当前的token被取消，此函数将会被执行
                Console.WriteLine("当前source已经被取消，现在可以做资源清理了。。。。");
            });

            var task = Task.Factory.StartNew(() =>
            {
                while (!source.IsCancellationRequested)
                {
                    Thread.Sleep(100);

                    Console.WriteLine("当前thread={0} 正在运行", Thread.CurrentThread.ManagedThreadId);
                }
            }, source.Token);

            source.CancelAfter(1000);//1秒后自动取消

            Console.Read();
        }
    }
}
