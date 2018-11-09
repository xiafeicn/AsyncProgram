using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Common;
using Grabzujuan;
using Grabzujuan.Common;
using Newtonsoft.Json.Linq;
using NHtmlUnit.Html;
using NHtmlUnit.Javascript.Host.Html;
using NSoup;

namespace KnowledgeTree
{
    class Program
    {
        static void Main(string[] args)
        {
            //GrabTopKnowledge();


            var listKnowledge = GetTopTreeList();

            foreach (var cate in listKnowledge)
            {
                Recrusion( cate.TreeId);
            }
        }


        public static void GrabTopKnowledge()
        {
            foreach (var chid in typeof(ChildEnum).GetEnumSource())
            {
                foreach (var xd in typeof(DegreeEnum).GetEnumSource())
                {
                    //if (CateExist(cate.CategoryId))
                    //    continue;
                    var url =
                        $"https://www.zujuan.com/question?chid={chid.Item1}&xd={xd.Item1}&tree_type=knowledge";
                    //var result = HttpClientHolder.GetRequest(url);
                    var res = new HttpUnitHelper().GetRealHtmlTrice(url);
                    var doc = NSoupClient.Parse(res);

                    var topItems = doc.Select("#J_Tree ul li");

                    foreach (var item in topItems)
                    {
                        var id = item.Attr("data-treeid").NullToInt();
                        var name = item.GetElementsByTag("em")[0].Text();
                        var pid = 0;

                        AddTree(name, pid, id, url);
                    }
                }

            }


        }

        /// <summary>
        /// 获取url字符串参数，返回参数值字符串
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="url">url字符串</param>
        /// <returns></returns>
        public static string GetQueryString(string name, string url)
        {
            System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(@"(^|&)?(\w+)=([^&]+)(&|$)?", System.Text.RegularExpressions.RegexOptions.Compiled);
            System.Text.RegularExpressions.MatchCollection mc = re.Matches(url);
            foreach (System.Text.RegularExpressions.Match m in mc)
            {
                if (m.Result("$2").Equals(name))
                {
                    return m.Result("$3");
                }
            }
            return "";
        }

        public static bool CateExist(int categoryId)
        {
            using (var db = new CrawlerEntities())
            {
                return db.CategoryTree.Any(t => t.CategoryId == categoryId);
            }
        }
        public static List<Common.KnowledgeTree> GetTopTreeList()
        {
            using (var db = new CrawlerEntities())
            {
                return db.KnowledgeTree.ToList();
            }
        }
        public static void AddTree(string name, int pid, int id, string url)
        {

            using (var db = new CrawlerEntities())
            {
                Common.KnowledgeTree entity = new Common.KnowledgeTree();


                entity.TreeName = name;
                entity.ParentTreeId = pid;
                entity.TreeId = id;
                entity.Url = url;
                db.KnowledgeTree.Add(entity);
                db.SaveChanges();
            }
        }


        public static void Recrusion(int pid)
        {
            var api = $"https://www.zujuan.com/question/tree?id={pid}&type=knowledge";
            var res = HttpClientHolder.GetRequest(api);
            JArray array = JArray.Parse(res);
            foreach (var item in array)
            {
                var id = item["id"].NullToInt();
                var name = item["title"].ToString();

                AddTree(name, pid, id, api);

                if (item["hasChild"].ToString().ToBool() == true)
                {
                    Recrusion( id);
                }
            }
        }
    }
}
