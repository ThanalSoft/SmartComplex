using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using ThanalSoft.SmartComplex.Api.UnitOfWork;
using ThanalSoft.SmartComplex.Business.Complex;
using ThanalSoft.SmartComplex.Common;
using ThanalSoft.SmartComplex.Common.Exceptions;
using ThanalSoft.SmartComplex.Common.Models.Complex;
using ThanalSoft.SmartComplex.Entities.Security;

namespace ThanalSoft.SmartComplex.Api.Controllers
{
    [RoutePrefix("api/Apartment")]
    public class ApartmentController : BaseSecureController
    {
        public ApartmentController(IUnitOfWork pUnitOfWork) : base(pUnitOfWork)
        {
        }

        #region Get Methods

        [HttpGet]
        public async Task<GeneralReturnInfo<ApartmentInfo[]>> GetAll()
        {
            var result = new GeneralReturnInfo<ApartmentInfo[]>();
            try
            {
                result.Info = await ApartmentContext.Instance.GetAllAsync();
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        [HttpGet]
        public async Task<GeneralReturnInfo<ApartmentInfo>> Get(string id)
        {
            var result = new GeneralReturnInfo<ApartmentInfo>();
            try
            {
                result.Info = await ApartmentContext.Instance.GetAsync(Convert.ToInt32(id));
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        //[HttpGet]
        //public async Task<GeneralReturnInfo<ApartmentUserInfo[]>> GetApartmentUsers(string id)
        //{
        //    var result = new GeneralReturnInfo<ApartmentUserInfo[]>();
        //    try
        //    {
        //        result.Info = await FlatUserContext.Instance.GetAllByApartment(Convert.ToInt32(id));
        //        foreach (var apartmentUserInfo in result.Info)
        //        {
        //            var roles = await UserManager.GetRolesAsync(apartmentUserInfo.UserId);
        //            apartmentUserInfo.UserRoles = string.Join(", ", roles);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Result = ApiResponseResult.Error;
        //        result.Reason = ex.Message;
        //    }
        //    return result;
        //}

        //[HttpGet]
        //public async Task<GeneralReturnInfo<ApartmentUserInfo>> GetApartmentUser(string id)
        //{
        //    var result = new GeneralReturnInfo<ApartmentUserInfo>();
        //    try
        //    {
        //        result.Info = await FlatUserContext.Instance.Get(Convert.ToInt32(id));
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Result = ApiResponseResult.Error;
        //        result.Reason = ex.Message;
        //    }
        //    return result;
        //}

        //[HttpGet]
        //public async Task<GeneralReturnInfo> MarkUserAdmin(string id)
        //{
        //    var result = new GeneralReturnInfo();
        //    try
        //    {
        //        var userid = await FlatUserContext.Instance.GetUserId(Convert.ToInt32(id));
        //        if (await UserManager.IsInRoleAsync(userid, "ApartmentAdmin"))
        //            await UserManager.RemoveFromRoleAsync(userid, "ApartmentAdmin");
        //        else
        //            await UserManager.AddToRoleAsync(userid, "ApartmentAdmin");
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Result = ApiResponseResult.Error;
        //        result.Reason = ex.Message;
        //    }
        //    return result;
        //}

        //public async Task<GeneralReturnInfo<ApartmentInfo[]>> GetUserApartments(string id)
        //{
        //    var result = new GeneralReturnInfo<ApartmentInfo[]>();
        //    try
        //    {
        //        result.Info = await ApartmentContext.Instance.GetUserApartments(Convert.ToInt64(id));
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Result = ApiResponseResult.Error;
        //        result.Reason = ex.Message;
        //    }
        //    return result;
        //}

        #endregion

        #region Post Methods

        [HttpPost]
        public async Task<GeneralReturnInfo> Create([FromBody]ApartmentInfo pApartmentInfo)
        {
            var result = new GeneralReturnInfo();
            try
            {
                await ApartmentContext.Instance.CreateAsync(pApartmentInfo, LoggedInUser);
            }
            catch (ItemAlreadyExistsException ex)
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

        [HttpPost]
        public async Task<GeneralReturnInfo> Update([FromBody]ApartmentInfo pApartmentInfo)
        {
            var result = new GeneralReturnInfo();
            try
            {
                await ApartmentContext.Instance.UpdateAsync(pApartmentInfo, LoggedInUser);
            }
            catch (ItemAlreadyExistsException ex)
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

        [HttpPost]
        public async Task<GeneralReturnInfo> DeleteUndelete([FromBody] int id)
        {
            var result = new GeneralReturnInfo();
            try
            {
                await ApartmentContext.Instance.DeleteUndeleteAsync(id, LoggedInUser);
            }
            catch (ItemAlreadyExistsException ex)
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

        [HttpPost]
        public async Task<GeneralReturnInfo> LockUnlock([FromBody]ApartmentInfo pApartmentInfo)
        {
            var result = new GeneralReturnInfo();
            try
            {
                await ApartmentContext.Instance.LockUnlockAsync(pApartmentInfo, LoggedInUser);
            }
            catch (ItemAlreadyExistsException ex)
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

        [HttpPost]
        public async Task<GeneralReturnInfo> UploadFlats(FlatUploadInfo[] pApartmentFlatInfoList)
        {
            var result = new GeneralReturnInfo();
            try
            {
                await ApartmentContext.Instance.UploadFlatsAsync(pApartmentFlatInfoList, LoggedInUser, ConfigureUser);
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

        [NonAction]
        private void ConfigureUser(FlatUploadInfo pUser, string pPassword, string pActivationCode)
        {
            if (string.IsNullOrEmpty(pUser.OwnerEmail))
                return;

            var user = UserManager.FindByEmail(pUser.OwnerEmail);
            
            AddOwnerRole(user);

            if (!string.IsNullOrEmpty(user.ActivationCode) && !user.IsActivated)
                SendUserEmail(pUser.OwnerEmail, pPassword, pActivationCode, user);
        }

        [NonAction]
        private void AddOwnerRole(LoginUser pUser)
        {
            UserManager.AddToRole(pUser.Id, "Owner");
        }

        [NonAction]
        private void SendUserEmail(string pEmail, string pPassword, string pActivationCode, LoginUser pUser)
        {
            var url = $"{ConfigurationManager.AppSettings["WEB_URL"]}/{"Account"}/{"ConfirmEmail"}/{pUser.Id}/?token={pActivationCode}";
            UserManager.SendEmail(pUser.Id, "Welcome to Smart Complex!", GetBody(url, pEmail, pPassword));
        }

        [NonAction]
        private string GetBody(string pUrl, string pEmail, string pPassword)
        {
            string content = $"<div style='color:#666 !important;'>Hi,<div><u><b><font size='3'><br></font></b></u></div><div><b><font size='3' face='Lucida Sans'>Thanks for choosing SmartComplex! Now leave SMARTLY in your COMPLEX.</font></b></div><div><br><br></div><div>Please click <strong style='font-size:135%;color:#49A6FD !important;'><a href='{ pUrl}'>here</a></strong> to confirm your email. Once the email validation is completed use the below credentials to login to your account.</div><div><br></div><div style='color:#000 !important;'><font face='Arial Black'>Username :&nbsp;{pEmail}</font></div><div style='color:#000 !important;'><font face='Arial Black'>Password &nbsp;:&nbsp;{pPassword}</font></div><div><br><br></div><div><b>Thank You!</b></div></div>";
            return content;
        }

        #endregion
        
    }
}