using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace WebServerProject.Server
{
    public interface IMiddleware
    {
        Task InvokeAsync(HttpListenerContext context, Dictionary<string, object> data);
    }

    public delegate Task HttpDelegate(HttpListenerContext context, Dictionary<string, object> data);

    public class Middleware1 : IMiddleware
    {
        private HttpDelegate next;

        public Middleware1(HttpDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpListenerContext context, Dictionary<string, object> data)
        {
            Console.WriteLine("Middleware 1 begin work: " + context.Request.Url.AbsoluteUri);
            await next.Invoke(context, data);
            Console.WriteLine("Middleware 1 end work");
        }
    }
}
