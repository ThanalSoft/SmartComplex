﻿using System.Threading.Tasks;
using System.Web.Mvc;
using ThanalSoft.SmartComplex.Common.Models.Account;
using ThanalSoft.SmartComplex.Web.Common;
using ThanalSoft.SmartComplex.Web.Models;

namespace ThanalSoft.SmartComplex.Web.Controllers
{
    public class AccountController : BaseSecuredController
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index()
        {
            LoggedInUser = null;

            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Index(UserLoginModel pModel, string pReturnurl)
        {
            if (!ModelState.IsValid)
            {
                return View(pModel);
            }
            var loginResponse = await new ApiConnector<LoginResultInfo>().PostAsync("Account", "SecureLogin", 
                                    new LoginRequestInfo(pModel.Email, pModel.Password));

            switch (loginResponse.LoginStatus)
            {
                case LoginStatus.Success:
                    var token = await new ApiConnector<string>().GetApiToken(pModel.Email, pModel.Password);
                    LoggedInUser = loginResponse.LoginUserInfo;
                    LoggedInUser.UserIdentity = token;
                    return RedirectToAction("Index", "Home");
                case LoginStatus.LockedOut:
                    break;
                case LoginStatus.RequiresVerification:
                    break;
                case LoginStatus.Failure:
                    ViewBag.ModelError = "Invalid credentials. Try again.";
                    return View(pModel);
                default:
                    ViewBag.ModelError = "Invalid credentials. Try again.";
                    return View(pModel);
            }
            return View();
        }
    }
}