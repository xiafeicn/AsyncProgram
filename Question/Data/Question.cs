//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Grabzujuan.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Question
    {
        public int Id { get; set; }
        public int Question_Id { get; set; }
        public string QuestionText { get; set; }
        public string QuestionHtml { get; set; }
        public int CategoryId { get; set; }
        public int BookId { get; set; }
        public int Child { get; set; }
        public int Degree { get; set; }
        public int question_channel_type { get; set; }
        public int difficult_index { get; set; }
        public int exam_type { get; set; }
        public int kid_num { get; set; }
        public int grade_id { get; set; }
        public string answer { get; set; }
        public Nullable<int> PaperId { get; set; }
        public string PaperUrl { get; set; }
        public string CrawlerUrl { get; set; }
        public int CategoryUrlListId { get; set; }
        public string ApiJson { get; set; }
        public string AnswerJson { get; set; }
        public string Knowledge { get; set; }
        public string QuestionFrom { get; set; }
        public string QuestionSource { get; set; }
        public string Score { get; set; }
        public bool IsGrabAns { get; set; }
    }
}
