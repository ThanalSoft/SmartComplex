using System.Web.Mvc;
using System.Web.Routing;

namespace ThanalSoft.SmartComplex.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection pRoutes)
        {
            pRoutes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            pRoutes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Account", action = "Index", id = UrlParameter.Optional },
                new[] { "ThanalSoft.SmartComplex.Web.Controllers" }
            );
        }
    }
}