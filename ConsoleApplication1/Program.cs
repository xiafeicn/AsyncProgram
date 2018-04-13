using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread thread=new Thread(new ThreadStart(() =>
            {

            }));
            thread.Start();
            Console.Read();
        }
    }
}
