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
    public partial class Form1 : Form
    {
        public delegate void AsyncHandler();
        public Form1()
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

        public void Method()
        {
            AppendText(string.Format("AsyncTest类中的Method()函数的线程ID是{0}", Thread.CurrentThread.ManagedThreadId));//Environment.CurrentManagedThreadId
            if (Thread.CurrentThread.IsThreadPoolThread)//判断当前线程是否托管在线程池上
            {
                Console.WriteLine("AsyncTest类中的Method()函数的线程托管于线程池");
            }
            else
            {
                Console.WriteLine("AsyncTest类中的Method()函数的线程没有托管在线程池上");
            }
        }

        public void AsyncMethod()
        {
            AppendText(string.Format("AsyncTest类中的Method()函数的线程ID是{0}", Thread.CurrentThread.ManagedThreadId));//Environment.CurrentManagedThreadId
            if (Thread.CurrentThread.IsThreadPoolThread)//判断当前线程是否托管在线程池上
            {
                AppendText(string.Format("AsyncTest类中的Method()函数的线程托管于线程池"));
            }
            else
            {
                AppendText(string.Format("AsyncTest类中的Method()函数的线程没有托管在线程池上"));
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            AppendText(string.Format("Program类中的Main()函数的线程ID是{0}", Thread.CurrentThread.ManagedThreadId));//Environment.CurrentManagedThreadId
            if (Thread.CurrentThread.IsThreadPoolThread)//判断当前线程是否托管在线程池上
            {
                AppendText(string.Format("Program类中的Main()函数的线程托管于线程池"));
            }
            else
            {
                AppendText(string.Format("Program类中的Main()函数的线程没有托管在线程池上"));
            }
            Console.WriteLine();

            //把Method 方法分配给委托对象
            AsyncHandler async = AsyncMethod; //

            //发起一个异步调用的方法,返回IAsyncResult 对象
            IAsyncResult result = async.BeginInvoke(null, null);
            
            //这里会阻碍线程,直到方法执行完毕
            async.EndInvoke(result);
        }
    }
}
