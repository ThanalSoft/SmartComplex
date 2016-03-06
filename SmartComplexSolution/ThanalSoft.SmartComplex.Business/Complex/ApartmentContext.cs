using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using ThanalSoft.SmartComplex.Common.Exceptions;
using ThanalSoft.SmartComplex.Common.Models.Complex;
using ThanalSoft.SmartComplex.Common.String;
using ThanalSoft.SmartComplex.DataAccess;
using ThanalSoft.SmartComplex.Entities.Complex;
using ThanalSoft.SmartComplex.Entities.Security;
using ThanalSoft.SmartComplex.Entities.UserUtilities;

namespace ThanalSoft.SmartComplex.Business.Complex
{
    public class ApartmentContext : BaseBusiness<ApartmentContext>
    {
        private readonly PasswordHasher _passwordHasher = new PasswordHasher();

        public async Task<ApartmentInfo[]> GetAllAsync()
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                var data = await context.Apartments.OrderByDescending(pX => pX.CreatedDate).ToArrayAsync();
                return data.Select(MapToApartmentInfo).ToArray();
            }
        }

        //public async Task<ApartmentInfo[]> GetUserApartments(Int64 pUserId)
        //{
        //    using (var context = new SmartComplexDataObjectContext())
        //    {
        //        var data = await context.Apartments
        //            .Include(pX => pX.Flats)
        //            .Include(pX => pX.Flats.Select(pY => pY.MemberFlats))
        //            .Include(pX => pX.Flats.Select(pY => pY.MemberFlats.Select(pZ => pZ.FlatUser)))
        //            .Where(pX => pX.Flats.Any(pY => pY.MemberFlats.Any(pZ => pZ.FlatUser.UserId == pUserId)))
        //            .OrderBy(pX => pX.Name).ToArrayAsync();

        //        return data.Select(MapToApartmentInfo).ToArray();
        //    }
        //}

        public async Task<ApartmentInfo> GetAsync(int pApartmentId)
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                var apartment = await context.Apartments.Where(pX => pX.Id.Equals(pApartmentId)).FirstAsync();

                var flats = apartment.Flats;
                var flatIds = flats.Select(pX => pX.Id);
                var flatUsers = await context.MemberFlats.Where(pX => flatIds.Contains(pX.FlatId)).Select(pX => pX.UserId).Distinct().ToArrayAsync();

                var apartmentInfo = MapToApartmentInfo(apartment);
                apartmentInfo.FlatCount = flats.Count;
                apartmentInfo.UserCount = flatUsers.Length;

                return apartmentInfo;
            }
        }

        public async Task CreateAsync(ApartmentInfo pApartmentInfo, Int64 pUserId)
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                if (await context.Apartments.AnyAsync(pX => pX.Name.Equals(pApartmentInfo.Name, StringComparison.OrdinalIgnoreCase)))
                    throw new ItemAlreadyExistsException(pApartmentInfo.Name, "Apartment");

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
                if(original == null)
                    throw new KeyNotFoundException(pApartmentInfo.Id.ToString());

                if (await context.Apartments.AnyAsync(pX => pX.Name.Equals(pApartmentInfo.Name, StringComparison.OrdinalIgnoreCase) && pX.Id != original.Id))
                    throw new ItemAlreadyExistsException(pApartmentInfo.Name, "Apartment");

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

        public async Task LockUnlockAsync(ApartmentInfo pApartmentInfo, Int64 pUserId)
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                var original = await context.Apartments.FindAsync(pApartmentInfo.Id);

                original.IsLocked = !original.IsLocked;
                original.LockedDate = !original.IsLocked ? (DateTime?)null : DateTime.Now;
                original.LockReason = !original.IsLocked ? null : pApartmentInfo.LockReason;
                original.LastUpdated = DateTime.Now;
                original.LastUpdatedBy = pUserId;
                await context.SaveChangesAsync();
            }
        }

        public async Task UploadFlatsAsync(FlatUploadInfo[] pFlatUploadInfoList, Int64 pLoginUser, Action<FlatUploadInfo, string, string> pConfigure)
        {
            if (pFlatUploadInfoList == null || !pFlatUploadInfoList.Any())
                return;

            using (var context = new SmartComplexDataObjectContext())
            {
                var apartment = await context.Apartments.FindAsync(pFlatUploadInfoList[0].ApartmentId);
                if (apartment == null)
                    throw new Exception("Apartment id doesnt exists!");

                context.Notifications.Add(new Notification
                {
                    CreatedDate = DateTime.Now,
                    HasUserRead = false,
                    LastUpdated = DateTime.Now,
                    LastUpdatedBy = pLoginUser,
                    Message = $"Request for uploading & configuring '{pFlatUploadInfoList.Length}' flats in apartment '{apartment.Name}' has received. System started processing the same.",
                    TargetUserId = pLoginUser,
                    UserReadDate = null
                });
                await context.SaveChangesAsync();

                foreach (var apartmentFlatInfo in pFlatUploadInfoList)
                {
                    var userAlreadyConfigured = true;
                    var activationCode = Guid.NewGuid().ToString();
                    var password = KeyGenerator.GetUniqueKey(8);

                    var flat = AddFlat(apartmentFlatInfo, pLoginUser);

                    if (flat.MemberFlats == null)
                        flat.MemberFlats = new List<MemberFlat>();

                    var existingUser = await context.Users.
                        FirstOrDefaultAsync(pX => pX.Email.ToLower().Equals(apartmentFlatInfo.OwnerEmail.ToLower()));
                    if (existingUser != null)
                    {
                        var flatUser = await context.Users.FirstOrDefaultAsync(pX => pX.Id.Equals(existingUser.Id));
                        flat.MemberFlats.Add(new MemberFlat
                        {
                            User = flatUser,
                            Flat = flat,
                            Apartment = apartment,
                            LastUpdated = DateTime.Now,
                            LastUpdatedBy = pLoginUser,
                            IsOwner = true
                        });

                        context.Notifications.Add(new Notification
                        {
                            CreatedDate = DateTime.Now,
                            HasUserRead = false,
                            LastUpdated = DateTime.Now,
                            LastUpdatedBy = pLoginUser,
                            Message = $"You can now start managing Flat '{flat.Name}'.",
                            TargetUserId = existingUser.Id
                        });
                    }
                    else
                    {
                        userAlreadyConfigured = false;

                        var flatUser = AddFlatOwner(apartmentFlatInfo);
                        flatUser.PasswordHash = _passwordHasher.HashPassword(password);
                        flatUser.ActivationCode = activationCode;
                        context.Notifications.Add(new Notification
                        {
                            CreatedDate = DateTime.Now,
                            HasUserRead = false,
                            LastUpdated = DateTime.Now,
                            LastUpdatedBy = pLoginUser,
                            Message = "Welcome to Smart Complex.",
                            TargetUserId = flatUser.Id
                        });
                        context.Notifications.Add(new Notification
                        {
                            CreatedDate = DateTime.Now,
                            HasUserRead = false,
                            LastUpdated = DateTime.Now,
                            LastUpdatedBy = pLoginUser,
                            Message = $"You can now start managing Flat '{flat.Name}'.",
                            TargetUserId = flatUser.Id
                        });

                        flat.MemberFlats.Add(new MemberFlat
                        {
                            User = flatUser,
                            Flat = flat,
                            Apartment = apartment,
                            LastUpdated = DateTime.Now,
                            LastUpdatedBy = pLoginUser,
                            IsOwner = true
                        });
                    }

                    context.Flats.Add(flat);

                    await context.SaveChangesAsync();

                    if (!userAlreadyConfigured)
                        pConfigure(apartmentFlatInfo, password, activationCode);
                }

                context.Notifications.Add(new Notification
                {
                    CreatedDate = DateTime.Now,
                    HasUserRead = false,
                    LastUpdated = DateTime.Now,
                    LastUpdatedBy = pLoginUser,
                    Message = $"All the '{pFlatUploadInfoList.Length}' flats in apartment '{apartment.Name}' are uploaded and configured succesfully.",
                    TargetUserId = pLoginUser
                });
                await context.SaveChangesAsync();
            }
        }

        //public async Task<FlatUser> GetMember(Int64 pUserId)
        //{
        //    using (var context = new SmartComplexDataObjectContext())
        //    {
        //        return await context.FlatUsers.FirstOrDefaultAsync(pX => pX.UserId.Equals(pUserId));
        //    }
        //}

        //private User CreateUserLoginForOwner(FlatUploadInfo pApartmentFlatInfo)
        //{
        //    return new User
        //    {
        //        AccessFailedCount = 0,
        //        ActivationCode = "",
        //        Email = pApartmentFlatInfo.OwnerEmail,
        //        EmailConfirmed = false,
        //        IsActivated = false,
        //        IsAdminUser = true,
        //        IsDeleted = false,
        //        LockoutEnabled = true,
        //        PasswordHash = "",
        //        PhoneNumber = pApartmentFlatInfo.OwnerMobile,
        //        PhoneNumberConfirmed = false,
        //        SecurityStamp = "",
        //        TwoFactorEnabled = false,
        //        UserName = pApartmentFlatInfo.OwnerEmail
        //    };
        //}

        private LoginUser AddFlatOwner(FlatUploadInfo pApartmentFlatInfo)
        {
            return new LoginUser
            {
                FirstName = pApartmentFlatInfo.OwnerName,
                LastName = "",
                BloodGroupId = null,
                Email = pApartmentFlatInfo.OwnerEmail,
                UserName = pApartmentFlatInfo.OwnerEmail,
                AccessFailedCount = 0,
                ActivationCode = "",
                EmailConfirmed = false,
                IsActivated = false,
                IsAdminUser = true,
                IsDeleted = false,
                LockoutEnabled = true,
                PasswordHash = "",
                PhoneNumber = pApartmentFlatInfo.OwnerMobile,
                PhoneNumberConfirmed = false,
                SecurityStamp = null,
                TwoFactorEnabled = false,
                IsFreezed = false,
                FreezedDate = null,
                ReasonForFreeze = null,
                ActivatedDate = null,
                LockoutEndDateUtc = null
            };
        }

        private Flat AddFlat(FlatUploadInfo pApartmentFlatInfo, Int64 pUserId)
        {
            return new Flat
            {
                ApartmentId = pApartmentFlatInfo.ApartmentId,
                Block = string.IsNullOrEmpty(pApartmentFlatInfo.Block) ? null : pApartmentFlatInfo.Block,
                ExtensionNumber = null,
                Floor = pApartmentFlatInfo.Floor,
                Name = pApartmentFlatInfo.Name,
                Phase = string.IsNullOrEmpty(pApartmentFlatInfo.Phase) ? null : pApartmentFlatInfo.Phase,
                SquareFeet = null,
                FlatTypeId = null,
                LastUpdated = DateTime.Now,
                LastUpdatedBy = pUserId
            };
        }

        private static ApartmentInfo MapToApartmentInfo(Apartment pApartment)
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