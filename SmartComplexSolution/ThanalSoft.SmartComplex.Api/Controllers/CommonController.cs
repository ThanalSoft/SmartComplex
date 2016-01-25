using System;
using System.Threading.Tasks;
using System.Web.Http;
using ThanalSoft.SmartComplex.Business.Common;
using ThanalSoft.SmartComplex.Common;
using ThanalSoft.SmartComplex.Common.Models.Common;

namespace ThanalSoft.SmartComplex.Api.Controllers
{
    [RoutePrefix("api/Common")]
    public class CommonController : BaseSecureController
    {
        [HttpGet]
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

        [HttpGet]
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

        [HttpGet]
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

        [HttpGet]
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

        [HttpGet]
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

        [HttpGet]
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

        [HttpGet]
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
    }
}