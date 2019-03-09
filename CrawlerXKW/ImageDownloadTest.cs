using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CrawlerXKW
{
    public class ImageDownloadTest
    {
        public string rootPath = @"D:\xkw_image";
        public void test()
        {
            Down("http://static.zujuan.xkw.com/Upload/2017-10/16/11d770f6-0d24-4932-9462-68acc546bb14/paper.files/image216.png");
        }


        public void Down(string href)
        {
            if (href.IndexOf("http", StringComparison.OrdinalIgnoreCase) < 0)
                return;
            var loalPath = rootPath;
            var segments = new Uri(href).Segments;
            foreach (var seg in segments)
            {
                var seg2 = seg.Replace("/", "").Replace("//", "");
                if (!string.IsNullOrWhiteSpace(seg2)/* && seg2.IndexOf(".png", StringComparison.OrdinalIgnoreCase) < 0 && seg2.IndexOf(".jpg", StringComparison.OrdinalIgnoreCase) < 0 && seg2.IndexOf(".gif", StringComparison.OrdinalIgnoreCase) < 0 && seg2.IndexOf(".bmp", StringComparison.OrdinalIgnoreCase) < 0*/)
                {
                    loalPath = Path.Combine(loalPath, seg2);
                }
            }

            Debug.WriteLine(loalPath);
            ExecuteDownload(href, loalPath);
        }

        public bool ExecuteDownload(string url, string localfile)
        {
            int retry = 3;
            for (var i = 0; i < retry; i++)
            {
                bool hasException = false;
                try
                {
                    Download(url, localfile);

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("error download image" + url);
                    hasException = true;
                }
                if (hasException)
                {
                    Task.Delay(1000);
                }
            }
            throw new Exception("error download image" + url);
            return false;
        }

        /// <summary>
        /// Http方式下载文件
        /// </summary>
        /// <param name="url">http地址</param>
        /// <param name="localfile">本地文件</param>
        /// <returns></returns>
        public void Download(string url, string localfile)
        {
            var directoryInfo = new FileInfo(localfile).Directory;
            if (directoryInfo != null && !directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
            if (File.Exists(localfile))
            {
                return;
            }
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Headers["cookie"] = "UM_distinctid=166e9518cbe298-01a36d572eaa51-43480420-144000-166e9518cbf41c; Hm_lvt_68fb48a14b4fce9d823df8a437386f93=1542198949,1542388644,1542451811,1542468807; Hm_lpvt_68fb48a14b4fce9d823df8a437386f93=1542468807; cn_5816665539db22708e01_dplus=%7B%22distinct_id%22%3A%20%22166e9518cbe298-01a36d572eaa51-43480420-144000-166e9518cbf41c%22%2C%22sp%22%3A%20%7B%22%24_sessionid%22%3A%200%2C%22%24_sessionTime%22%3A%201542452058%2C%22%24dp%22%3A%200%2C%22%24_sessionPVTime%22%3A%201542452058%2C%22%24recent_outside_referrer%22%3A%20%22%24direct%22%7D%2C%22initial_view_time%22%3A%20%221541513972%22%2C%22initial_referrer%22%3A%20%22http%3A%2F%2Fzujuan.xkw.com%2Fgzsx%2Fzsd27929%2Fpt3%2F%22%2C%22initial_referrer_domain%22%3A%20%22zujuan.xkw.com%22%7D";
                request.Headers["Accept-Language"] = "zh-CN,zh;q=0.9";
                //request.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                request.UserAgent =
                    "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3440.106 Safari/537.36";
                var response = request.GetResponse();
                var stream = response.GetResponseStream();
                Image _image = Image.FromStream(stream);
                _image.Save(localfile);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
