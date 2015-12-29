using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ThanalSoft.SmartComplex.Common.Exceptions;
using ThanalSoft.SmartComplex.Common.Models.Complex;
using ThanalSoft.SmartComplex.DataAccess;
using ThanalSoft.SmartComplex.DataObjects.Common;
using ThanalSoft.SmartComplex.DataObjects.Complex;

namespace ThanalSoft.SmartComplex.Business.Complex
{
    public class ApartmentContext : BaseBusiness<ApartmentContext>
    {
        public async Task<ApartmentInfo[]> GetAllAsync()
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                var data = await context.Apartments.OrderByDescending(pX => pX.CreatedDate).ToArrayAsync();
                return data.Select(pX => MapToInfo(pX, context.States.Find(pX.StateId))).ToArray();
            }
        }

        public async Task<ApartmentInfo> GetAsync(int pApartmentId)
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                var data = await context.Apartments.FirstAsync(pX => pX.Id.Equals(pApartmentId));
                return MapToInfo(data, context.States.Find(data.StateId));
            }
        }

        public async Task CreateAsync(ApartmentInfo pApartmentInfo, Int64 pUserId)
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                if (await context.Apartments.AnyAsync(pX => pX.Name.Equals(pApartmentInfo.Name, StringComparison.OrdinalIgnoreCase)))
                    throw new ItemAlreadyExistsException();

                context.Apartments.Add(new Apartment
                {
                    Phone = pApartmentInfo.Phone,
                    StateId = pApartmentInfo.StateId,
                    Name = pApartmentInfo.Name,
                    Address = pApartmentInfo.Address,
                    City = pApartmentInfo.City,
                    IsDeleted = false,
                    IsLocked = false,
                    LastUpdated = DateTime.Now,
                    LastUpdatedBy = pUserId,
                    PinCode = pApartmentInfo.PinCode,
                    CreatedDate = DateTime.Now
                });

                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(ApartmentInfo pApartmentInfo, Int64 pUserId)
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                var original = await context.Apartments.FindAsync(pApartmentInfo.Id);

                if (await context.Apartments.AnyAsync(pX => pX.Name.Equals(pApartmentInfo.Name, StringComparison.OrdinalIgnoreCase) && pX.Id != original.Id))
                    throw new ItemAlreadyExistsException();

                original.Phone = pApartmentInfo.Phone;
                original.StateId = pApartmentInfo.StateId;
                original.Name = pApartmentInfo.Name;
                original.Address = pApartmentInfo.Address;
                original.City = pApartmentInfo.City;
                original.LastUpdated = DateTime.Now;
                original.LastUpdatedBy = pUserId;
                original.PinCode = pApartmentInfo.PinCode;
                await context.SaveChangesAsync();
            }
        }

        private static ApartmentInfo MapToInfo(Apartment pApartment, State pState)
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
                StateId = pApartment.StateId,
                CreatedDate = pApartment.CreatedDate,
                State = pState.Name
            };
        }

        public async Task DeleteUndeleteAsync(int pId, Int64 pUserId)
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                var original = await context.Apartments.FindAsync(pId);

                original.IsDeleted = !original.IsDeleted;
                original.LastUpdated = DateTime.Now;
                original.LastUpdatedBy = pUserId;
               
                await context.SaveChangesAsync();
            }
        }

        public async Task LockUnlockAsync(ApartmentInfo pApartmentInfo, long pUserId)
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                var original = await context.Apartments.FindAsync(pApartmentInfo.Id);

                original.IsLocked = !original.IsLocked;
                original.LockedDate = !original.IsLocked ? (DateTime?) null : DateTime.Now;
                original.LockReason = !original.IsLocked ? null : pApartmentInfo.LockReason;
                original.LastUpdated = DateTime.Now;
                original.LastUpdatedBy = pUserId;
                await context.SaveChangesAsync();
            }
        }
    }
}