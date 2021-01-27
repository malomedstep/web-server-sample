using System.Collections.Generic;

namespace WebServerProject.Models
{
    public class BookService : IBookService
    {
        static private List<string> books;

        static BookService()
        {
            books = new List<string>()
            {
                "Book 1", 
                "Book 2", 
                "Book 3", 
                "Book 4", 
                "Book 5",
            };
        }

        public List<string> GetAllBooks()
        {
            return books;
        }

        public void AddBook(string title)
        {
            books.Add(title);
        }
    }
}
