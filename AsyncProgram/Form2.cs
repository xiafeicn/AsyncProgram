using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsyncProgram
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public void AppendText(string text)
        {
            //1.注意，controller的begininvoke是运行在UI线程上的，防止主界面假死，这段代码应该运行在非UI线程
            //2.如果从另外一个线程操作windows窗体上的控件，就会和主线程产生竞争，造成不可预料的结果，甚至死锁。因此windows GUI编程有一个规则，就是只能通过创建控件的线程来操作控件的数据，否则就可能产生不可预料的结果。
            // 因此，dotnet里面，为了方便地解决这些问题，Control类实现了ISynchronizeInvoke接口，提供了Invoke和BeginInvoke方法来提供让其它线程更新GUI界面控件的机制。
            this.BeginInvoke(new EventHandler((sender, e) => { listBox1.Items.Add(text); }));
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var tcs = new TaskCompletionSource<int>();

            AppendText(string.Format("main is running on a thread id {0}. is thread pool thread:{1}, priority {2}",
                Thread.CurrentThread.ManagedThreadId,
                Thread.CurrentThread.IsThreadPoolThread,
                Thread.CurrentThread.Priority));

            var worker = new BackgroundWorker();
            worker.DoWork += (s, eventArgs) =>
            {
                eventArgs.Result = TaskMethod("Background worker", 3);
            };

            worker.RunWorkerCompleted += (s, eventArgs) =>
            {
                if (eventArgs.Error != null)
                {
                    tcs.SetException(eventArgs.Error);
                }
                else if (eventArgs.Cancelled)
                {
                    tcs.SetCanceled();
                }
                else
                {
                    tcs.SetResult((int)eventArgs.Result);
                }
            };
            //Console.WriteLine("----------- 0 ---------------");  
            worker.RunWorkerAsync();

            int result = tcs.Task.Result;

            AppendText(string.Format("result is: {0}", result));
        }

         int TaskMethod(string name, int seconds)
        {
            AppendText(string.Format("Task {0} is running on a thread id {1}. is thread pool thread:{2}, priority {3}",
                name,
                Thread.CurrentThread.ManagedThreadId,
                Thread.CurrentThread.IsThreadPoolThread,
                Thread.CurrentThread.Priority));
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
            return seconds;
        }
    }
}
