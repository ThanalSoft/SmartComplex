using System;
using System.Linq;
using System.Threading.Tasks;
using ThanalSoft.SmartComplex.Api.UnitOfWork;
using ThanalSoft.SmartComplex.Common;
using ThanalSoft.SmartComplex.Common.Models.Common;
using ThanalSoft.SmartComplex.Entities.UserUtilities;
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
                var groups = await UnitOfWork.BloodGroups.AllAsync();
                result.Info = groups.Select(pX => new GeneralInfo
                {
                    Name = pX.Group,
                    Id = pX.Id
                }).ToArray();
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
                var states = await UnitOfWork.States.AllAsync();
                result.Info = states.Select(pX => new GeneralInfo
                {
                    Name = pX.Name,
                    Id = pX.Id
                }).ToArray();
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
                var flatTypes = await UnitOfWork.FlatTypes.AllAsync();
                result.Info = flatTypes.Select(pX => new GeneralInfo
                {
                    Name = pX.Name,
                    Id = pX.Id
                }).ToArray();
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        [System.Web.Http.HttpGet]
        public async Task<GeneralReturnInfo<int>> GetUserNotificationCount(Int64 id)
        {
            var result = new GeneralReturnInfo<int>();
            try
            {
                result.Info = await UnitOfWork.Notifications.CountAsync(pX => pX.TargetUserId == id && !pX.HasUserRead);
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        [System.Web.Http.HttpGet]
        public async Task<GeneralReturnInfo<NotificationInfo[]>> GetUserNotifications(Int64 id)
        {
            var result = new GeneralReturnInfo<NotificationInfo[]>();
            try
            {
                var notifications = await UnitOfWork.Notifications.AllAsync(pX => pX.TargetUserId == id);
                result.Info = notifications.OrderByDescending(pX => pX.CreatedDate).Select(MapToInfo).ToArray();
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        [System.Web.Http.HttpGet]
        public async Task<GeneralReturnInfo<NotificationInfo[]>> GetLatestUserNotifications(Int64 id)
        {
            var result = new GeneralReturnInfo<NotificationInfo[]>();
            try
            {
                var notifications = await UnitOfWork.Notifications.AllAsync(pX => pX.TargetUserId == id);
                result.Info = notifications.OrderByDescending(pX => pX.CreatedDate).Take(5).Select(MapToInfo).ToArray();
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        [System.Web.Http.HttpGet]
        public async Task<GeneralReturnInfo<NotificationInfo[]>> ReadUserNotifications(Int64 id)
        {
            var result = new GeneralReturnInfo<NotificationInfo[]>();
            try
            {
                var notifications = await UnitOfWork.Notifications.AllAsync(pX => !pX.HasUserRead && pX.TargetUserId == id);
                foreach (var notification in notifications)
                {
                    notification.HasUserRead = true;
                    notification.UserReadDate = DateTime.Now;
                    notification.LastUpdatedBy = LoggedInUser;
                    notification.LastUpdated = DateTime.Now;
                }
                await UnitOfWork.WorkCompleteAsync();
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        #endregion

        [System.Web.Http.NonAction]
        private NotificationInfo MapToInfo(Notification pNotification)
        {
            return new NotificationInfo
            {
                Id = pNotification.Id,
                CreatedDate = pNotification.CreatedDate,
                Message = pNotification.Message,
                UserReadDate = pNotification.UserReadDate,
                HasUserRead = pNotification.HasUserRead,
                TargetUserId = pNotification.TargetUserId
            };
        }
    }
}