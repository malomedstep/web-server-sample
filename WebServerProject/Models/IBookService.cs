using System.Collections.Generic;

namespace WebServerProject.Models
{
    public interface IBookService
    {
        List<string> GetAllBooks();
        void AddBook(string title);
    }
}
