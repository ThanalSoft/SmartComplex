using System;
using System.Threading.Tasks;
using System.Web.Http;
using ThanalSoft.SmartComplex.Api.UnitOfWork;
using ThanalSoft.SmartComplex.Business.Complex;
using ThanalSoft.SmartComplex.Common;
using ThanalSoft.SmartComplex.Common.Exceptions;
using ThanalSoft.SmartComplex.Common.Models.Complex;

namespace ThanalSoft.SmartComplex.Api.Controllers
{
    [RoutePrefix("api/Flat")]
    public class FlatController : BaseSecureController
    {
        public FlatController(IUnitOfWork pUnitOfWork) : base(pUnitOfWork)
        {
        }

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

        //[HttpGet]
        //public async Task<GeneralReturnInfo<FlatInfo[]>> GetUserFlats(string id)
        //{
        //    var result = new GeneralReturnInfo<FlatInfo[]>();
        //    try
        //    {
        //        result.Info = await FlatContext.Instance.GetUserFlats(Convert.ToInt64(id));
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Result = ApiResponseResult.Error;
        //        result.Reason = ex.Message;
        //    }
        //    return result;
        //}

        [HttpPost]
        public async Task<GeneralReturnInfo> Create(FlatInfo pApartmentFlatInfo)
        {
            var result = new GeneralReturnInfo();
            try
            {
                await FlatContext.Instance.Create(pApartmentFlatInfo, LoggedInUser);
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
        public async Task<GeneralReturnInfo> Update(FlatInfo pApartmentFlatInfo)
        {
            var result = new GeneralReturnInfo();
            try
            {
                await FlatContext.Instance.Update(pApartmentFlatInfo, LoggedInUser);
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

    }
}