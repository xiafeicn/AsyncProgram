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

namespace GrabTree
{
    class Program
    {
        static void Main(string[] args)
        {
            GrabTopCate();


            var listCategory = GetTopTreeList();

            foreach (var cate in listCategory)
            {
                Recrusion(cate.CategoryId,cate.TreeId);
            }
        }


        public static void GrabTopCate()
        {
            var listCategory = DataService.GetCategorylist();
            foreach (var cate in listCategory)
            {
                if (CateExist(cate.CategoryId))
                    continue;
                var url =
                    $"https://www.zujuan.com/question?categories={cate.CategoryId}&bookversion={cate.BookVersionId}&nianji={cate.CategoryId}&chid={cate.Child}&xd={cate.Degree}";
                var result = HttpClientHolder.GetRequest(url);
                //var res = new HttpUnitHelper().GetRealHtmlOnce(url);
                var doc = NSoupClient.Parse(result);

                var topItems = doc.Select("#J_Tree div a");

                foreach (var item in topItems)
                {
                    var href = item.Attr("href");
                    var id = GetQueryString("categories", href).NullToInt();
                    var name = item.Text();
                    var pid = cate.CategoryId;

                    AddTree(cate.CategoryId, name, pid, id);
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
        public static List<CategoryTree> GetTopTreeList()
        {
            using (var db = new CrawlerEntities())
            {
                return db.CategoryTree.ToList();
            }
        }
        public static void AddTree(int categoryId, string name, int pid, int id)
        {

            using (var db = new CrawlerEntities())
            {


                CategoryTree entity = new CategoryTree();
                entity.CategoryId = categoryId;
                entity.TreeName = name;
                entity.ParentTreeId = pid;
                entity.TreeId = id;

                db.CategoryTree.Add(entity);
                db.SaveChanges();
            }
        }


        public static void Recrusion(int categoryid, int pid)
        {

            var api = $"https://www.zujuan.com/question/tree?id={pid}&type=category";
            var res = HttpClientHolder.GetRequest(api);
            JArray array = JArray.Parse(res);
            foreach (var item in array)
            {
                var id = item["id"].NullToInt();
                var name = item["title"].ToString();

                AddTree(categoryid, name, pid, id);

                if (item["hasChild"].ToString().ToBool() == true)
                {
                    Recrusion(categoryid, id);
                }
            }
        }
    }
}
