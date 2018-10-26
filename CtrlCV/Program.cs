using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace CtrlCV
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "config.txt"))
            {
                throw new FileNotFoundException("config.txt not found!");
            }
            var configs = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "config.txt");
            if (configs.Length < 1)
            {
                throw new ArgumentException("config.txt 配置错误!");
            }

            var watchDir = configs[0];
            if (!Directory.Exists(watchDir))
            {
                throw new FileNotFoundException(watchDir + "not found!");
            }

            copy(watchDir);


        }

        private static void copy(string dir)
        {
            var files = new DirectoryInfo(dir).GetFiles();
            if (files.Length < 1)
            {
                throw new Exception("当前目录没有要复制的文件");
            }

            foreach (var file in files)
            {
                if (file.Extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase) ||
                    file.Extension.Equals(".png", StringComparison.OrdinalIgnoreCase))
                {
                    string filePath = file.FullName;
                    if (File.Exists(filePath))
                    {
                        //System.Collections.Specialized.StringCollection strcoll = new System.Collections.Specialized.StringCollection();

                        //strcoll.Add(filePath);


                        //strcoll.Add(dirPath);
                        //Clipboard.SetFileDropList(strcoll);

                        Image img = Image.FromFile(filePath);
                        Clipboard.SetImage(img);
                    }
                    break;
                }
            }
            
        }
    }
}
