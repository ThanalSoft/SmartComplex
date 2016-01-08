using System;
using System.Threading.Tasks;
using System.Web.Http;
using ThanalSoft.SmartComplex.Business.Complex;
using ThanalSoft.SmartComplex.Common;
using ThanalSoft.SmartComplex.Common.Models.Complex;

namespace ThanalSoft.SmartComplex.Api.Controllers
{
    [RoutePrefix("api/Flat")]
    public class FlatController : BaseSecureApiController
    {
        [HttpGet]
        public async Task<GeneralReturnInfo<FlatInfo>> Get(int id)
        {
            var result = new GeneralReturnInfo<FlatInfo>();
            try
            {
                result.Info = await FlatContext.Instance.Get(id);
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        [HttpGet]
        public async Task<GeneralReturnInfo<FlatInfo[]>> GetAll(int id)
        {
            var result = new GeneralReturnInfo<FlatInfo[]>();
            try
            {
                result.Info = await FlatContext.Instance.GetAll(id);
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        [HttpPost]
        public async Task<GeneralReturnInfo> Create(FlatInfo pApartmentFlatInfo)
        {
            var result = new GeneralReturnInfo();
            try
            {
                await FlatContext.Instance.Create(pApartmentFlatInfo, LoggedInUser);
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