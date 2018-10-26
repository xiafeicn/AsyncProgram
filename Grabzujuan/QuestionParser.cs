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
                    exam_q = ProcessHtmlImageElement(exam_q);


                }
                obj["question"] = exam_q.Html();
            }

            if (li.GetElementsByClass("exam-qlist").Count > 0)
            {
                var qList = li.GetElementsByClass("exam-qlist")[0];
                var qArray = new JArray();
                foreach (var q in qList.GetElementsByClass("exam-q"))
                {
                    var cq = ProcessHtmlImageElement(q);
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
                    var currentAns = ProcessHtmlImageElement(ans);
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
            string url = string.Format("https://www.zujuan.com/question/detail-{0}.shtml", questionId);
            var html = new HttpUnitHelper().GetRealHtmlOnce(url);
            var qlist = NSoupClient.Parse(html).GetElementById("J_QuestionList");

            JObject answer = new JObject();
            answer["kaodian"] = ProcessHtmlImageElement(qlist.GetElementsByClass("analyticbox")[0].GetElementsByTag("div")[0]).Html();
            answer["daan"] = ProcessHtmlImageElement(qlist.GetElementsByClass("analyticbox")[1].GetElementsByTag("div")[0]).Html();
            answer["jiexi"] = ProcessHtmlImageElement(qlist.GetElementsByClass("analyticbox")[2].GetElementsByTag("div")[0]).Html();

            return answer;
        }


        public Element ProcessHtmlImageElement(Element element)
        {
            var mathImages = element.GetElementsByClass("mathml");

            var allImages= element.GetElementsByTag("img").Except(mathImages);

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

        public string ConvertHttpImageToBase64(string imgUrl)
        {
            Image _image = Image.FromStream(System.Net.WebRequest.Create(imgUrl).GetResponse().GetResponseStream() ?? throw new InvalidOperationException());
            return ImgToBase64String(_image);
        }

        /// <summary>
        /// 图片 转为    base64编码的文本
        /// </summary>
        /// <param name="bmp">待转的Bitmap</param>
        /// <returns>转换后的base64字符串</returns>
        public String ImgToBase64String(Image bmp)
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
        public byte[] convertByte(Image img)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, img.RawFormat);
            byte[] bytes = ms.ToArray();
            ms.Close();
            return bytes;
        }
    }
}
