using Autofac;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using WebServerProject.Controllers;

namespace WebServerProject.Server
{
    public class MvcMiddleware : IMiddleware
    {
        private HttpDelegate next;

        public MvcMiddleware(HttpDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpListenerContext context, Dictionary<string, object> data)
        {
            HttpListenerResponse response = context.Response;
            StreamWriter writer = new StreamWriter(response.OutputStream);
            try
            {
                string resp = FindControllerAction(context);
                if (resp != null)
                {
                    response.StatusCode = 200;
                    response.ContentType = "text/html";
                    writer.Write(resp);
                }
                else
                {
                    await next.Invoke(context, data);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                response.StatusCode = 401;
                response.ContentType = "text/plain";
                writer.Write(ex.Message);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.ContentType = "text/plain";
                writer.Write(ex.Message);
            }
            finally
            {
                writer.Close();
            }            
        }

        private string FindControllerAction(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            string[] urlparts = request.Url.LocalPath.Split(new char[] { '/', '\\', '?' }, StringSplitOptions.RemoveEmptyEntries);
            if (urlparts.Length < 2) return null;

            string controller = urlparts[0];
            string action = urlparts[1];

            Assembly curAssembly = Assembly.GetExecutingAssembly();
            Type controllerType = curAssembly.GetType($"WebServerProject.Controllers.{controller}Controller", false, true);
            if (controllerType == null) return null;

            MethodInfo actionMethod = controllerType.GetMethod(action, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
            if (actionMethod == null) return null;

            HttpMethodAttribute attrHttpMethod = actionMethod.GetCustomAttribute<HttpMethodAttribute>();
            if (attrHttpMethod == null)
            {
                attrHttpMethod = new HttpMethodAttribute("get");
            }
            if (attrHttpMethod.Method.ToLower() != request.HttpMethod.ToLower()) return null;

            AuthAttribute attrAuth = methodType.GetCustomAttribute<AuthAttribute>();
            if (attrAuth != null)
            {
                if (!data.ContainsKey("user"))
                {
                    throw new UnauthorizedAccessException();
                }
            }

            List<object> paramsToMethod = new List<object>();
            NameValueCollection coll = null;

            if (request.HttpMethod == "GET")
            {
                if (urlparts.Length == 2 && actionMethod.GetParameters().Length != 0) return null;
                if (urlparts.Length > 2)
                {
                    coll = System.Web.HttpUtility.ParseQueryString(urlparts[2]);
                }
            }
            else if (request.HttpMethod == "POST")
            {
                string body;
                using (StreamReader reader = new StreamReader(request.InputStream))
                {
                    body = reader.ReadToEnd();
                }
                coll = System.Web.HttpUtility.ParseQueryString(body);
            }
            else { return null; }

            ParameterInfo[] parameters = actionMethod.GetParameters();
            foreach (ParameterInfo pi in parameters)
            {
                paramsToMethod.Add(Convert.ChangeType(coll[pi.Name], pi.ParameterType));
            }
            if (paramsToMethod.Count != actionMethod.GetParameters().Length) return null;
            BaseController controllerObject = (BaseController)MyWebServer.Services.Resolve(controllerType);
            controllerObject.Request = context.Request;
            controllerObject.Response = context.Response;
            string resp = (string)actionMethod.Invoke(controllerObject, paramsToMethod.ToArray());
            return resp;
        }
    }
}
