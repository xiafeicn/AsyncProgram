using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication27
{
    class Program
    {
        static void Main(string[] args)
        {
            var info = Hello().Result;

            Console.WriteLine(info);

            Console.Read();
        }

        static async Task<string> Hello()
        {
            //主线程执行，底层还会调用一个 AwaitUnsafeOnCompleted 委托给线程池
            Console.WriteLine("hello world");

            //在工作线程中执行，movenext
            var x = await Task.Run(() =>
            {
                Console.WriteLine("i'm middle");

                return "i'm ok";
            });

            Console.WriteLine("我是结尾哦:{0}", x);

            return x;
        }
    }
}
