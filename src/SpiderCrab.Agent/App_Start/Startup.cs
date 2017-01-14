namespace SpiderCrab.Agent
{
    using System;
    using System.Net;
    using System.Reflection;
    using System.Web.Http;
    using Newtonsoft.Json.Serialization;
    using Ninject;
    using Ninject.Web.Common.OwinHost;
    using Ninject.Web.WebApi.OwinHost;
    using Owin;
    using Properties;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var listenerKey = typeof(HttpListener).FullName;
            if (app.Properties == null || !app.Properties.ContainsKey(listenerKey))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(app), "Cannnot access HTTP listener property");
            }

            // Authn; Authz
            var listener = (HttpListener)app.Properties[listenerKey];
            listener.AuthenticationSchemes = AuthenticationSchemes.IntegratedWindowsAuthentication;
            var config = new HttpConfiguration();
            config.Filters.Add(
                new AuthorizeAttribute { Roles = $"{Environment.MachineName}\\SpiderCrab Operators" });

            // Routes
            config.Routes.MapHttpRoute(
                name: "default",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            // Serialization
            config.Formatters.XmlFormatter.UseXmlSerializer = true;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver
                = new CamelCasePropertyNamesContractResolver();

            // Dependency injection
            app.UseNinjectMiddleware(CreateKernel)
                .UseNinjectWebApi(config);
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel(new ServiceModule(Settings.Default));
            kernel.Load(Assembly.GetExecutingAssembly());
            return kernel;
        }
    }
}