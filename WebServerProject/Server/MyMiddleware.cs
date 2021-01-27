using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WebServerProject.Models;

namespace WebServerProject.Server
{
    public class MyMiddleware : IMiddleware
    {
        private HttpDelegate next;

        public MyMiddleware(HttpDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpListenerContext context, Dictionary<string, object> data)
        {
            BookService bookService = new BookService();

            var request = context.Request;
            var response = context.Response;
            var writer = new StreamWriter(response.OutputStream);
            var reader = new StreamReader(request.InputStream);

            var url = request.Url.LocalPath;
            var method = request.HttpMethod;

            try
            {
                if (url == "/books" && method == "GET")
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
                        .Append("<form action='/addbook' method='POST'>")
                        .Append("<input type='text' name='title' />")
                        .Append("<input type='submit' value='Add book' />")
                        .Append("</form>")
                        .Append("</body></html>");
                    response.ContentType = "text/html";
                    response.StatusCode = 200;
                    writer.Write(resp);
                }
                else if (url == "/addbook" && method == "POST")
                {
                    var datas = HttpUtility.ParseQueryString(reader.ReadToEnd());
                    bookService.AddBook(datas["title"]);
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
                        .Append("<form action='/addbook' method='POST'>")
                        .Append("<input type='text' name='title' />")
                        .Append("<input type='submit' value='Add book' />")
                        .Append("</form>")
                        .Append("</body></html>");
                    response.StatusCode = 200;
                    response.ContentType = "text/html";
                    writer.Write(resp);
                }
                else
                {
                    await next.Invoke(context, data);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                writer.Close();
            }
        }
    }
}
