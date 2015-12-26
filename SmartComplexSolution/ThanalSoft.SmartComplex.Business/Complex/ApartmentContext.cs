using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using ThanalSoft.SmartComplex.Common.Models.Complex;
using ThanalSoft.SmartComplex.DataAccess;
using ThanalSoft.SmartComplex.DataObjects.Complex;

namespace ThanalSoft.SmartComplex.Business.Complex
{
    public class ApartmentContext : BaseBusiness<ApartmentContext>
    {
        public async Task<ApartmentInfo[]> GetAllAsync()
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                var data = await context.Apartments.ToArrayAsync();
                return data.Select(MapToInfo).ToArray();
            }
        }

        public async Task<ApartmentInfo> GetAsync(int pApartmentId)
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                var data = await context.Apartments.FirstAsync(pX => pX.Id.Equals(pApartmentId));
                return MapToInfo(data);
            }
        }

        public async Task CreateAsync(ApartmentInfo pApartmentInfo, Int64 pUserId)
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                context.Apartments.AddOrUpdate(new Apartment
                {
                    Phone = pApartmentInfo.Phone,
                    StateId = 1,
                    Name = pApartmentInfo.Name,
                    Address = pApartmentInfo.Address,
                    City = pApartmentInfo.City,
                    IsDeleted = false,
                    IsLocked = false,
                    LastUpdated = DateTime.Now,
                    LastUpdatedBy = pUserId,
                    PinCode = pApartmentInfo.PinCode
                });

                await context.SaveChangesAsync();
            }
        }

        private static ApartmentInfo MapToInfo(Apartment pApartment)
        {
            return new ApartmentInfo
            {
                Name = pApartment.Name,
                Id = pApartment.Id,
                Address = pApartment.Address,
                City = pApartment.City,
                IsDeleted = pApartment.IsDeleted,
                IsLocked = pApartment.IsLocked,
                LockReason = pApartment.LockReason,
                LockedDate = pApartment.LockedDate,
                Phone = pApartment.Phone,
                PinCode = pApartment.PinCode,
                StateId = pApartment.StateId
            };
        }
    }
}