using System;
using ThanalSoft.SmartComplex.Entities.UserUtilities;

namespace ThanalSoft.SmartComplex.Business.Common
{
    public static class UserNotificationsHelper
    {
        public static Notification GetNotification(Int64 pTargetUser, string pMessage, Int64 pLoggedInUser)
        {
            return new Notification
            {
                CreatedDate = DateTime.Now,
                HasUserRead = false,
                LastUpdated = DateTime.Now,
                LastUpdatedBy = pLoggedInUser,
                Message = pMessage,
                TargetUserId = pTargetUser,
                UserReadDate = null
            };
        }
    }
}