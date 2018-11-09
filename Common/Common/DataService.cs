using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Grabzujuan.Common
{
    public static class DataService
    {
        public static List<V_ALL_CategoryUrlList> GetQuestionList()
        {
            using (var db = new CrawlerEntities())
            {
                return db.V_ALL_CategoryUrlList.Where(t => t.Status == false).Take(4000).ToList();
            }
        }

        public static void UpdateQuestionStatus(int id)
        {
            using (var db = new CrawlerEntities())
            {
                var entity = db.CategoryUrlList.FirstOrDefault(t => t.Id == id);
                entity.Status = true;
                db.SaveChanges();
            }
        }

        public static bool ExistQuestion(int questionId)
        {
            using (var db = new CrawlerEntities())
            {
                return db.Question.Any(t => t.Question_Id == questionId);
            }
        }
        public static void AddQuestion(Question entity)
        {
            try
            {
                using (var db = new CrawlerEntities())
                {
                    db.Question.Add(entity);
                    db.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
                var msg = string.Empty;

                foreach (var validationError in ((DbEntityValidationException)ex).EntityValidationErrors)
                    foreach (var error in validationError.ValidationErrors)
                        msg += string.Format("Property: {0} Error: {1}", error.PropertyName, error.ErrorMessage);

                var fail = new Exception(msg);

            }
        }
        public static void UpdateQuestionFromCrawler(int questionId, string QuestionHtml, string answer, string knowledge, string questionfrom, string QuestionSource, string Score, string CrawlerUrl, string AnswerJson)
        {
            try
            {
                using (var db = new CrawlerEntities())
                {
                    var entity = db.Question.FirstOrDefault(t => t.Question_Id == questionId);
                    if (entity != null)
                    {
                        entity.QuestionHtml = QuestionHtml;
                        entity.answer = answer;
                        entity.Knowledge = knowledge;
                        entity.QuestionFrom = questionfrom;
                        entity.QuestionSource = QuestionSource;
                        entity.Score = Score;
                        entity.CrawlerUrl = CrawlerUrl;
                        entity.AnswerJson = AnswerJson;
                        entity.IsGrabAns = true;
                        db.SaveChanges();
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                var msg = string.Empty;

                foreach (var validationError in ((DbEntityValidationException)ex).EntityValidationErrors)
                    foreach (var error in validationError.ValidationErrors)
                        msg += string.Format("Property: {0} Error: {1}", error.PropertyName, error.ErrorMessage);

                throw new Exception(msg);
            }
        }

        public static void AddQuestion(List<Question> entity)
        {
            try
            {
                using (var db = new CrawlerEntities())
                {
                    foreach (var e in entity)
                    {
                        if (!db.Question.Any(t => t.Question_Id == e.Question_Id))
                        {
                            db.Question.Add(e);
                        }
                    }
                    db.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
                var msg = string.Empty;

                foreach (var validationError in ((DbEntityValidationException)ex).EntityValidationErrors)
                    foreach (var error in validationError.ValidationErrors)
                        msg += string.Format("Property: {0} Error: {1}", error.PropertyName, error.ErrorMessage);

                var fail = new Exception(msg);
                throw fail;
            }
        }

        public static void AddBook(int childId, int degreeId, string bookName, int bookVersionId)
        {
            using (var db = new CrawlerEntities())
            {
                if (db.Book.Any(t => t.BookVersionId == bookVersionId))
                    return;
                var entity = new Book();
                ;
                entity.BookName = bookName;
                entity.BookVersionId = bookVersionId;
                entity.Child = childId.NullToInt();
                entity.Degree = degreeId;

                db.Book.Add(entity);
                db.SaveChanges();
            }
        }


        public static List<V_AllCategory> GetCategorylist()
        {
            using (var db = new CrawlerEntities())
            {
                return db.V_AllCategory.ToList();
            }
        }

        //public static void UpdateCategoryTotalCount(int categoryId,int count)
        //{
        //    using (var db = new CrawlerEntities())
        //    {
        //        var entity = db.Category.FirstOrDefault(t => t.CategoryId == categoryId);
        //        entity.Total = count;
        //        db.SaveChanges();
        //    }
        //}



        public static List<int> GetCrawleredCatelist()
        {
            using (var db = new CrawlerEntities())
            {
                return db.CategoryUrlList.Select(t => t.CategoryId).Distinct().ToList();
            }
        }
        //public static void UpdateCategoryCount(int id, int count)
        //{
        //    using (var db = new CrawlerEntities())
        //    {
        //        var entity = db.Category.FirstOrDefault(t => t.Id == id);
        //        if (entity != null)
        //        {
        //            entity.Total = count;
        //            db.SaveChanges();
        //        }
        //    }
        //}
        public static bool IsCateUrlGrabed(int categoryId, int page)
        {
            using (var db = new CrawlerEntities())
            {
                return db.CategoryUrlList.Any(t => t.CategoryId == categoryId && t.PageNum == page);
            }
        }

        public static void AddCateUrl(string url, int categoryId, int pageNum, string apiUrl, string apiJson,int index)
        {
            using (var db = new CrawlerEntities())
            {
                if (db.CategoryUrlList.Any(t => t.GrabUrl == url))
                    return;
                var entity = new CategoryUrlList();
                ;
                entity.GrabUrl = url;
                entity.CategoryId = categoryId;
                entity.PageNum = pageNum;
                entity.ApiJson = apiJson;
                entity.ApiUrl = apiUrl;
                entity.IndexOfTask = index;
                db.CategoryUrlList.Add(entity);
                db.SaveChanges();
            }
        }

        //public static void AddCategory(int bookId, int categoryId, string categoryName, int total)
        //{
        //    using (var db = new CrawlerEntities())
        //    {
        //        if (db.Category.Any(t => t.CategoryId == categoryId))
        //            return;
        //        var entity = new Category();
        //        ;
        //        entity.BookId = bookId;
        //        entity.CategoryId = categoryId;
        //        entity.CategoryName = categoryName;
        //        entity.GrabUrl = "";
        //        entity.Total = total;
        //        db.Category.Add(entity);
        //        db.SaveChanges();
        //    }
        //}
    }
}
