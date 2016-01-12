using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using ThanalSoft.SmartComplex.Web.Security;

namespace ThanalSoft.SmartComplex.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthenticateRequest(Object pSender, EventArgs e)
        {
            var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                var serializer = new JavaScriptSerializer();

                if (authTicket != null)
                {
                    var serializeModel = serializer.Deserialize<SmartComplexPrincipalSerializeModel>(authTicket.UserData);

                    var newUser = new SmartComplexPrincipal(authTicket.Name)
                    {
                        UserId = serializeModel.UserId,
                        Email = serializeModel.Email,
                        Name = serializeModel.Name,
                        UserName = serializeModel.UserName,
                        UserIdentity = serializeModel.UserIdentity,
                        Roles = serializeModel.Roles
                    };

                    HttpContext.Current.User = newUser;
                }
            }
        }
    }
}