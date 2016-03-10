using System.Web.Mvc;

namespace ThanalSoft.SmartComplex.Web.Helpers.Html
{
    public static class UrlGenerationHelper
    {
        public static string AbsoluteAction(this UrlHelper pUrl, string pActionName, string pControllerName, object pRouteValues = null)
        {
            if (pUrl.RequestContext.HttpContext.Request.Url != null)
            {
                var scheme = pUrl.RequestContext.HttpContext.Request.Url.Scheme;
                return pUrl.Action(pActionName, pControllerName, pRouteValues, scheme);
            }
            return string.Empty;
        }
    }
}