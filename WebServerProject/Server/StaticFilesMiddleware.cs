using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace WebServerProject.Server
{
    public class StaticFilesMiddleware : IMiddleware
    {
        private HttpDelegate next;

        public StaticFilesMiddleware(HttpDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpListenerContext context, Dictionary<string, object> data)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            string url = request.Url.LocalPath.Trim('/', '\\');
            if (Path.HasExtension(url))
            {
                string path = $@"{Directory.GetCurrentDirectory()}\..\..\wwwroot\{url}";
                if (File.Exists(path))
                {
                    using (StreamWriter writer = new StreamWriter(response.OutputStream))
                    {
                        writer.Write(File.ReadAllText(path));
                    }
                    response.StatusCode = 200;
                    if (Path.GetExtension(url) == ".html")
                    {
                        response.ContentType = "text/html";
                    }
                    return;
                }
            }
            await next.Invoke(context, data);
        }
    }
}
