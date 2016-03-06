using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ThanalSoft.SmartComplex.Common.Extensions;
using ThanalSoft.SmartComplex.Common.Models.Account;
using ThanalSoft.SmartComplex.DataAccess;

namespace ThanalSoft.SmartComplex.Business.User
{
    public class FlatUserContext : BaseBusiness<FlatUserContext>
    {
        public async Task<UserProfileInfo> GetUserProfile(Int64 pUserId)
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                var user = (await context.Users
                    .Select(pX => new
                    {
                        pX.Email, pX.PhoneNumber, pX.BloodGroupId, pX.FirstName, pX.LastName, pX.Id
                    })
                    .Where(pX => pX.Id.Equals(pUserId)).ToListAsync()).FirstOrDefault();

                if (user == null)
                    throw new KeyNotFoundException();

                return new UserProfileInfo
                {
                    LastName = user.LastName,
                    FirstName = user.FirstName,
                    Mobile = user.PhoneNumber,
                    BloodGroupId = user.BloodGroupId,
                    Email = user.Email,
                    UserId = user.Id
                };
            }
        }

        public async Task<UserProfileWidgetInfo> GetUserProfileWidgetInfo(Int64 pUserId)
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                var user = (await context.Users
                    .Select(pX => new
                    {
                        Email = pX.Email,
                        PhoneNumber = pX.PhoneNumber,
                        BloodGroupId = pX.BloodGroupId,
                        FirstName = pX.FirstName,
                        LastName = pX.LastName,
                        Id = pX.Id
                    })
                    .Where(pX => pX.Id.Equals(pUserId)).ToListAsync())
                    .FirstOrDefault();

                if (user == null)
                    throw new KeyNotFoundException();
                string bloodGroup = null;
                if (user.BloodGroupId != null)
                    bloodGroup = (await context.BloodGroups.FirstOrDefaultAsync(pX => pX.Id == user.BloodGroupId)).Group;

                return new UserProfileWidgetInfo
                {
                    Email = user.Email,
                    Name = user.FirstName + (string.IsNullOrEmpty(user.LastName)
                                                    ? ""
                                                    : " " + user.LastName),
                    BloodGroup = bloodGroup,
                    Mobile = user.PhoneNumber
                };
            }
        }
        
        public async Task UpdateUserProfile(UserProfileInfo pUserProfileInfo, Action pUpdateMobile)
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                var user = await context.Users.Where(pX => pX.Id.Equals(pUserProfileInfo.UserId)).FirstOrDefaultAsync();

                if (user == null)
                    throw new KeyNotFoundException();

                user.FirstName = pUserProfileInfo.FirstName;
                user.LastName = pUserProfileInfo.LastName;
                if (!user.PhoneNumber.Equals(pUserProfileInfo.Mobile))
                    pUpdateMobile();

                user.PhoneNumber = pUserProfileInfo.Mobile;
                user.BloodGroupId = pUserProfileInfo.BloodGroupId;
                
                await context.SaveChangesAsync();
            }
        }
    }
}