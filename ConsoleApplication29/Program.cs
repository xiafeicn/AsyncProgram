using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication29
{
    class Program
    {
        public static SpinLock spinLock = new SpinLock();

        static void Main(string[] args)
        {
            //Interlocked.Exchange(ref sum, 10);  // sum=10;
            //Interlocked.CompareExchange(ref num1, sum, num2);  // num1==num2 ; num1=sum;

            //比如开启5个task
            for (int i = 0; i < 5; i++)
            {
                Task.Factory.StartNew(() =>
                {
                    Run();
                });
            }

            Console.Read();
        }

        static int nums = 0;

        static void Run()
        {
            for (int i = 0; i < 100; i++)
            {
                try
                {
                    var b = false;

                    spinLock.Enter(ref b);

                    Console.WriteLine(nums++);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    spinLock.Exit();
                }
            }
        }
    }
}
