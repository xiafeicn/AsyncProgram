using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using NSoup;

namespace CrawlerXKW
{
    public class HtmlTest
    {
        public void TestParseQuestion()
        {
            var html = File.ReadAllText(@"D:\html.txt");
            AddQuestion(html, 1, "", "", 0, 0, 0, 0);
        }
        public List<QuestionXkw> AddQuestion(string html, int jiaocaiId, string subjectId, string sourceUrl, int areaId, int sourceId, int total, int pageNum)
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


                    //var paths = new ParseQuestionXkw().SaveAnswerImage(entity.QuestionId.ToString(), entity.key, subjectId);
                    //entity.AnalysisImg = paths[0];
                    //entity.AnswerImg = paths[1];
                    entity.QuestionAnalysis = analysisUrl;
                    entity.QuestionAnswer = answerUrl;
                    entity.CreateTime = DateTime.Now;
                    result.Add(entity);
                }
               
                catch (Exception ex)
                {
                    throw ex;

                }
            }
            return result;
        }
    }
}
