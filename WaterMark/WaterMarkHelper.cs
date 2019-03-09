using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMark
{
    public class WaterMarkHelper
    {
        public void ProcessWaterMark(string imagePath)
        {
            //var imagePath = @"C:\Users\fei.xia\Documents\Tencent Files\312166604\FileRecv\MobileFile\output\Analysis\2131182\Analysis.png";
            var b = Bitmap.FromFile(imagePath).Clone() as Bitmap;
            int h = b.Height;
            int w = b.Width;
            for (int i = 1; i < w; i++)
            {
                for (int j = 1; j < h; j++)
                {
                    Color c = b.GetPixel(i, j);//根据像素获取颜色
                    if (c.R>240&&c.G>240&&c.B>240/* == Color.FromArgb(242,242, 242)|| c == Color.FromArgb(243, 243, 243) || c == Color.FromArgb(244, 244, 244) || c == Color.FromArgb(245, 245, 245) || c == Color.FromArgb(246, 246, 246) || c == Color.FromArgb(247, 247, 247) || c == Color.FromArgb(248, 248, 248)*/)
                    {
                        b.SetPixel(i, j, Color.White);//根据像素设置颜色
                    }
                }
            }
            b.Save("D:\\a.png");

        }
    }
}
