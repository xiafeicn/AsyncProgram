using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabzujuan.Data;

namespace Grabzujuan.Common
{
    public static class DataService
    {

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

        public static List<Book> GetBooklist()
        {
            using (var db = new CrawlerEntities())
            {
                return db.Book.ToList();
            }
        }

        public static List<V_AllCategory> GetCategorylist()
        {
            using (var db = new CrawlerEntities())
            {
                return db.V_AllCategory.ToList();
            }
        }
        public static List<int> GetCrawleredCatelist()
        {
            using (var db = new CrawlerEntities())
            {
                return db.CategoryUrlList.Select(t => t.CategoryId).Distinct().ToList();
            }
        }
        public static void UpdateCategoryCount(int id, int count)
        {
            using (var db = new CrawlerEntities())
            {
                var entity = db.Category.FirstOrDefault(t => t.Id == id);
                if (entity != null)
                {
                    entity.Total = count;
                    db.SaveChanges();
                }
            }
        }

        public static void AddCateUrl(string url, int categoryId, int pageNum, string apiUrl, string apiJson)
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
                db.CategoryUrlList.Add(entity);
                db.SaveChanges();
            }
        }

        public static void AddCategory(int bookId, int categoryId, string categoryName, int total)
        {
            using (var db = new CrawlerEntities())
            {
                if (db.Category.Any(t => t.CategoryId == categoryId))
                    return;
                var entity = new Category();
                ;
                entity.BookId = bookId;
                entity.CategoryId = categoryId;
                entity.CategoryName = categoryName;
                entity.GrabUrl = "";
                entity.Total = total;
                db.Category.Add(entity);
                db.SaveChanges();
            }
        }
    }
}
