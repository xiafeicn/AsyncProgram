using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Baidu.Aip.Ocr;

namespace OCR
{
    class Program
    {
        // 设置APPID/AK/SK
        static string APP_ID = "14651186";
        static string API_KEY = "WA4eQQhtY6qMWaGeX11V49oT";
        static string SECRET_KEY = "Ld9iDkS7cAl9ypcxCfRyAwGc0QGBCWP8";

        static Ocr client = new Baidu.Aip.Ocr.Ocr(API_KEY, SECRET_KEY);

        static void Main(string[] args)
        {
            client.Timeout = 60000;  // 修改超时时间

            GeneralBasicUrlDemo();

            Console.ReadKey();
        }

        static public void GeneralBasicDemo()
        {
            var image = File.ReadAllBytes(@"C:\Users\Administrator.ZV248HJXEANVUIN\Desktop\QQ图片20181104123935.jpg");
            // 调用通用文字识别, 图片参数为本地图片，可能会抛出网络等异常，请使用try/catch捕获
            var result = client.GeneralBasic(image);
            Console.WriteLine(result);
            // 如果有可选参数
            var options = new Dictionary<string, object>{
                {"language_type", "CHN_ENG"},
                {"detect_direction", "true"},
                {"detect_language", "true"},
                {"probability", "true"}
            };
            // 带参数调用通用文字识别, 图片参数为本地图片
            result = client.GeneralBasic(image, options);
            Console.WriteLine(result);
        }
        static public void GeneralBasicUrlDemo()
        {
            var url = "http://im.zujuan.xkw.com/Parse/7121096/11/700/14/28/7e27af2fa1a1878115058598985874ca";

            // 调用通用文字识别, 图片参数为远程url图片，可能会抛出网络等异常，请使用try/catch捕获
            var result = client.GeneralBasicUrl(url);
            Console.WriteLine(result);
            // 如果有可选参数
            var options = new Dictionary<string, object>{
                {"language_type", "CHN_ENG"},
                {"detect_direction", "true"},
                {"detect_language", "true"},
                {"probability", "true"}
            };
            // 带参数调用通用文字识别, 图片参数为远程url图片
            result = client.GeneralBasicUrl(url, options);
            Console.WriteLine(result);
        }
    }
}
