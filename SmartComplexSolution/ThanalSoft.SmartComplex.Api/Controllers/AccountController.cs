﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using ThanalSoft.SmartComplex.Business.Complex;
using ThanalSoft.SmartComplex.Common;
using ThanalSoft.SmartComplex.Common.Models.Account;

namespace ThanalSoft.SmartComplex.Api.Controllers
{
    [System.Web.Http.RoutePrefix("api/Account")]
    public class AccountController : BaseSecureApiController
    {
        [System.Web.Http.HttpPost]
        [System.Web.Http.AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<LoginResultInfo> SecureLogin(LoginRequestInfo pLogin)
        {
            var user = await UserManager.FindByEmailAsync(pLogin.Email);
            if(user == null)
                return new LoginResultInfo
                {
                    LoginStatus = LoginStatus.Failure
                };
            if (user.IsDeleted)
            {
                return new LoginResultInfo
                {
                    LoginStatus = LoginStatus.Deleted
                };
            }

            var status = await SignInManager.PasswordSignInAsync(pLogin.Email, pLogin.Password, true, true);
            switch (status)
            {
                case SignInStatus.Success:
                    var flatUser = await ApartmentContext.Instance.GetMember(user.Id);
                    var userInfo = new LoginUserInfo
                    {
                        Email = pLogin.Email,
                        UserName = user.UserName,
                        UserId = user.Id,
                        Roles = (await UserManager.GetRolesAsync(user.Id)).ToArray(),
                        Name = flatUser == null 
                                    ? pLogin.Email 
                                    : flatUser.FirstName + (string.IsNullOrEmpty(flatUser.LastName) 
                                                ? "" 
                                                : " " + flatUser.LastName),
                    };
                    return new LoginResultInfo
                    {
                        LoginStatus = LoginStatus.Success,
                        LoginUserInfo = userInfo
                    };
                case SignInStatus.LockedOut:
                    return new LoginResultInfo
                    {
                        LoginStatus = LoginStatus.LockedOut
                    };
                case SignInStatus.RequiresVerification:
                    return new LoginResultInfo
                    {
                        LoginStatus = LoginStatus.RequiresVerification
                    };
                case SignInStatus.Failure:
                    return new LoginResultInfo
                    {
                        LoginStatus = LoginStatus.Failure
                    };
                default:
                    return new LoginResultInfo
                    {
                        LoginStatus = LoginStatus.Failure
                    };
            }
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.AllowAnonymous]
        public async Task<GeneralReturnInfo> ConfirmUser(ConfirmEmailAccount pConfirmEmailAccount)
        {
            var result = new GeneralReturnInfo();

            if (string.IsNullOrEmpty(pConfirmEmailAccount.Id))
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = "User Empty!";

                return result;
            }
            if (string.IsNullOrEmpty(pConfirmEmailAccount.Token))
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = "Token Empty!";
                return result;
            }

            try
            {
                var user = await UserManager.FindByIdAsync(Convert.ToInt64(pConfirmEmailAccount.Id));
                if (user == null)
                {
                    result.Result = ApiResponseResult.Error;
                    result.Reason = "User not found!";
                    return result;
                }

                if (!string.IsNullOrEmpty(user.ActivationCode) && !user.ActivationCode.Equals(pConfirmEmailAccount.Token))
                {
                    result.Result = ApiResponseResult.Error;
                    result.Reason = "Invalid token provided!";
                    return result;
                }

                if (user.EmailConfirmed)
                {
                    result.Result = ApiResponseResult.Error;
                    result.Reason = "Your email is already validated successfully. Try to login with the credentials provided or contact Administrator.";
                    return result;
                }

                user.ActivationCode = null;
                user.ActivatedDate = DateTime.Now;
                user.IsActivated = true;
                user.EmailConfirmed = true;

                await UserManager.UpdateAsync(user);
                await UserManager.UpdateSecurityStampAsync(user.Id);
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        [System.Web.Http.HttpGet]
        [AllowAnonymous]
        public string Test()
        {
            return "Sucess";
        }
    }
}