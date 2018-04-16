using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication24
{
    class Program
    {
        static void Main(string[] args)
        {
            var task = Task.Factory.StartNew(() =>
            {
                var childTask1 = Task.Factory.StartNew(() =>
                {
                    throw new Exception("child1 exception");
                }, TaskCreationOptions.AttachedToParent);
                var childTask2 = Task.Factory.StartNew(() =>
                {
                    throw new Exception("child2 exception");
                }, TaskCreationOptions.AttachedToParent);
            });

            try
            {
                task.Wait();
            }
            catch (AggregateException ex)
            {
                //当前的Handle就是来遍历 异常数组，如果有一个异常信息是这样的，我认为是已经处理的。
                //如果你觉得异常还需要往上抛，请返回false。。。
               ex.Handle(x =>
               {
                   if (ex.InnerException.Message == "child1 exception")
                   {
                       return true;
                   }

                   return false;
               });
            }

            Console.Read();
        }
    }
}
