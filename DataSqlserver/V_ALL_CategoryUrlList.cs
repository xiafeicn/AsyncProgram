//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataSqlserver
{
    using System;
    using System.Collections.Generic;
    
    public partial class V_ALL_CategoryUrlList
    {
        public int Id { get; set; }
        public string GrabUrl { get; set; }
        public int CategoryId { get; set; }
        public int PageNum { get; set; }
        public string ApiUrl { get; set; }
        public string ApiJson { get; set; }
        public bool Status { get; set; }
        public int BookVersionId { get; set; }
        public string BookName { get; set; }
        public int Degree { get; set; }
        public int Child { get; set; }
        public int BookId { get; set; }
        public string CategoryName { get; set; }
    }
}