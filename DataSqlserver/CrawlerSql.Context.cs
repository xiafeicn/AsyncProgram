﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CrawlerSqlEntities : DbContext
    {
        public CrawlerSqlEntities()
            : base("name=CrawlerSqlEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Book> Book { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<CategoryTree> CategoryTree { get; set; }
        public virtual DbSet<CategoryUrlList> CategoryUrlList { get; set; }
        public virtual DbSet<CategoryUrlListQuestion> CategoryUrlListQuestion { get; set; }
        public virtual DbSet<Question> Question { get; set; }
        public virtual DbSet<V_ALL_CategoryUrlList> V_ALL_CategoryUrlList { get; set; }
        public virtual DbSet<V_ALL_CrawlerQuestion> V_ALL_CrawlerQuestion { get; set; }
        public virtual DbSet<V_AllCategory> V_AllCategory { get; set; }
        public virtual DbSet<V_CategoryTree> V_CategoryTree { get; set; }
    }
}
