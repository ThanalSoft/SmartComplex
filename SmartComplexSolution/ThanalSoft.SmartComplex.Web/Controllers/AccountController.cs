﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using ThanalSoft.SmartComplex.Common;
using ThanalSoft.SmartComplex.Common.Models.Account;
using ThanalSoft.SmartComplex.Common.Models.Common;
using ThanalSoft.SmartComplex.Web.Common;
using ThanalSoft.SmartComplex.Web.Models.Account;
using ThanalSoft.SmartComplex.Web.Models.Common;
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
        public async Task<ActionResult> Logout()
        {
            await new ApiConnector<GeneralReturnInfo>().SecurePostAsync("Account", "UserLogout", User.Email);
            FormsAuthentication.SignOut();
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> UserProfile()
        {
            var response = GetUserProfileInfo();
            var bloodGroups = GetBloodGroups();
            await Task.WhenAll(response, bloodGroups);

            if (response.Result?.Info != null)
            {
                var profile = new ProfileViewModel
                {
                    FirstName = response.Result.Info.FirstName,
                    LastName = response.Result.Info.LastName,
                    Mobile = response.Result.Info.Mobile,
                    BloodGroupId = response.Result.Info.BloodGroupId,
                    BloodGroups = bloodGroups.Result.ToArray()
                };

                return View(new ProfileUpdateViewModel
                {
                    Email = response.Result.Info.Email,
                    ProfileViewModel = profile,
                    CredentialViewModel = new CredentialViewModel(),
                });
            }

            ////This should happen only for admin user
            return View(new ProfileUpdateViewModel
            {
                Email = User.Email,
                ProfileViewModel = new ProfileViewModel
                {
                    BloodGroups = bloodGroups.Result.ToArray()
                },
                CredentialViewModel = new CredentialViewModel()
            });
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult PasswordUpdated()
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

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<PartialViewResult> UpdateProfile(ProfileViewModel pProfileViewModel)
        {
            var bloodGroups = await GetBloodGroups();
            pProfileViewModel.BloodGroups = bloodGroups.ToArray();

            if (!ModelState.IsValid)
            {
                return PartialView("_UpdateUserProfile", pProfileViewModel);
            }

            var result = await UpdateUserProfile(new UserProfileInfo
            {
                FirstName = pProfileViewModel.FirstName,
                LastName = pProfileViewModel.LastName,
                Mobile = pProfileViewModel.Mobile,
                BloodGroupId = pProfileViewModel.BloodGroupId
            });

            pProfileViewModel.ActionResultStatus = result.Result == ApiResponseResult.Error 
                ? new ActionResultStatusViewModel(result.Reason, ActionStatus.Error) 
                : new ActionResultStatusViewModel("Your profile is updated successfully.", ActionStatus.Success);

            return PartialView("_UpdateUserProfile", pProfileViewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> UpdateCredentails(CredentialViewModel pCredentialsViewModel)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_UpdateCredentials", pCredentialsViewModel);
            }

            var result = await UpdateUserCredentials(new UserProfileInfo
            {
                Password = pCredentialsViewModel.Password,
                NewPassword = pCredentialsViewModel.NewPassword
            });

            pCredentialsViewModel.ActionResultStatus = result.Result == ApiResponseResult.Error
               ? new ActionResultStatusViewModel(result.Reason, ActionStatus.Error)
               : new ActionResultStatusViewModel("Your Credentials updated successfully.", ActionStatus.Success);

            if (pCredentialsViewModel.ActionResultStatus.ActionStatus == ActionStatus.Success)
            {
                FormsAuthentication.SignOut();
                return JavaScript("document.location.replace('" + Url.Action("PasswordUpdated") + "');");
            }

            return PartialView("_UpdateCredentials", pCredentialsViewModel);
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

        [NonAction]
        private async Task<GeneralReturnInfo<UserProfileInfo>> GetUserProfileInfo()
        {
            return await new ApiConnector<GeneralReturnInfo<UserProfileInfo>>().SecureGetAsync("Account", "GetUserProfileDetails", User.UserId.ToString());
        }

        [NonAction]
        private async Task<GeneralReturnInfo> UpdateUserProfile(UserProfileInfo pUserProfileInfo)
        {
            pUserProfileInfo.UserId = User.UserId;
            var result = await new ApiConnector<GeneralReturnInfo>().SecurePostAsync("Account", "UpdateUserProfile", pUserProfileInfo);
            return result;
        }

        [NonAction]
        private async Task<GeneralReturnInfo> UpdateUserCredentials(UserProfileInfo pUserProfileInfo)
        {
            pUserProfileInfo.UserId = User.UserId;
            var result = await new ApiConnector<GeneralReturnInfo>().SecurePostAsync("Account", "UpdateCredentials", pUserProfileInfo);
            return result;
        }

        [NonAction]
        private async Task<List<SelectListItem>> GetBloodGroups()
        {
            var response = await new ApiConnector<GeneralReturnInfo<GeneralInfo[]>>().SecureGetAsync("Common", "GetBloodGroups");
            var ddlItems = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "0",
                    Text = "-- Select --"
                }
            };
            ddlItems.AddRange(response.Info.Select((pX => new SelectListItem
            {
                Text = pX.Name,
                Value = Convert.ToString(pX.Id)
            })));

            return ddlItems;
        }

        #endregion

    }
}