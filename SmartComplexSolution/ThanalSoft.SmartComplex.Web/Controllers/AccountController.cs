using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using ThanalSoft.SmartComplex.Common;
using ThanalSoft.SmartComplex.Common.Models.Account;
using ThanalSoft.SmartComplex.Web.Common;
using ThanalSoft.SmartComplex.Web.Models.Account;
using ThanalSoft.SmartComplex.Web.Security;

namespace ThanalSoft.SmartComplex.Web.Controllers
{
    public class AccountController : BaseSecuredController
    {
        #region Get Methods

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index(string returnUrl)
        {
            if (User != null)
                return RedirectToAction("Index", "Home", new { area = "Dashboard" });

            ViewBag.ReturnUrl = returnUrl;
            return View(new UserLoginModel
            {
                Email = "admin@sc.com",
                Password = "admin"
            });
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string id, string token)
        {
            var response = await new ApiConnector<GeneralReturnInfo>().PostAsync("Account", "ConfirmUser", new ConfirmEmailAccount { Id = id, Token = token });
            if (response.Result == ApiResponseResult.Success)
                return View();

            return View(new ErrorConfirmModel(response.Reason));
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return View();
        }

        [HttpGet]
        public ActionResult Profile()
        {
            return View();
        }

        #endregion

        #region Post Methods

        [ValidateAntiForgeryToken]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Index(UserLoginModel pModel, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(pModel);
            }
            var loginResponse = await new ApiConnector<LoginResultInfo>().PostAsync("Account", "SecureLogin", new LoginRequestInfo(pModel.Email, pModel.Password));

            switch (loginResponse.LoginStatus)
            {
                case LoginStatus.Success:
                    var token = await new ApiConnector<object>().GetApiToken(pModel.Email, pModel.Password);
                    if (string.IsNullOrEmpty(token))
                    {
                        ViewBag.ModelError = "Invalid credentials. Try again.";
                        return View(pModel);
                    }

                    var serializeModel = new SmartComplexPrincipalSerializeModel
                    {
                        Email = loginResponse.LoginUserInfo.Email,
                        Name = loginResponse.LoginUserInfo.Name,
                        UserName = loginResponse.LoginUserInfo.UserName,
                        UserIdentity = token,
                        UserId = loginResponse.LoginUserInfo.UserId,
                        Roles = loginResponse.LoginUserInfo.Roles
                    };
                    var serializer = new JavaScriptSerializer();
                    var userData = serializer.Serialize(serializeModel);

                    var authTicket = new FormsAuthenticationTicket(
                             1,
                             loginResponse.LoginUserInfo.Email,
                             DateTime.Now,
                             DateTime.Now.AddDays(5),
                             true,
                             userData);

                    var encTicket = FormsAuthentication.Encrypt(authTicket);
                    var faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    Response.Cookies.Add(faCookie);

                    if (string.IsNullOrEmpty(returnUrl))
                        return RedirectToAction("Index", "Home", new { area = "Dashboard" });

                    return RedirectToLocal(returnUrl);
                case LoginStatus.LockedOut:
                    ViewBag.ModelError = "Your account is locked. Try after sometime.";
                    return View(pModel);
                case LoginStatus.RequiresVerification:
                    ViewBag.ModelError = "Your email is not yet confirmed, login to your email and check the Welcome mail.";
                    return View(pModel);
                case LoginStatus.Failure:
                    ViewBag.ModelError = "Invalid credentials. Try again.";
                    return View(pModel);
                default:
                    ViewBag.ModelError = "Invalid credentials. Try again.";
                    return View(pModel);
            }
        }

        #endregion

        #region Private Methods

        [NonAction]
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}