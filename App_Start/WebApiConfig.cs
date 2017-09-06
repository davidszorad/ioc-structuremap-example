using StructureMap;
using System.Web.Http;
using IoCStructureMapConfiguration.IoC;

namespace IoCStructureMapConfiguration
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var container = new Container(x =>
            {
                x.Scan(scan =>
                {
                    scan.AssembliesAndExecutablesFromApplicationBaseDirectory(assembly => assembly.GetName().Name.StartsWith("Volvo"));
                    scan.WithDefaultConventions();
                });
            });

            config.DependencyResolver = new StructureMapResolver(container);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
