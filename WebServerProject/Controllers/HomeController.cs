using System;
using System.Net;
using WebServerProject.Server;

namespace WebServerProject.Controllers
{
    public class HomeController : BaseController
    {
        [Auth]
        [HttpMethod(method: "GET")]
        public string Index()
        {
            return "Home/Index";
        }

        [Auth]
        [HttpMethod(method: "GET")]
        public string About()
        {
            return "Home/About";
        }

        [HttpMethod(method: "GET")]
        public string AuthPage()
        {
            return "<form method='post' action='/home/login'>" +
                "<input type='text' name='login' required /><br/>" +
                "<input type='submit' /><br/>" +
                "</form>";
        }

        [HttpMethod(method: "POST")]
        public string LogIn(string login)
        {
            Response.Cookies.Add(new Cookie("user", login));
            return "<a href='/home/index'>index page</a></br>" +
                "<a href='/home/about'>about page</a></br></br>" +
                "<a href='/home/logout'>logout</a></br>";
        }

        [Auth]
        [HttpMethod(method: "GET")]
        public string LogOut()
        {
            Cookie cookie = Request.Cookies["user"];
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);
            return "<a href='/home/authpage'>auth page</a></br>";
        }
    }
}
