using System.Threading.Tasks;
using System.Web.Mvc;
using ThanalSoft.SmartComplex.Common;
using ThanalSoft.SmartComplex.Common.Models.Account;
using ThanalSoft.SmartComplex.Web.Common;
using ThanalSoft.SmartComplex.Web.Models;
using ThanalSoft.SmartComplex.Web.Models.Account;

namespace ThanalSoft.SmartComplex.Web.Controllers
{
    public class AccountController : BaseSecuredController
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index(string returnUrl)
        {
            if (LoggedInUser != null)
                return RedirectToAction("Index", "Home");

            ViewBag.ReturnUrl = returnUrl;
            return View(new UserLoginModel
            {
                Email = "admin@sc.com",
                Password = "admin"
            });
        }

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
                    var token = await new ApiConnector<string>().GetApiToken(pModel.Email, pModel.Password);
                    if (string.IsNullOrEmpty(token))
                    {
                        ViewBag.ModelError = "Invalid credentials. Try again.";
                        return View(pModel);
                    }
                    LoggedInUser = loginResponse.LoginUserInfo;
                    LoggedInUser.UserIdentity = token;

                    if (string.IsNullOrEmpty(returnUrl))
                        return RedirectToAction("Index", "Home");

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

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string id, string token)
        {
            var response = await new ApiConnector<GeneralReturnInfo>().PostAsync("Account", "ConfirmUser", new ConfirmEmailAccount { Id = id, Token = token });
            if(response.Result == ApiResponseResult.Success)
                return View();

            return View(new ErrorConfirmModel(response.Reason));
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Logout()
        {
            LoggedInUser = null;
            return RedirectToAction("Index");
        }
    }
}