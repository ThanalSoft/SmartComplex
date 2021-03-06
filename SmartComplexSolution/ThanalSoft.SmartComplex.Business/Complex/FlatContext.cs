﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ThanalSoft.SmartComplex.Common.Exceptions;
using ThanalSoft.SmartComplex.Common.Models.Complex;
using ThanalSoft.SmartComplex.DataAccess;
using ThanalSoft.SmartComplex.Entities.Complex;

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
                    .Include(pX => pX.FlatType)
                    .Where(pX => pX.ApartmentId.Equals(pApartmentId)).ToListAsync();
                return flats.Select(MapToFlatInfo).ToArray();
            }
        }

        //public async Task<FlatInfo[]> GetUserFlats(Int64 pUserId)
        //{
        //    using (var context = new SmartComplexDataObjectContext())
        //    {
        //        //var a = from m in context.MemberFlats
        //        //        join f in context.Flats on m.FlatId equals f.Id
        //        //        join apartment in context.Apartments on f.ApartmentId equals apartment.Id
        //        //        join flatType in context.FlatTypes on f.FlatTypeId equals flatType.Id
        //        //        where m.FlatUserId.Equals(pUserId)







        //        var flats = await context.Flats
        //            //.Include(pX => pX.MemberFlats)
        //            //.Include(pX => pX.Apartment)
        //            //.Include(pX => pX.FlatType)
        //            .Where(pX => pX.MemberFlats.Any(pZ => pZ.FlatUser.UserId == pUserId)).ToListAsync();

        //        return flats.Select(MapToFlatInfo).ToArray();
        //    }
        //}

        public async Task<FlatInfo> Get(int pFlatId)
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                var flatInfo = await context.Flats
                    .Include(pX => pX.Apartment)
                    .Include(pX => pX.FlatType)
                    .Where(pX => pX.Id.Equals(pFlatId)).FirstAsync();
                return MapToFlatInfo(flatInfo);
            }
        }

        public async Task Create(FlatInfo pApartmentFlatInfo, Int64 pUserId)
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                if(await context.Flats.AnyAsync(pX => pX.ApartmentId.Equals(pApartmentFlatInfo.ApartmentId) && pX.Name.Equals(pApartmentFlatInfo.Name)))
                    throw new ItemAlreadyExistsException(pApartmentFlatInfo.Name, "Flat");

                var flat = AddFlat(pApartmentFlatInfo, pUserId);
                context.Flats.Add(flat);

                await context.SaveChangesAsync();
            }
        }

        

        private Flat AddFlat(FlatInfo pApartmentFlatInfo, Int64 pUserId)
        {
            return new Flat
            {
                ApartmentId = pApartmentFlatInfo.ApartmentId,
                Block = pApartmentFlatInfo.Block,
                ExtensionNumber = pApartmentFlatInfo.ExtensionNumber,
                Floor = pApartmentFlatInfo.Floor.Value,
                Name = pApartmentFlatInfo.Name,
                Phase = pApartmentFlatInfo.Phase,
                SquareFeet = pApartmentFlatInfo.SquareFeet,
                LastUpdated = DateTime.Now,
                LastUpdatedBy = pUserId,
                FlatTypeId = pApartmentFlatInfo.FlatTypeId <= 0 ? null : pApartmentFlatInfo.FlatTypeId
            };
        }

        //private FlatUserInfo MapToFlatUserInfo(FlatUser pFlatUser)
        //{
        //    return new FlatUserInfo
        //    {
        //        FirstName = pFlatUser.FirstName,
        //        LastName = pFlatUser.LastName,
        //        IsLocked = pFlatUser.IsLocked,
        //        LockReason = pFlatUser.LockReason,
        //        IsDeleted = pFlatUser.IsDeleted,
        //        Email = pFlatUser.User.Email,
        //        EmailConfirmed = pFlatUser.User.EmailConfirmed,
        //        IsOwner = pFlatUser.IsOwner,
        //        LockedDate = pFlatUser.LockedDate,
        //        Mobile = pFlatUser.Mobile,
        //        IsActivated = pFlatUser.User.IsActivated,
        //        PhoneNumber = pFlatUser.User.PhoneNumber,

        //    };
        //}

        public async Task Update(FlatInfo pApartmentFlatInfo, long pLoggedInUser)
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                var original = await context.Flats.FindAsync(pApartmentFlatInfo.Id);

                if (original == null)
                    throw new KeyNotFoundException(pApartmentFlatInfo.Id.ToString());

                if (await context.Flats.AnyAsync(pX => pX.Name.Equals(pApartmentFlatInfo.Name, StringComparison.OrdinalIgnoreCase) && pX.ApartmentId.Equals(pApartmentFlatInfo.ApartmentId) && pX.Id != original.Id))
                    throw new ItemAlreadyExistsException(pApartmentFlatInfo.Name, "Flat");

                original.Block = pApartmentFlatInfo.Block;
                original.ExtensionNumber = pApartmentFlatInfo.ExtensionNumber;
                original.FlatTypeId = pApartmentFlatInfo.FlatTypeId <= 0 ? null : pApartmentFlatInfo.FlatTypeId;
                original.Floor = pApartmentFlatInfo.Floor.Value;
                original.Name = pApartmentFlatInfo.Name;
                original.Phase = pApartmentFlatInfo.Phase;
                original.SquareFeet = pApartmentFlatInfo.SquareFeet;
                original.LastUpdated = DateTime.Now;
                original.LastUpdatedBy = pLoggedInUser;

                await context.SaveChangesAsync();
            }
        }
    }
}