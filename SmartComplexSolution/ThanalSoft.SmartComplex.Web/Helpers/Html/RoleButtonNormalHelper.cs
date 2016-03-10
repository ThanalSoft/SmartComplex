using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThanalSoft.SmartComplex.Web.Security;

namespace ThanalSoft.SmartComplex.Web.Helpers.Html
{
    public static class RoleButtonNormalHelper
    {
        public static IHtmlString RoleButtonNormalFor<TModel>(this HtmlHelper<TModel> pHtmlHelper,
            string pText,
            string pId = null,
            string pIcon = null,
            object pHtmlRowAttributes = null,
            params string[] pRoles)
        {
            return GetHtmlString<TModel>(pText, pId, pIcon, pHtmlRowAttributes, pRoles);
        }

        private static IHtmlString GetHtmlString<TModel>(string pText, string pId, string pIcon, object pHtmlRowAttributes = null, params string[] pRoles)
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

            var buttonTag = new TagBuilder("button");
            buttonTag.GenerateId(!string.IsNullOrEmpty(pId) ? pId : Guid.NewGuid().ToString());
            buttonTag.AddCssClass("btn btn-success");
            buttonTag.Attributes.Add("type", "button");

            if (!string.IsNullOrEmpty(pIcon))
            {
                var iTag = new TagBuilder("i");
                iTag.AddCssClass("fa " + pIcon);
                buttonTag.InnerHtml += iTag.ToString();
                buttonTag.InnerHtml += " ";
            }
            buttonTag.InnerHtml += pText;
            return MvcHtmlString.Create(buttonTag.ToString());
        }
    }
}