using System.Web.Http;
using System.Web.Http.Cors;

namespace CentralizedSecurity.webApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var enableCorsAttribute = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(enableCorsAttribute);


           
            // Configuración y servicios de API web
            //config.MessageHandlers.Add(new TokenValidationHandler());
            //config.Filters.Add()
            //config.Filters.Add(new )
            // Rutas de API web
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
          
        }
    }
}
