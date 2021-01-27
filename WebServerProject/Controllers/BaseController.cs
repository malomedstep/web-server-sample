using System.Net;

namespace WebServerProject.Controllers
{
    public class BaseController
    {
        public HttpListenerRequest Request { get; set; }
        public HttpListenerResponse Response { get; set; }
    }
}
