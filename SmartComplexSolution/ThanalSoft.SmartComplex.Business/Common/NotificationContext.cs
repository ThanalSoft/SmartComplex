using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ThanalSoft.SmartComplex.Common.Models.Common;
using ThanalSoft.SmartComplex.DataAccess;
using ThanalSoft.SmartComplex.DataObjects.UserUtilities;

namespace ThanalSoft.SmartComplex.Business.Common
{
    public class NotificationContext : BaseBusiness<NotificationContext>
    {
        public async Task<NotificationInfo[]> GetAll(Int64 pUserId)
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                var data = await context.Notifications.Where(pX => pX.TargetUserId == pUserId).OrderByDescending(pX => pX.CreatedDate).Take(5).ToArrayAsync();
                return data.Select(MapToInfo).ToArray();
            }
        }

        private static NotificationInfo MapToInfo(Notification pX)
        {
            return new NotificationInfo
            {
                Id = pX.Id,
                CreatedDate = pX.CreatedDate,
                Message = pX.Message,
                UserReadDate = pX.UserReadDate,
                HasUserRead = pX.HasUserRead,
                TargetUserId = pX.TargetUserId
            };
        }

        public async Task<NotificationInfo[]> Read(Int64 pUserId)
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                var notifications = await context.Notifications.Where(pX => !pX.HasUserRead && pX.TargetUserId == pUserId).ToListAsync();
                foreach (var notification in notifications)
                {
                    notification.HasUserRead = true;
                    notification.UserReadDate = DateTime.Now;
                    notification.LastUpdatedBy = pUserId;
                    notification.LastUpdated = DateTime.Now;
                }
                await context.SaveChangesAsync();

                var data = await context.Notifications.Where(pX => pX.TargetUserId == pUserId).OrderByDescending(pX => pX.CreatedDate).Take(5).ToArrayAsync();
                return data.Select(MapToInfo).ToArray();
            }
        }
    }
}