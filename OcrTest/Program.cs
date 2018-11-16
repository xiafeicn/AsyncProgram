using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Aspose.OCR;

namespace OcrTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var engine = new Aspose.OCR.OcrEngine();
            //Image _image = Image.FromStream(/* ?? throw new InvalidOperationException()*/);

            engine.Image = ImageStream.FromFile(@"D:\cb035707ba69886349f3b0b6bc4e_1573200kn.png");


            engine.Process();
            var a = engine.Text;
        }

    }
}
