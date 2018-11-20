using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CrawlerXKW
{
    public class ExportTest
    {
        public void export()
        {
            var list = new List<V_questionxkw>();

            using (var db = new XKWEntities2())
            {

                list =
                    db.V_questionxkw.Take(1000).ToList();
            }

            foreach (var item in list)
            {
                CopyTo(item.AnswerImg);
                CopyTo(item.AnalysisImg);

                //item.Knowledge =
                //    item.categories.Split(new string[] {"###"}, StringSplitOptions.RemoveEmptyEntries).ToList();
            }

            var str = JsonConvert.SerializeObject(list);
            File.WriteAllText(@"D:\output\test.json", str);
        }

        public void CopyTo(string src)
        {
            var dest = src.Replace(@"D:\xkw_image", @"D:\output");
            if (File.Exists(dest)) return;
            FileInfo fi=new FileInfo(dest);
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }
            File.Copy(src, dest);
        }
    }
}
