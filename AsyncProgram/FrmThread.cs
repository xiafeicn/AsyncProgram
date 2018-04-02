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
    public partial class FrmThread : Form
    {


        //空间开销:1.内核结构（osid，上下文(cpu寄存器的里面的一些变量)）2. thread环境块(Tls, exceptionList) 3.分配的堆栈空间（参数局部变量）4.内核模式堆栈（在CLR的线程操作，包括线程同步，大多都是调用底层的win32函数，用户模式堆栈需要传递到内核模式）

        //时间开销：1.启动程序加载的exe和dll，元数据等（开启一个thread，销毁一个thread，都会通知进程中的dll，attach，detach标志位。 ）
        //2.时间片切换开销

        public FrmThread()
        {
            InitializeComponent();
            Thread t = new Thread(new ThreadStart(() =>

            {



            }));

            t.Start();
        }
    }
}
