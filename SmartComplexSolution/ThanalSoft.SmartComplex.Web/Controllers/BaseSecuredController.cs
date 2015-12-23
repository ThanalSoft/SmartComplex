using System.Security.Principal;
using System.Threading;
using System.Web.Mvc;
using ThanalSoft.SmartComplex.Common.Account;

namespace ThanalSoft.SmartComplex.Web.Controllers
{
    [Authorize]
    public class BaseSecuredController : Controller
    {
        protected LoginUserInfo LoggedInUser
        {
            get { return (LoginUserInfo) Session["UserInfo"]; }
            set { Session["UserInfo"] = value; }
        }

        protected override void OnAuthorization(AuthorizationContext pFilterContext)
        {
            if (LoggedInUser != null)
            {
                var identity = new GenericIdentity(LoggedInUser.Email);
                IPrincipal principal = new GenericPrincipal(identity, new[] {""});
                Thread.CurrentPrincipal = principal;
                pFilterContext.HttpContext.User = principal;
            }
            else
            {
                var identity = new GenericIdentity("");
                IPrincipal principal = new GenericPrincipal(identity, new[] { "" });
                Thread.CurrentPrincipal = principal;
                pFilterContext.HttpContext.User = principal;
            }
            base.OnAuthorization(pFilterContext);
        }
    }
}