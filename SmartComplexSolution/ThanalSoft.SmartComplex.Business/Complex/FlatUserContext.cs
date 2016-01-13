using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
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
                ApartmentName = pFlatUser.MemberFlats.First().Flat.Apartment.Name,
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
    }
}
