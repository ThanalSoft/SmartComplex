using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using ThanalSoft.SmartComplex.Api.UnitOfWork;
using ThanalSoft.SmartComplex.Common;
using ThanalSoft.SmartComplex.Common.Extensions;
using ThanalSoft.SmartComplex.Common.Models.Account;
using WebApi.OutputCache.V2;

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
                var user = await UnitOfWork.Users.FindAsync(Convert.ToInt64(id));
                if(user == null)
                    throw new KeyNotFoundException();

                result.Info = new UserProfileInfo
                {
                    LastName = user.LastName,
                    FirstName = user.FirstName,
                    Mobile = user.PhoneNumber,
                    BloodGroupId = user.BloodGroupId,
                    Email = user.Email,
                    UserId = user.Id
                };
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
                var user = await UnitOfWork.Users.FindAsync(Convert.ToInt64(id));
                if (user == null)
                    throw new KeyNotFoundException();

                string bloodGroup = null;
                if (user.BloodGroupId != null)
                    bloodGroup = (await UnitOfWork.BloodGroups.FindAsync(user.BloodGroupId)).Group;

                result.Info = new UserProfileWidgetInfo
                {
                    Email = user.Email,
                    Name = user.FirstName + (string.IsNullOrEmpty(user.LastName)
                                                    ? ""
                                                    : " " + user.LastName),
                    BloodGroup = bloodGroup,
                    Mobile = user.PhoneNumber

                };
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
                var user = await UnitOfWork.Users.FindAsync(pUserProfileInfo.UserId);
                if(user == null)
                    throw new KeyNotFoundException();

                user.FirstName = pUserProfileInfo.FirstName;
                user.LastName = pUserProfileInfo.LastName;
                user.BloodGroupId = pUserProfileInfo.BloodGroupId;

                if (!user.PhoneNumber.Equals(pUserProfileInfo.Mobile))
                    await ChangePhonenumber(pUserProfileInfo);

                user.PhoneNumber = pUserProfileInfo.Mobile;
                
                await UnitOfWork.WorkCompleteAsync();
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