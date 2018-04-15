using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            //Thread.AllocateDataSlot("token");
            //在所有现成上分配了token得槽位
            var slot = Thread.AllocateNamedDataSlot("token");
            //主线程上设置槽位，token只能被主线程读取，其他线程无法读取
            Thread.SetData(slot, Guid.NewGuid().ToString());
            //Thread.FreeNamedDataSlot("");
            var t = new Thread(() =>
            {
                // Thread.SetData(slot, Guid.NewGuid().ToString());
                var obj = Thread.GetData(slot);

                Console.WriteLine("当前工作线程:{0}", obj);
            });

            t.Start();

            var obj2 = Thread.GetData(slot);

            Console.WriteLine("主线程:{0}", obj2);

            Console.Read();
        }


      
    }
}
