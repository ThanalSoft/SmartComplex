using System.Web.Mvc;

namespace ThanalSoft.SmartComplex.Website
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection pFilters)
        {
            pFilters.Add(new HandleErrorAttribute());
        }
    }
}