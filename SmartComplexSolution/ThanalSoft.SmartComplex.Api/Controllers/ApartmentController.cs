using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using ThanalSoft.SmartComplex.Business.Complex;
using ThanalSoft.SmartComplex.Common;
using ThanalSoft.SmartComplex.Common.Exceptions;
using ThanalSoft.SmartComplex.Common.Models.Complex;
using ThanalSoft.SmartComplex.DataObjects.Security;

namespace ThanalSoft.SmartComplex.Api.Controllers
{
    [RoutePrefix("api/Apartment")]
    public class ApartmentController : BaseSecureApiController
    {
        private readonly PasswordHasher _passwordHasher = new PasswordHasher();

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

        private void ConfigureUser(FlatUploadInfo pUser, string pPassword, string pActivationCode)
        {
            if (string.IsNullOrEmpty(pUser.OwnerEmail))
                return;

            var user = UserManager.FindByEmail(pUser.OwnerEmail);
            AddOwnerRole(user);
            
            pPassword = _passwordHasher.HashPassword(pPassword);
            if (!string.IsNullOrEmpty(user.ActivationCode) && !user.IsActivated)
                SendUserEmail(pUser.OwnerEmail, pPassword, pActivationCode, user);
        }
        
        private void AddOwnerRole(User pUser)
        {
            UserManager.AddToRole(pUser.Id, "Owner");
        }

        private void SendUserEmail(string pEmail, string pPassword, string pActivationCode, User pUser)
        {
            var url = $"{ConfigurationManager.AppSettings["WEB_URL"]}/{"Account"}/{"ConfirmEmail"}/{pUser.Id}/?token={pActivationCode}";
            UserManager.SendEmail(pUser.Id, "Welcome to Smart Complex!", GetBody(url, pEmail, pPassword));
        }

        private string GetBody(string pUrl, string pEmail, string pPassword)
        {
            string content = $"<div style='color:#666 !important;'>Hi,<div><u><b><font size='3'><br></font></b></u></div><div><u><b><font size='3' face='Lucida Sans'>Thanks for choosing SmartComplex! Now leave in your complex SMARTLY.</font></b></u></div><div><br></div><div>Please click <strong style='font-size:125%;color:#49A6FD !important;'><a href='{ pUrl}'>here</a></strong> to confirm your email. Once the email validation is completed use the following credentials to login to your account.</div><div><br></div><div style='color:#000 !important;'><font face='Arial Black'>Username :&nbsp;{pEmail}</font></div><div><font face='Arial Black'>Password :&nbsp;{pPassword}</font></div><div><br></div><div>Thank You!</div></div>";
            return content;
        }
    }
}