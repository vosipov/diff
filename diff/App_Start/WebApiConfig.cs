using System.Web.Http;
using Newtonsoft.Json;

namespace diff
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

//            config.Routes.MapHttpRoute(
//                name: "diffApi",
//                routeTemplate: "v1/{controller}/{id}",
//                defaults: new { id = RouteParameter.Optional }
//            );

//            config.Formatters.Add(new BrowserJsonFormatter());
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        }
    }
}
