﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class XKWEntities2 : DbContext
    {
        public XKWEntities2()
            : base("name=XKWEntities2")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Area> Area { get; set; }
        public virtual DbSet<Degree> Degree { get; set; }
        public virtual DbSet<Grade> Grade { get; set; }
        public virtual DbSet<JiaoCai> JiaoCai { get; set; }
        public virtual DbSet<JiaocaiDetail> JiaocaiDetail { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<QuestionJiaoCaiSource> QuestionJiaoCaiSource { get; set; }
        public virtual DbSet<QuestionJiaocaiSourceResult> QuestionJiaocaiSourceResult { get; set; }
        public virtual DbSet<QuestionJiaocaiSourceResultQuestion> QuestionJiaocaiSourceResultQuestion { get; set; }
        public virtual DbSet<QuestionPageAreaSource> QuestionPageAreaSource { get; set; }
        public virtual DbSet<QuestionPageList> QuestionPageList { get; set; }
        public virtual DbSet<QuestionPageSource> QuestionPageSource { get; set; }
        public virtual DbSet<QuestionXkw> QuestionXkw { get; set; }
        public virtual DbSet<Subject> Subject { get; set; }
        public virtual DbSet<SubjectCategory> SubjectCategory { get; set; }
        public virtual DbSet<SubjectGrade> SubjectGrade { get; set; }
        public virtual DbSet<C_log2> C_log2 { get; set; }
        public virtual DbSet<Test> Test { get; set; }
        public virtual DbSet<V_Jiaocai> V_Jiaocai { get; set; }
        public virtual DbSet<V_JiaocaiDetail> V_JiaocaiDetail { get; set; }
        public virtual DbSet<V_JiaocaiDetailYSY> V_JiaocaiDetailYSY { get; set; }
        public virtual DbSet<V_QuestionJiaoCaiSource> V_QuestionJiaoCaiSource { get; set; }
        public virtual DbSet<V_QuestionJiaocaiSourceResult> V_QuestionJiaocaiSourceResult { get; set; }
        public virtual DbSet<V_questionxkw> V_questionxkw { get; set; }
    }
}
