using System;
using System.Web;
using System.Web.Mvc;

namespace ThanalSoft.SmartComplex.Web.Attributes
{
    public class WebAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase pHttpContext)
        {
            if (pHttpContext.Session?["UserInfo"] != null)
                return true;
            return false;
        }
    }
}