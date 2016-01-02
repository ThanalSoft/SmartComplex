using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using ThanalSoft.SmartComplex.Business.Complex;
using ThanalSoft.SmartComplex.Common;
using ThanalSoft.SmartComplex.Common.Exceptions;
using ThanalSoft.SmartComplex.Common.Models.Complex;

namespace ThanalSoft.SmartComplex.Api.Controllers
{
    [RoutePrefix("api/Apartment")]
    public class ApartmentController : BaseSecureApiController
    {
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

        [HttpGet]
        public async Task<GeneralReturnInfo<ApartmentFlatInfo>> GetFlat(int id)
        {
            var result = new GeneralReturnInfo<ApartmentFlatInfo>();
            try
            {
                result.Info = await ApartmentContext.Instance.GetFlatAsync(id);
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        [HttpGet]
        public async Task<GeneralReturnInfo<ApartmentFlatInfo[]>> GetApartmentFlats(int id)
        {
            var result = new GeneralReturnInfo<ApartmentFlatInfo[]>();
            try
            {
                result.Info = await ApartmentContext.Instance.GetFlatsAsync(id);
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        [HttpPost]
        public async Task<GeneralReturnInfo> CreateFlat(ApartmentFlatInfo pApartmentFlatInfo)
        {
            var result = new GeneralReturnInfo();
            try
            {
                await ApartmentContext.Instance.CreateFlatAsync(pApartmentFlatInfo, LoggedInUser);
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        [HttpPost]
        public async Task<GeneralReturnInfo> UploadFlats(ApartmentFlatInfo[] pApartmentFlatInfoList)
        {
            var result = new GeneralReturnInfo();
            try
            {
                await ApartmentContext.Instance.UploadFlatsAsync(pApartmentFlatInfoList, LoggedInUser, SendEmail);
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<GeneralReturnInfo> ConfirmUserEmail(string id, string token)
        {
            if(string.IsNullOrEmpty(id))
                throw new Exception("Invalid User!");
            if (string.IsNullOrEmpty(token))
                throw new Exception("Invalid Token!");

            var result = new GeneralReturnInfo();
            try
            {
                var user = await UserManager.FindByIdAsync(Convert.ToInt64(id));
                if(user == null)
                    throw new Exception("User not found!");

                if(!user.ActivationCode.Equals(token))
                    throw new Exception("Invalid user token");

                user.ActivationCode = null;
                user.ActivatedDate = DateTime.Now;
                user.IsActivated = true;
                user.EmailConfirmed = true;
                await UserManager.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        private void SendEmail(string pEmail, string pPassword, string pActivationCode)
        {
            var user = UserManager.FindByEmail(pEmail);
            var url = this.Url.Link("DefaultApi", new { Controller = "Apartment", Action = "ConfirmUserEmail", id = user.Id, token = pActivationCode });
            UserManager.SendEmail(user.Id, "Welcome to Smart Complex!", GetBody(url, pEmail, pPassword));
        }

        private string GetBody(string pUrl, string pEmail, string pPassword)
        {
            string content = $"Hi,<div><u><b><font size='3'><br></font></b></u></div><div><u><b><font size='3' face='Lucida Sans'>Thanks for choosing SmartComplex! Now leave in your complex SMARTLY.</font></b></u></div><div><br></div><div>Please click <a href='{pUrl}'>here</a> to confirm your email. Once the email validation is completed use the following credentials to login to your account.</div><div><br></div><div><font face='Arial Black'>Username :&nbsp;{pEmail}</font></div><div><font face='Arial Black'>Password :&nbsp;{pPassword}</font></div><div><br></div><div>Thank You!</div>";
            return content;
        }
    }
}