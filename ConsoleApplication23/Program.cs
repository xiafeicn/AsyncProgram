using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication23
{
    class Program
    {
        static void Main(string[] args)
        {
            var task = Task.Factory.StartNew(() =>
            {
                var childTask1= Task.Factory.StartNew(() =>
                {
                    throw new Exception("child1 exception");
                },TaskCreationOptions.AttachedToParent);
                var childTask2 = Task.Factory.StartNew(() =>
                {
                    throw new Exception("child2 exception");
                }, TaskCreationOptions.AttachedToParent);
            });

            try
            {
                task.Wait();
            }
            catch(AggregateException ex)
            {
                foreach (var exception in ex.InnerExceptions)
                {
                    Console.WriteLine("message {0}  type{1}", exception.InnerException.Message, exception.GetType().Name);
                }
            }

            Console.Read();
        }
    }
}
