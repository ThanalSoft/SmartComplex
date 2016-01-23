using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ThanalSoft.SmartComplex.Common;
using ThanalSoft.SmartComplex.Common.Models.Account;
using ThanalSoft.SmartComplex.Common.Models.Complex;
using ThanalSoft.SmartComplex.DataAccess;
using ThanalSoft.SmartComplex.DataObjects.Complex;

namespace ThanalSoft.SmartComplex.Business.Complex
{
    public class FlatUserContext : BaseBusiness<FlatUserContext>
    {
        public async Task<ApartmentUserInfo[]> GetAllByApartment(int pApartmentId)
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                var users = await context.FlatUsers
                    .Include(pX => pX.User)
                    .Include(pX => pX.BloodGroup)
                    .Include(pX => pX.MemberFlats)
                    .Include(pX => pX.MemberFlats.Select(pY => pY.Flat))
                    .Include(pX => pX.MemberFlats.Select(pY => pY.Flat.Apartment))
                    .Where(pX => pX.MemberFlats.Any(pY => pY.Flat.Apartment.Id.Equals(pApartmentId))).ToListAsync();

                return users.Select(MapApartmentUserInfo).ToArray();
            }
        }

        public async Task<ApartmentUserInfo> Get(int pUserId)
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                var user = await context.FlatUsers
                    .Include(pX => pX.BloodGroup)
                    .Include(pX => pX.MemberFlats)
                    .Include(pX => pX.MemberFlats.Select(pY => pY.Flat))
                    .Include(pX => pX.MemberFlats.Select(pY => pY.Flat.Apartment))
                    .Where(pX => pX.Id.Equals(pUserId)).FirstAsync();

                return MapApartmentUserInfo(user);
            }
        }

        private ApartmentUserInfo MapApartmentUserInfo(FlatUser pFlatUser)
        {
            return new ApartmentUserInfo
            {
                Id = pFlatUser.Id,
                UserId = pFlatUser.User.Id,
                IsLocked = pFlatUser.IsLocked,
                Email = pFlatUser.Email,
                LockReason = pFlatUser.LockReason,
                LockedDate = pFlatUser.LockedDate,
                FirstName = pFlatUser.FirstName,
                IsOwner = pFlatUser.IsOwner,
                LastName = pFlatUser.LastName,
                Mobile = pFlatUser.Mobile,
                BloodGroup = pFlatUser.BloodGroup?.Group,
                BloodGroupId = pFlatUser.BloodGroupId,
                UserFlats = pFlatUser.MemberFlats.Select(pX => MapToFlatInfo(pX.Flat)).ToArray(),
                ApartmentId = pFlatUser.MemberFlats.First().Flat.ApartmentId,
                ApartmentName = pFlatUser.MemberFlats.First().Flat.Apartment.Name
            };
        }

        private FlatInfo MapToFlatInfo(Flat pFlat)
        {
            var info = new FlatInfo
            {
                Name = pFlat.Name,
                ApartmentId = pFlat.ApartmentId,
                Phase = pFlat.Phase,
                Floor = pFlat.Floor,
                Block = pFlat.Block,
                ExtensionNumber = pFlat.ExtensionNumber,
                SquareFeet = pFlat.SquareFeet,
                Id = pFlat.Id,
                ApartmentName = pFlat.Apartment.Name,
                FlatType = pFlat.FlatType?.Name,
                FlatTypeId = pFlat.FlatTypeId
            };
            return info;
        }

        public async Task<Int64> GetUserId(int pFlatUserId)
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                var users = await context.FlatUsers.Where(pX => pX.Id.Equals(pFlatUserId)).FirstAsync();

                return users.UserId;
            }
        }

        public async Task<UserProfileInfo> GetUserProfile(int pUserId)
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                var user = await context.FlatUsers
                    .Where(pX => pX.UserId.Equals(pUserId)).FirstOrDefaultAsync();

                if (user == null)
                    throw new KeyNotFoundException();

                return new UserProfileInfo
                {
                    LastName = user.LastName,
                    FirstName = user.FirstName,
                    Mobile = user.Mobile,
                    BloodGroupId = user.BloodGroupId,
                    Email = user.Email
                };
            }
        }

        public async Task UpdateUserProfile(UserProfileInfo pUserProfileInfo)
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                var user = await context.FlatUsers
                    .Where(pX => pX.UserId.Equals(pUserProfileInfo.UserId)).FirstOrDefaultAsync();

                if (user == null)
                    throw new KeyNotFoundException();

                user.FirstName = pUserProfileInfo.FirstName;
                user.LastName = pUserProfileInfo.LastName;
                user.Mobile = pUserProfileInfo.Mobile;

                await context.SaveChangesAsync();
            }
        }
    }
}
