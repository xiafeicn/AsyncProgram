﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CrawlerEntities : DbContext
    {
        public CrawlerEntities()
            : base("name=CrawlerEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Book> Book { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<CategoryUrlList> CategoryUrlList { get; set; }
        public virtual DbSet<difficult_index> difficult_index { get; set; }
        public virtual DbSet<exam_type> exam_type { get; set; }
        public virtual DbSet<Paper> Paper { get; set; }
        public virtual DbSet<Subject> Subject { get; set; }
        public virtual DbSet<kid_num> kid_num { get; set; }
        public virtual DbSet<V_ALL_Question> V_ALL_Question { get; set; }
        public virtual DbSet<V_AllCategory> V_AllCategory { get; set; }
        public virtual DbSet<Proxy> Proxy { get; set; }
        public virtual DbSet<Question> Question { get; set; }
    }
}