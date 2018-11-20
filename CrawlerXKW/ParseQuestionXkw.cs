using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Common;
using NSoup;
using NSoup.Select;

namespace CrawlerXKW
{
    public class ParseQuestionXkw
    {

        public string rootPath = @"D:\xkw_image";
        //public void Start()
        //{
        //    while (ExistQuestionJiaocaiSourceResult())
        //    {
        //        var listResult = GetRandom10QuestionJiaocaiSourceResult();
        //        foreach (var item in listResult)
        //        {
        //            var doc = NSoupClient.Parse(item.Html);
        //            var imgs = doc.GetElementsByTag("img");
        //            SaveImage(imgs);
        //            SaveAnswerImage(item.Html, item);
        //            AddQuestion(item.Html, item.QuestionJiaoCaiSourceId, item.JiaocaiId, item.CrawlerUrl);
        //        }
        //    }
        //}

        public void SaveImage(Elements elements)
        {
            Parallel.ForEach(elements, (element) =>
              {
                  var href = element.Attr("src").NullToString();
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
              });
        }

        public List<string> SaveAnswerImage(string questionId, string key, string subjectId)
        {
            List<string> result = new List<string>();
            var url = $"http://im.zujuan.xkw.com/Parse/{questionId}/{subjectId}/700/14/28/{key}";
            string localFile = Path.Combine(rootPath, "Analysis", (questionId.NullToInt() / 1000).ToString(), questionId, "Analysis.png");
            ExecuteDownload(url, localFile);
            result.Add(localFile);
            var urlAnswer = $"http://im.zujuan.xkw.com/Answer/{questionId}/{subjectId}/700/14/28/{key}";
            string localAnswerFile = Path.Combine(rootPath, "Answer", (questionId.NullToInt() / 1000).ToString(), questionId, "Answer.png");
            ExecuteDownload(urlAnswer, localAnswerFile);
            result.Add(localAnswerFile);
            return result;
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

        public void TestDown()
        {
            ExecuteDownload(
                " http://static.zujuan.xkw.com/Upload/2018-08/08/c56e28df-d8d5-4050-a8af-e7fc8eab3939/paper.files/image001.png",
                @"D:\xkw_image\Upload\2018-08\08\c56e28df-d8d5-4050-a8af-e7fc8eab3939\paper.files\image001.png");
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

        public bool ExistQuestionJiaocaiSourceResult()
        {
            using (var db = new XKWEntities2())
            {
                return
                    db.QuestionJiaocaiSourceResult.Any(t => t.Status == false);
            }
        }
        public List<V_QuestionJiaocaiSourceResult> GetRandom10QuestionJiaocaiSourceResult()
        {
            using (var db = new XKWEntities2())
            {

                return
                    db.Database.SqlQuery<V_QuestionJiaocaiSourceResult>(
                        @"
select top 10 *, NewID() as random from [V_QuestionJiaocaiSourceResult] where status=0 ").ToList();
            }
        }
        public List<QuestionXkw> AddQuestion(string html,string questionHtml, int jiaocaiId, string subjectId, string sourceUrl, int areaId, int sourceId, int total, int pageNum)
        {
            List<QuestionXkw> result = new List<QuestionXkw>();
            var elements = NSoupClient.Parse(html).GetElementsByClass("quesbox");
            foreach (var element in elements)
            {
                try
                {

                    QuestionXkw entity = new QuestionXkw();
                    entity.OriginHtml = element.Html();
                    var detail = element.Select("div.join-sj>a")[0];
                    entity.QuestionId = detail.Attr("quesid").NullToInt();
                    entity.@class = detail.Attr("class").NullToString();
                    entity.guid = detail.Attr("guid").NullToString();
                    entity.childnum = detail.Attr("childnum").NullToInt();
                    entity.questitle = detail.Attr("questitle").NullToString();
                    entity.categories = detail.Attr("categories").NullToString();
                    entity.qyid = detail.Attr("qyid").NullToInt();
                    entity.qdid = detail.Attr("qdid").NullToInt();
                    entity.qyname = detail.Attr("qyname").NullToString();
                    entity.qdname = detail.Attr("qdname").NullToString();
                    entity.JiaocaiId = jiaocaiId;
                    var source = element.Select("div.quesource")[0];

                    entity.source = source.Html();
                    entity.SourceUrl = sourceUrl;

                    var questiontitle = element.Select("div.question-inner")[0];
                    entity.key = questiontitle.Attr("key").NullToString();
                    entity.question_text = questiontitle.Html();

                    var href = element.Select("a.detail")[0];
                    entity.CrawlerUrl = href.Attr("href").NullToString();


                    var str =
                        entity.CrawlerUrl.Replace("http://zujuan.xkw.com/", "")
                            .Replace("https://zujuan.xkw.com/", "");
                    var bankId = str.Substring(0, str.IndexOf("q", StringComparison.OrdinalIgnoreCase));
                    ;
                    var analysisUrl =
                        $"http://im.zujuan.xkw.com/Parse/{entity.QuestionId}/{bankId}/700/14/28/{entity.key}";
                    var answerUrl =
                        $"http://im.zujuan.xkw.com/Answer/{entity.QuestionId}/{bankId}/700/14/28/{entity.key}";


                    var paths = new ParseQuestionXkw().SaveAnswerImage(entity.QuestionId.ToString(), entity.key, subjectId);
                    entity.AnalysisImg = paths[0];
                    entity.AnswerImg = paths[1];
                    entity.QuestionAnalysis = analysisUrl;
                    entity.QuestionAnswer = answerUrl;
                    entity.CreateTime = DateTime.Now;
                    result.Add(entity);
                }
                //catch (DbUpdateException exception)
                //{
                //    var msg = string.Empty;

                //    foreach (var validationError in ((DbUpdateException)exception).Data)
                //    {
                //        var o = validationError;
                //    }
                //    throw new Exception();
                //}
                //catch (DbEntityValidationException ex)
                //{
                //    var msg = string.Empty;

                //    foreach (var validationError in ((DbEntityValidationException)ex).EntityValidationErrors)
                //        foreach (var error in validationError.ValidationErrors)
                //            msg += string.Format("Property: {0} Error: {1}", error.PropertyName, error.ErrorMessage);

                //    var fail = new Exception(msg);
                //    throw fail;
                //}
                catch (Exception ex)
                {
                    WriteLog(questionHtml, ex.ToString(),sourceUrl);

                    throw ex;

                }
            }

            try
            {
                using (var db = new XKWEntities2())
                {
                    db.QuestionXkw.AddRange(result);

                    if (
                        !db.QuestionJiaocaiSourceResult.Any(
                            t => t.JiaocaiId == jiaocaiId && t.AreaId == areaId && t.PageNum == pageNum))
                    {
                        var entity = new QuestionJiaocaiSourceResult();
                        entity.AreaId = areaId;
                        entity.JiaocaiId = jiaocaiId;
                        entity.Html = html;
                        entity.Total = total;
                        entity.PageNum = pageNum;
                        entity.QuestionJiaoCaiSourceId = sourceId;
                        entity.CrawlerUrl = sourceUrl;
                        db.QuestionJiaocaiSourceResult.Add(entity);
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                WriteLog(questionHtml, ex.ToString(),sourceUrl);

                throw ex;
            }
            return result;
        }


        public void WriteLog(string error, string error2,string url)
        {
            using (var db = new XKWEntities2())
            {
                var log = new Log();
                log.Content = error;
                log.Content2 = error2;
                log.CrawlerUrl = url;
                db.Log.Add(log);
                db.SaveChanges();
            }
        }

    }
}
