using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication26
{
    class Program
    {
        static void Main(string[] args)
        {
            var task = GetTaskAsync("http://www.baidu.com");
            Console.WriteLine(task.Result.Length);
            Console.ReadLine();
        }

        public static Task<byte[]> GetTaskAsync(string url)
        {
            TaskCompletionSource<byte[]> source = new TaskCompletionSource<byte[]>();
            WebClient wc = new WebClient();
            wc.DownloadDataCompleted += (sender, e) =>
            {
                try
                {
                    source.TrySetResult(e.Result);
                }
                catch (Exception ex)
                {
                    source.TrySetException(ex);
                }
            };
            wc.DownloadDataAsync(new Uri(url));
            return source.Task;
        }

        private static void Wc_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
