using System;
using WebServerProject.Server;

namespace WebServerProject
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Server is running...");
                MyWebServer webServer = new MyWebServer("http://127.0.0.1", "8084");
                webServer.Configure<Configurator>().Run();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Server stoped");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
