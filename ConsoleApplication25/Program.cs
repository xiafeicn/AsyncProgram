using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication25
{
    class Program
    {
        static void Main(string[] args)
        {

            FileStream fs = new FileStream(Environment.CurrentDirectory + "//1.txt", FileMode.Open);

            var bytes = new byte[fs.Length];

            fs.BeginRead(bytes, 0, bytes.Length, (aysc) =>
            {
                var nums = fs.EndRead(aysc);

                Console.WriteLine(nums);

            }, string.Empty);



            ////task 包装 APM
            //FileStream fs = new FileStream(Environment.CurrentDirectory + "//1.txt", FileMode.Open);

            //var bytes = new byte[fs.Length];

            //var task = Task.Factory.FromAsync(fs.BeginRead, fs.EndRead, bytes, 0, bytes.Length, string.Empty);

            //var nums = task.Result;

            //Console.WriteLine(nums);

            Console.Read();
        }
    }
}
