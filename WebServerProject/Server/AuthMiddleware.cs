using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebServerProject.Server
{
    public class AuthMiddleware : IMiddleware
    {
        private HttpDelegate next;

        public AuthMiddleware(HttpDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpListenerContext context, Dictionary<string, object> data)
        {
            Cookie user = context.Request.Cookies["user"];
            if (user != null)
            {
                data.Add("user", user.Value);
            }
            await next.Invoke(context, data);
        }
    }
}
