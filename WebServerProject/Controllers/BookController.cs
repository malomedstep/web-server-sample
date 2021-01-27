using System.Collections.Generic;
using System.Text;
using WebServerProject.Models;
using WebServerProject.Server;

namespace WebServerProject.Controllers
{
    public class BookController : BaseController
    {
        private IBookService bookService = new BookService();

        public BookController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        [HttpMethod(method: "GET")]
        public string All()
        {
            List<string> books = bookService.GetAllBooks();
            StringBuilder resp = new StringBuilder()
                .Append("<html><body>")
                .Append("<h3>All books:</h3>")
                .Append("<ul>");
            foreach (string b in books)
            {
                resp.Append($"<li>{b}</li>");
            }
            resp.Append("</ul>")
                .Append("<br/>")
                .Append("<form action='/book/add' method='POST'>")
                .Append("<input type='text' name='title' />")
                .Append("<input type='submit' value='Add book' />")
                .Append("</form>")
                .Append("</body></html>");
            return resp.ToString();
        }

        [HttpMethod(method: "POST")]
        public string Add(string title)
        {
            bookService.AddBook(title);
            List<string> books = bookService.GetAllBooks();
            StringBuilder resp = new StringBuilder()
                .Append("<html><body>")
                .Append("<h3>All books:</h3>")
                .Append("<ul>");
            foreach (string b in books)
            {
                resp.Append($"<li>{b}</li>");
            }
            resp.Append("</ul>")
                .Append("<br/>")
                .Append("<form action='/book/add' method='POST'>")
                .Append("<input type='text' name='title' />")
                .Append("<input type='submit' value='Add book' />")
                .Append("</form>")
                .Append("</body></html>");
            return resp.ToString();
        }
    }
}
