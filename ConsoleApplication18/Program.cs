using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication18
{
    class Program
    {
        static void Main(string[] args)
        {
            CancellationTokenSource source1 = new CancellationTokenSource();

            //现在要让source1取消
            //source1.Cancel();

            CancellationTokenSource source2 = new CancellationTokenSource();

            source2.Cancel();

            var combineSource = CancellationTokenSource.CreateLinkedTokenSource(source1.Token, source2.Token);

            Console.WriteLine("s1={0}  s2={1}  s3={2}", source1.IsCancellationRequested,
                source2.IsCancellationRequested,
                combineSource.IsCancellationRequested);

            Console.Read();
        }
    }
}
