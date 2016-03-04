using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using ThanalSoft.SmartComplex.Api.UnitOfWork;
using ThanalSoft.SmartComplex.Business.Complex;
using ThanalSoft.SmartComplex.Common;
using ThanalSoft.SmartComplex.Common.Extensions;
using ThanalSoft.SmartComplex.Common.Models.Account;
using WebApi.OutputCache.V2;
using FlatUserContext = ThanalSoft.SmartComplex.Business.User.FlatUserContext;

namespace ThanalSoft.SmartComplex.Api.Controllers
{
    [System.Web.Http.RoutePrefix("api/Account")]
    public class AccountController : BaseSecureController
    {
        public AccountController(IUnitOfWork pUnitOfWork) : base(pUnitOfWork)
        {
            
        }

        #region Get Methods

        [System.Web.Http.HttpGet]
        public async Task<GeneralReturnInfo<UserProfileInfo>> GetUserProfileDetails(string id)
        {
            var result = new GeneralReturnInfo<UserProfileInfo>();
            try
            {
                result.Info = await FlatUserContext.Instance.GetUserProfile(Convert.ToInt64(id));
            }
            catch (KeyNotFoundException ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        [System.Web.Http.HttpGet]
        [CacheOutput(ClientTimeSpan = 600, ServerTimeSpan = 600)]
        public async Task<GeneralReturnInfo<UserProfileWidgetInfo>> GetUserProfileWidgetInfo(string id)
        {
            var result = new GeneralReturnInfo<UserProfileWidgetInfo>();
            try
            {
                result.Info = await FlatUserContext.Instance.GetUserProfileWidgetInfo(Convert.ToInt64(id));
            }
            catch (KeyNotFoundException ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
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

        #endregion

        #region Post Methods

        [System.Web.Http.HttpPost]
        [System.Web.Http.AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<LoginResultInfo> SecureLogin(LoginRequestInfo pLogin)
        {
            var user = await UserManager.FindByEmailAsync(pLogin.Email);
            if (user == null)
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
            if (user.IsFreezed)
            {
                return new LoginResultInfo
                {
                    LoginStatus = LoginStatus.LockedOut
                };
            }
            var status = await SignInManager.PasswordSignInAsync(pLogin.Email, pLogin.Password, true, true);
            switch (status)
            {
                case SignInStatus.Success:

                    var userInfo = new LoginUserInfo
                    {
                        Email = pLogin.Email,
                        UserName = user.UserName,
                        UserId = user.Id,
                        Roles = (await UserManager.GetRolesAsync(user.Id)).ToArray(),
                        Name = user.UserFullName()
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

        [HttpPost]
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

        [System.Web.Http.HttpPost]
        public async Task<GeneralReturnInfo> UpdateUserProfile(UserProfileInfo pUserProfileInfo)
        {
            var result = new GeneralReturnInfo();
            try
            {
                await FlatUserContext.Instance.UpdateUserProfile(pUserProfileInfo, async () => await ChangePhonenumber(pUserProfileInfo));
            }
            catch (KeyNotFoundException ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }
        
        [System.Web.Http.HttpPost]
        public async Task<GeneralReturnInfo> UpdateCredentials(UserProfileInfo pUserProfileInfo)
        {
            var result = new GeneralReturnInfo();
            try
            {
                var user = await UserManager.FindByIdAsync(pUserProfileInfo.UserId);
                if (user == null)
                    throw new KeyNotFoundException();

                var passwordChecked = await UserManager.CheckPasswordAsync(user, pUserProfileInfo.Password);
                if (!passwordChecked)
                    throw new Exception("Invalid password provided.");

                var changeResult = await UserManager.ChangePasswordAsync(pUserProfileInfo.UserId, pUserProfileInfo.Password, pUserProfileInfo.NewPassword);
                if (!changeResult.Succeeded)
                {
                    result.Result = ApiResponseResult.Error;
                    result.Reason = changeResult.Errors.First();
                }
            }
            catch (KeyNotFoundException ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        #endregion

        #region Private Methods

        private async Task ChangePhonenumber(UserProfileInfo pUserProfileInfo)
        {
            var token = await UserManager.GenerateChangePhoneNumberTokenAsync(pUserProfileInfo.UserId, pUserProfileInfo.Mobile);
            await UserManager.ChangePhoneNumberAsync(pUserProfileInfo.UserId, pUserProfileInfo.Mobile, token);
        }
        
        #endregion

    }
}