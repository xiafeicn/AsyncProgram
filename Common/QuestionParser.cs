using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;
using NSoup;
using NSoup.Nodes;

namespace Grabzujuan
{
    public class QuestionParser
    {
        public JArray ParseMath(string html)
        {
            var qlist = NSoupClient.Parse(html).GetElementById("J_QuestionList");

            var lis = qlist.GetElementsByTag("li");

            var jArray = new JArray();

            //<input type="hidden" name="categories" value="106064">
            foreach (var li in lis)
            {
                jArray.Add(ParseQuestion(li));
            }

            return jArray;
        }

        public JObject ParseQuestion(Element li)
        {
            JObject obj = new JObject();
           
            var questionId = li.Attr("data-qid");
            obj["id"] = questionId;
            if (li.GetElementsByClass("exam-q").Count > 0)
            {
                var exam_q = li.GetElementsByClass("exam-q")[0];
                if (exam_q.GetElementsByTag("img").Count > 0)
                {
                    exam_q = exam_q.ProcessHtmlImageElement();


                }
                obj["question"] = exam_q.Html();
            }

            if (li.GetElementsByClass("exam-qlist").Count > 0)
            {
                var qList = li.GetElementsByClass("exam-qlist")[0];
                var qArray = new JArray();
                foreach (var q in qList.GetElementsByClass("exam-q"))
                {
                    var cq = q.ProcessHtmlImageElement();
                    var item = new JObject();
                    item["question_item"] = cq.Html();

                    qArray.Add(item);
                }

                obj["question_list"] = qArray;
            }

            if (li.GetElementsByClass("exam-s").Count > 0)
            {
                var opItems = li.GetElementsByClass("exam-s")[0].GetElementsByClass("op-item");
                var opArray = new JArray();
                foreach (var ans in opItems)
                {
                    var item = new JObject();
                    item["out"] = ans.GetElementsByClass("op-item-nut").Text;
                    var currentAns = ans.ProcessHtmlImageElement();
                    if (ans.GetElementsByTag("img").Count > 0)
                    {
                        item["meat"] = currentAns.GetElementsByTag("img").Attr("src");
                    }
                    else
                    {
                        item["meat"] = currentAns.GetElementsByClass("op-item-meat").Text;

                    }


                    opArray.Add(item);
                }
                obj["option"] = opArray;
            }



            obj["type"] = li.GetElementsByClass("exam-new").Text;

            var type = li.GetElementsByClass("exam-head-left")[0].GetElementsByTag("span");


            obj["tixing"] = type[0].Text();
            obj["tilei"] = type[1].Text();
            obj["nanyidu"] = type[2].Text();


            obj["answer"] = ParseAnswer(questionId);
            return obj;
        }

        public JObject ParseAnswer(string questionId)
        {
           // questionId = "8630746";
            JObject answer = new JObject();
            string url = string.Format("https://www.zujuan.com/question/detail-{0}.shtml", questionId);
            var html = new HttpUnitHelper().GetRealHtmlOnce(url);

            if (
                NSoupClient.Parse(html).GetElementById("J_QuestionList").GetElementsByClass("exam-con")[0]
                    .GetElementsByClass("exam-qlist").Count > 0)
            {
                var qlist = NSoupClient.Parse(html).GetElementById("J_QuestionList").GetElementsByClass("exam-con")[0].GetElementsByClass("exam-qlist")[0];

                var exam_cons = qlist.GetElementsByClass("exam-con");

                var exam_cons_answers = qlist.Select("div.analyticbox.replace_anawer");
                
                JArray answer_list = new JArray();
                for (int i = 0; i < exam_cons.Count; i++)
                {
                    JObject result = new JObject();
                    result["question"] = exam_cons[i].GetElementsByClass("analyticbox")[0].ProcessHtmlImageElement().Html();
                    result["answer"] = exam_cons_answers[i].ProcessHtmlImageElement().Html();
                    answer_list.Add(result);
                }
                answer["answer_list"] = answer_list;
            }
          

            var analyticbox_brick = NSoupClient.Parse(html).GetElementById("J_QuestionList").GetElementsByClass("analyticbox-brick")[0];


            answer["kaodian"] = analyticbox_brick.Children[0].ProcessHtmlImageElement().Html();// ProcessHtmlImageElement(qlist.GetElementsByClass("analyticbox")[0].GetElementsByTag("div")[0]).Html();
            answer["jiexi"] = analyticbox_brick.Children[1].ProcessHtmlImageElement().Html();

            return answer;
        }


      
    }


    public static class Base64Helper
    {

        public static Element ProcessHtmlImageElement(this Element element)
        {
            var mathImages = element.GetElementsByClass("mathml");

            var allImages = element.GetElementsByTag("img").Except(mathImages);

            if (mathImages.Count > 0)
            {
                foreach (var math in mathImages)
                {

                    math.Attr("src", HttpUtility.UrlDecode(math.Attr("src")));


                };
            }

            if (allImages.Any())
            {
                foreach (var math in allImages)
                {
                    var imgUrl = math.Attr("src");
                    var base64 = ConvertHttpImageToBase64(imgUrl);
                    var newImageUrl = "data:image/png;base64," + base64;

                    math.Attr("src", newImageUrl.Trim(new char[] { '"' }));


                }
            }
            //if (element.GetElementsByAttribute("data-cke-saved-src").Count > 0)
            //{
            //    //连接性的图片
            //    foreach (var math in element.GetElementsByAttribute("data-cke-saved-src"))
            //    {
            //        var imgUrl = math.Attr("src");
            //        var base64 = ConvertHttpImageToBase64(imgUrl);
            //        var newImageUrl = "data:image/png;base64," + base64;

            //        math.Attr("src", newImageUrl.Trim(new char[] { '"' }));


            //    }

            //}

            //if (element.GetElementsByClass("mathml").Count > 0)
            //{
            //    foreach (var math in element.GetElementsByClass("mathml"))
            //    {

            //        math.Attr("src", HttpUtility.UrlDecode(math.Attr("src")));


            //    };
            //}
            return element;
        }

        public static string ConvertHttpImageToBase64(string imgUrl)
        {
            Image _image = Image.FromStream(System.Net.WebRequest.Create(imgUrl).GetResponse().GetResponseStream()/* ?? throw new InvalidOperationException()*/);
            return ImgToBase64String(_image);
        }

        /// <summary>
        /// 图片 转为    base64编码的文本
        /// </summary>
        /// <param name="bmp">待转的Bitmap</param>
        /// <returns>转换后的base64字符串</returns>
        public static String ImgToBase64String(Image bmp)
        {
            String strbaser64 = String.Empty;
            var btarr = convertByte(bmp);
            strbaser64 = Convert.ToBase64String(btarr);

            return strbaser64;
        }

        /// <summary>
        /// Image转byte[]
        /// </summary>
        /// <param name="img">Img格式数据</param>
        /// <returns>byte[]格式数据</returns>
        public static byte[] convertByte(Image img)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, img.RawFormat);
            byte[] bytes = ms.ToArray();
            ms.Close();
            return bytes;
        }
    }
}
