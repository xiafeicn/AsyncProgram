using System.Collections.Generic;
using System.Linq;
using Common;
using Grabzujuan;

namespace CrawlerHttp
{
    public class BookService
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
    }
}
