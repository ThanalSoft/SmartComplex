using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using ThanalSoft.SmartComplex.Api.UnitOfWork;
using ThanalSoft.SmartComplex.Business.Common;
using ThanalSoft.SmartComplex.Common;
using ThanalSoft.SmartComplex.Common.Models.Common;
using WebApi.OutputCache.V2;

namespace ThanalSoft.SmartComplex.Api.Controllers
{
    [System.Web.Http.RoutePrefix("api/Common")]
    public class CommonController : BaseSecureController
    {
        public CommonController(IUnitOfWork pUnitOfWork) : base(pUnitOfWork)
        {
        }

        #region Get Methods

        [System.Web.Http.HttpGet]
        [CacheOutput(ClientTimeSpan = 99999999, ServerTimeSpan = 99999999)]
        public async Task<GeneralReturnInfo<GeneralInfo[]>> GetBloodGroups()
        {
            var result = new GeneralReturnInfo<GeneralInfo[]>();
            try
            {
                result.Info = await BloodGroupContext.Instance.GetBloodGroupsAsync();
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        [System.Web.Http.HttpGet]
        [CacheOutput(ClientTimeSpan = 99999999, ServerTimeSpan = 99999999)]
        public async Task<GeneralReturnInfo<GeneralInfo[]>> GetStates()
        {
            var result = new GeneralReturnInfo<GeneralInfo[]>();
            try
            {
                result.Info = await StateContext.Instance.GetStatesAsync();
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        [System.Web.Http.HttpGet]
        [CacheOutput(ClientTimeSpan = 99999999, ServerTimeSpan = 99999999)]
        public async Task<GeneralReturnInfo<GeneralInfo[]>> GetFlatTypes()
        {
            var result = new GeneralReturnInfo<GeneralInfo[]>();
            try
            {
                result.Info = await FlatTypeContext.Instance.GetFlatTypesAsync();
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        [System.Web.Http.HttpGet]
        public async Task<GeneralReturnInfo<int>> GetUserNotificationCount(int id)
        {
            var result = new GeneralReturnInfo<int>();
            try
            {
                result.Info = await NotificationContext.Instance.GetCount(id);
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        [System.Web.Http.HttpGet]
        public async Task<GeneralReturnInfo<NotificationInfo[]>> GetUserNotifications(int id)
        {
            var result = new GeneralReturnInfo<NotificationInfo[]>();
            try
            {
                result.Info = await NotificationContext.Instance.GetAll(id);
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        [System.Web.Http.HttpGet]
        public async Task<GeneralReturnInfo<NotificationInfo[]>> GetLatestUserNotifications(int id)
        {
            var result = new GeneralReturnInfo<NotificationInfo[]>();
            try
            {
                result.Info = await NotificationContext.Instance.GetLatest(id);
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        [System.Web.Http.HttpGet]
        public async Task<GeneralReturnInfo<NotificationInfo[]>> ReadUserNotifications(int id)
        {
            var result = new GeneralReturnInfo<NotificationInfo[]>();
            try
            {
                result.Info = await NotificationContext.Instance.Read(id);
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        #endregion
        
    }
}