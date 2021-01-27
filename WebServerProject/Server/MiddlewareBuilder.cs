using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebServerProject.Server
{
    public class MiddlewareBuilder
    {
        private Stack<Type> types = new Stack<Type>();
        
        public MiddlewareBuilder Use<T>()
        {
            int count = typeof(T).GetConstructors().Where(c => 
                c.GetParameters().Count() == 1 && 
                c.GetParameters()[0].ParameterType == typeof(HttpDelegate))
                .Count();
            if (count == 0)
            {
                throw new Exception("Middleware need have constructor with HttpDelegate param");
            }
            types.Push(typeof(T));
            return this;
        }

        public HttpDelegate Build()
        {
            HttpDelegate first = Last;
            while (types.Count > 0)
            {
                first = (Activator.CreateInstance(types.Pop(), first) as IMiddleware).InvokeAsync;
            }
            return first;
        }

        private async Task Last(HttpListenerContext context, Dictionary<string, object> data)
        {
            context.Response.StatusCode = 404;
            context.Response.Close();
        }
    }
}
