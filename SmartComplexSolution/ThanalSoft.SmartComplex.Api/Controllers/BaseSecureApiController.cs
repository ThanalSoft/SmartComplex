using System;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ThanalSoft.SmartComplex.Api.Security;

namespace ThanalSoft.SmartComplex.Api.Controllers
{
    [Authorize]
    public class BaseSecureApiController : ApiController
    {
        private SecureUserManager _userManager;
        private SecureSignInManager _signInManager;

        protected SecureUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<SecureUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        protected SecureSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<SecureSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        protected Int64 LoggedInUser => Convert.ToInt64(User.Identity.GetUserId());
    }
}