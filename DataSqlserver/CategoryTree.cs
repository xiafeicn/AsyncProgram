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
    
    public partial class CategoryTree
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int TreeId { get; set; }
        public int ParentTreeId { get; set; }
        public string TreeName { get; set; }
        public Nullable<int> Tempid { get; set; }
    }
}
