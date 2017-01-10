namespace SpiderCrab.Agent
{
    using Ninject;
    using Ninject.Web.Common.OwinHost;
    using Ninject.Web.WebApi.OwinHost;
    using Owin;
    using Properties;
    using System.Reflection;
    using System.Web.Http;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "default",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            app.UseNinjectMiddleware(CreateKernel)
                .UseNinjectWebApi(config);
        }

        private IKernel CreateKernel()
        {
            var kernel = new StandardKernel(new ServiceModule(Settings.Default));
            kernel.Load(Assembly.GetExecutingAssembly());
            return kernel;
        }
    }
}