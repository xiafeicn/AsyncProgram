//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Common
{
    using System;
    using System.Collections.Generic;
    
    public partial class QuestionAll
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string AnswerJson { get; set; }
        public string ApiJson { get; set; }
        public string CrawlerUrl { get; set; }
        public string CrawlerApiUrl { get; set; }
        public Nullable<int> xd { get; set; }
        public Nullable<int> child { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public bool IsGrabAns { get; set; }
        public bool IsRemoteDelete { get; set; }
    }
}
