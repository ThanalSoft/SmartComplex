using System.Web.Http;
using Microsoft.Owin.Security.OAuth;

namespace ThanalSoft.SmartComplex.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration pConfig)
        {
            pConfig.SuppressDefaultHostAuthentication();
            pConfig.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            pConfig.MapHttpAttributeRoutes();

            pConfig.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}