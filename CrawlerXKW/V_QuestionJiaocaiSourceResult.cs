//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace CrawlerXKW
{
    using System;
    using System.Collections.Generic;
    
    public partial class V_QuestionJiaocaiSourceResult
    {
        public int Id { get; set; }
        public int JiaocaiId { get; set; }
        public int AreaId { get; set; }
        public int Total { get; set; }
        public int PageNum { get; set; }
        public string Html { get; set; }
        public string CrawlerUrl { get; set; }
        public int QuestionJiaoCaiSourceId { get; set; }
        public bool Status { get; set; }
        public Nullable<int> SubjectId { get; set; }
        public string SubjectName { get; set; }
    }
}