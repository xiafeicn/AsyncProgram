using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Common;
using NSoup;

namespace CrawlerXKW
{
    public class QuestionJiaoCaiDetailSourceCrawler
    {

        public void StartCrawler()
        {
            var list = GetHugeList();
            Parallel.ForEach(list, new ParallelOptions() { MaxDegreeOfParallelism = 2 }, (item) =>
               {
                   var listJcDetail = GetJiaocaiDetails(item.JiaocaiId);
                   foreach (var jcdetail in listJcDetail)
                   {
                       var url = $"http://zujuan.xkw.com/{item.Prefix}/zj{jcdetail.JiaoCaiDetailId}/a{item.AreaId}/";
                       var html = HttpWebResponseUtility.ExecuteCreateGetHttpResponse(url, 50000, null);

                       var doc = NSoupClient.Parse(html);
                       var totalCount = doc.GetElementById("questioncount").Text().NullToInt();
                       AddJiaocaiDetaiSource(item.AreaId, item.JiaocaiId, jcdetail.JiaoCaiDetailId, totalCount, url);
                   }
               });
        }


        public void AddJiaocaiDetaiSource(int areaId, int jiaocaiId, int jiaocaiDetaiId, int total, string url)
        {
            using (var db = new XKWEntities2())
            {
                if (db.QuestionJiaoCaiDetailSource.Any(t => t.JiaocaiDetailId == jiaocaiDetaiId && t.AreaId == areaId))
                    return;


                var entity = new QuestionJiaoCaiDetailSource();
                entity.AreaId = areaId;
                entity.JiaocaiId = jiaocaiId;
                entity.JiaocaiDetailId = jiaocaiDetaiId;
                entity.Total = total;
                entity.CrawlerUrl = url;
                db.QuestionJiaoCaiDetailSource.Add(entity);
                db.SaveChanges();
            }
        }

        public List<V_QuestionJiaoCaiSource> GetHugeList()
        {
            using (var db = new XKWEntities2())
            {
                return db.V_QuestionJiaoCaiSource.Where(t => t.Total > 12000).ToList();
            }
        }

        public List<JiaocaiDetail> GetJiaocaiDetails(int jiaocaiID)
        {
            using (var db = new XKWEntities2())
            {
                return db.JiaocaiDetail.Where(t => t.JiaoCaiDetailParentId == jiaocaiID).ToList();
            }
        }
    }
}
