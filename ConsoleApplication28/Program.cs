using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication28
{
    class Program
    {
        static void Main(string[] args)
        {
            var info = Hello();

            Console.WriteLine("我去： " + info.Result);

            Console.Read();
        }

        static Task<string> Hello()
        {
            MyStateMachine machine = new MyStateMachine();

            machine.t_builder = AsyncTaskMethodBuilder<string>.Create();

            //TaskCompletionSource

            machine.state = -1;

            var t_builder = machine.t_builder;

            t_builder.Start(ref machine);

            return machine.t_builder.Task;
        }
    }

    public class MyStateMachine : IAsyncStateMachine
    {
        public AsyncTaskMethodBuilder<string> t_builder;

        public int state;

        private MyStateMachine machine = null;

        private TaskAwaiter<string> myawaiter;

        string result = string.Empty;

        public MyStateMachine()
        {
        }

        public void MoveNext()
        {
            try
            {
                switch (state)
                {
                    case -1:
                        Console.WriteLine("hello world");

                        var waiter = Task.Run(() =>
                        {
                            Console.WriteLine("i'm middle");

                            return "i'm ok";
                        }).GetAwaiter();

                        state = 0;  //设置下一个状态
                        myawaiter = waiter;

                        machine = this;

                        //丢给线程池执行了。。。
                        t_builder.AwaitUnsafeOnCompleted(ref waiter, ref machine);
                        break;

                    case 0:

                        var j = myawaiter.GetResult();

                        Console.WriteLine("我是结尾哦:{0}", j);

                        t_builder.SetResult(j);
                        break;
                }
            }
            catch (Exception ex)
            {
                t_builder.SetException(ex);  //设置t_builder的异常
            }
        }

        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }
    }
}
