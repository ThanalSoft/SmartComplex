using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThanalSoft.SmartComplex.Web.Security;

namespace ThanalSoft.SmartComplex.Web.Helpers.Html
{
    public static class RoleButtonBiggerHelper
    {
        public static IHtmlString RoleButtonBiggerFor<TModel>(this HtmlHelper<TModel> pHtmlHelper,
            string pText,
            string pId = null,
            string pIcon = null,
            int? pNotifications = null, 
            string pNotificationCss = null,
            object pHtmlRowAttributes = null,
            params string[] pRoles)
        {
            return GetHtmlString<TModel>(pText, pId, pIcon, pNotifications, pNotificationCss, pHtmlRowAttributes, pRoles);
        }

        private static IHtmlString GetHtmlString<TModel>(string pText, string pId, string pIcon, int? pNotifications = null, string pNotificationCss = null, object pHtmlRowAttributes = null, params string[] pRoles)
        {
            var currentUser = HttpContext.Current.User as SmartComplexPrincipal;
            if (currentUser == null)
                return MvcHtmlString.Empty;

            if (pRoles != null && pRoles.Any())
            {
                var inRole = currentUser.Roles.Any(pRoles.Contains);
                if (!inRole)
                    return MvcHtmlString.Empty;
            }

            var aTag = new TagBuilder("a");
            aTag.GenerateId(!string.IsNullOrEmpty(pId) ? pId : Guid.NewGuid().ToString());
            aTag.AddCssClass("btn btn-app pull-left");

            if (pNotifications.HasValue)
            {
                var spanTag = new TagBuilder("span");
                spanTag.AddCssClass("badge " + pNotificationCss);
                spanTag.InnerHtml += pNotifications.Value;
                aTag.InnerHtml += spanTag.ToString();
            }
            if (!string.IsNullOrEmpty(pIcon))
            {
                var iTag = new TagBuilder("i");
                iTag.AddCssClass("fa " + pIcon);
                aTag.InnerHtml += iTag.ToString();
                aTag.InnerHtml += " ";
            }
            aTag.InnerHtml += pText;
            return MvcHtmlString.Create(aTag.ToString());
        }
    }
}