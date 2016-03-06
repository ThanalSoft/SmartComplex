using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ThanalSoft.SmartComplex.Business.Repositories;
using ThanalSoft.SmartComplex.Common.Models.Complex;
using ThanalSoft.SmartComplex.DataAccess;
using ThanalSoft.SmartComplex.Entities.Complex;
using ThanalSoft.SmartComplex.Entities.Security;

namespace ThanalSoft.SmartComplex.Business.Complex
{
    public class ApartmentRepository : RepositoryService<Apartment>, IApartmentRepository
    {
        public ApartmentRepository(SmartComplexDataObjectContext pContext) : base(pContext)
        {

        }

        public async Task<IEnumerable<ApartmentUserInfo>> GetAllApartmentUsersAsync(int pApartmentId)
        {
            var result = await Context.MemberFlats
                .Include(pX => pX.User)
                .Include(pX => pX.Apartment)
                .Where(pX => pX.ApartmentId.Equals(pApartmentId)).ToListAsync();

            return result.Select(MapApartmentUserInfo);
        }

        public async Task<ApartmentUserInfo> GetApartmentUserAsync(int pApartmentId)
        {
            var result = await Context.MemberFlats.FirstOrDefaultAsync(pX => pX.ApartmentId.Equals(pApartmentId));

            return MapApartmentUserInfo(result);
        }

        public async Task<ApartmentInfo[]> GetUserApartmentsAsync(Int64 pUserId)
        {
            var result = await Context.MemberFlats.Where(pX => pX.UserId.Equals(pUserId)).ToListAsync();
            return result.Select(pX => MapToApartmentInfo(pX.Apartment)).ToArray();
        }

        private ApartmentUserInfo MapApartmentUserInfo(MemberFlat pMemberFlat)
        {
            return new ApartmentUserInfo
            {
                Id = pMemberFlat.Id,
                UserId = pMemberFlat.UserId,
                IsLocked = pMemberFlat.User.IsFreezed,
                Email = pMemberFlat.User.Email,
                LockReason = pMemberFlat.User.ReasonForFreeze,
                LockedDate = pMemberFlat.User.FreezedDate,
                FirstName = pMemberFlat.User.FirstName,
                IsOwner = pMemberFlat.IsOwner,
                LastName = pMemberFlat.User.LastName,
                Mobile = pMemberFlat.User.PhoneNumber,
                BloodGroup = pMemberFlat.User.BloodGroup?.Group,
                BloodGroupId = pMemberFlat.User.BloodGroupId,
                ApartmentId = pMemberFlat.ApartmentId,
                ApartmentName = pMemberFlat.Apartment.Name,
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

        private ApartmentInfo MapToApartmentInfo(Apartment pApartment)
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
                State = pApartment.State?.Name
            };
        }
    }
}