using Autofac;
using WebServerProject.Controllers;
using WebServerProject.Models;

namespace WebServerProject.Server
{
    public interface IConfigurator
    {
        void ConfigureMiddleware(MiddlewareBuilder builder);
        void ConfigureServices(ContainerBuilder builder);
    }

    public class Configurator : IConfigurator
    {
        public void ConfigureMiddleware(MiddlewareBuilder builder)
        {
            builder.Use<Middleware1>()
                .Use<StaticFilesMiddleware>()
                .Use<AuthMiddleware>()
                .Use<MvcMiddleware>();
        }

        public void ConfigureServices(ContainerBuilder builder)
        {
            builder.RegisterType<HomeController>();
            builder.RegisterType<BookController>();
            builder.RegisterType<BookService>().As<IBookService>();
        }
    }
}
