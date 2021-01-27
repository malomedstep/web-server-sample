using Autofac;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace WebServerProject.Server
{
    public class MyWebServer
    {
        private string domain;
        private string port;

        private HttpListener httpListener;
        private HttpDelegate firstMiddleware;

        public static IContainer Services;

        public MyWebServer(string domain, string port)
        {
            this.domain = domain;
            this.port = port;
            httpListener = new HttpListener();
            httpListener.Prefixes.Add($"{domain}:{port}/");
        }

        public MyWebServer Configure<T>() where T : IConfigurator, new()
        {
            IConfigurator configurator = new T();
            MiddlewareBuilder middlewareBuilder = new MiddlewareBuilder();
            configurator.ConfigureMiddleware(middlewareBuilder);
            firstMiddleware = middlewareBuilder.Build();

            ContainerBuilder containerBuilder = new ContainerBuilder();
            //builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
            configurator.ConfigureServices(containerBuilder);
            Services = containerBuilder.Build();

            return this;
        }

        public void Run()
        {
            httpListener.Start();
            while(true)
            {
                HttpListenerContext context = httpListener.GetContext();
                Task.Run(() => { Process(context); });
            }
        }

        private void Process(HttpListenerContext context)
        {
            try
            {
                firstMiddleware.Invoke(context, new Dictionary<string, object>());              
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "text/plain";
                using (StreamWriter writer = new StreamWriter(context.Response.OutputStream))
                {
                    writer.Write($"Error on MyWebServer: {ex.Message}");
                }
            }

        }
    }
}
