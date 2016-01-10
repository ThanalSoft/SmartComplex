using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ThanalSoft.SmartComplex.Common.Exceptions;
using ThanalSoft.SmartComplex.Common.Models.Complex;
using ThanalSoft.SmartComplex.DataAccess;
using ThanalSoft.SmartComplex.DataObjects.Complex;

namespace ThanalSoft.SmartComplex.Business.Complex
{
    public class FlatContext : BaseBusiness<FlatContext>
    {
        public async Task<FlatInfo[]> GetAll(int pApartmentId)
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                var flats = await context.Flats
                    .Include(pX => pX.Apartment)
                    .Where(pX => pX.ApartmentId.Equals(pApartmentId)).ToListAsync();
                return flats.Select(MapToFlatInfo).ToArray();
            }
        }

        public async Task<FlatInfo> Get(int pFlatId)
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                var flatInfo = await context.Flats
                    .Include(pX => pX.Apartment)
                    .Where(pX => pX.Id.Equals(pFlatId)).FirstAsync();
                return MapToFlatInfo(flatInfo);
            }
        }

        public async Task Create(FlatInfo pApartmentFlatInfo, Int64 pUserId)
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                if(await context.Flats.AnyAsync(pX => pX.ApartmentId.Equals(pApartmentFlatInfo.ApartmentId) && pX.Name.Equals(pApartmentFlatInfo.Name)))
                    throw new ItemAlreadyExistsException(pApartmentFlatInfo.Name);

                var flat = AddFlat(pApartmentFlatInfo, pUserId);
                context.Flats.Add(flat);

                await context.SaveChangesAsync();
            }
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
                ApartmentName = pFlat.Apartment.Name
            };
            return info;
        }

        private Flat AddFlat(FlatInfo pApartmentFlatInfo, Int64 pUserId)
        {
            return new Flat
            {
                ApartmentId = pApartmentFlatInfo.ApartmentId,
                Block = pApartmentFlatInfo.Block,
                ExtensionNumber = null,
                Floor = pApartmentFlatInfo.Floor,
                Name = pApartmentFlatInfo.Name,
                Phase = pApartmentFlatInfo.Phase,
                SquareFeet = null,
                LastUpdated = DateTime.Now,
                LastUpdatedBy = pUserId,
            };
        }

        private FlatUserInfo MapToFlatUserInfo(FlatUser pFlatUser)
        {
            return new FlatUserInfo
            {
                FirstName = pFlatUser.FirstName,
                LastName = pFlatUser.LastName,
                IsLocked = pFlatUser.IsLocked,
                LockReason = pFlatUser.LockReason,
                IsDeleted = pFlatUser.IsDeleted,
                Email = pFlatUser.User.Email,
                EmailConfirmed = pFlatUser.User.EmailConfirmed,
                IsOwner = pFlatUser.IsOwner,
                LockedDate = pFlatUser.LockedDate,
                Mobile = pFlatUser.Mobile,
                IsActivated = pFlatUser.User.IsActivated,
                PhoneNumber = pFlatUser.User.PhoneNumber,

            };
        }
    }
}