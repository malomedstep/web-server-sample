using System;

namespace WebServerProject.Server
{
    public class HttpMethodAttribute : Attribute
    {
        public string Method { get; set; }

        public HttpMethodAttribute(string method)
        {
            if (method.ToLower() != "get" && method.ToLower() != "post")
            {
                throw new ArgumentException("Method must be GET or POST.");
            }
            Method = method;
        }
    }
}
