using System;
using System.Threading.Tasks;
using System.Web.Http;
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
                await ApartmentContext.Instance.UploadFlatsAsync(pApartmentFlatInfoList, LoggedInUser);
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }
    }
}