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
using ThanalSoft.SmartComplex.DataObjects.Complex;
using ThanalSoft.SmartComplex.DataObjects.Security;
using ThanalSoft.SmartComplex.DataObjects.UserUtilities;

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

        public async Task<ApartmentInfo> GetAsync(int pApartmentId)
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                var apartment = await context.Apartments
                    .Include(pX => pX.State)
                    .Where(pX => pX.Id.Equals(pApartmentId)).FirstAsync();

                var flats = await context.Flats.Where(pX => pX.ApartmentId.Equals(pApartmentId)).ToListAsync();
                
                var flatIds = flats.Select(pX => pX.Id);
                var flatUsers = await context.MemberFlats.Where(pX => flatIds.Contains(pX.FlatId)).ToArrayAsync();
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
                    throw new ItemAlreadyExistsException(pApartmentInfo.Name);

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
                    throw new ItemAlreadyExistsException(pApartmentInfo.Name);

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

        public async Task UploadFlatsAsync(FlatUploadInfo[] pFlatUploadInfoList, Int64 pUserId, Action<FlatUploadInfo, string, string> pConfigure)
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
                    LastUpdatedBy = pUserId,
                    Message = $"Request for uploading & configuring '{pFlatUploadInfoList.Length}' flats in apartment '{apartment.Name}' has received. System started processing the same.",
                    TargetUserId = pUserId,
                    UserReadDate = null
                });
                await context.SaveChangesAsync();

                foreach (var apartmentFlatInfo in pFlatUploadInfoList)
                {
                    var userAlreadyConfigured = true;
                    var activationCode = Guid.NewGuid().ToString();
                    var password = KeyGenerator.GetUniqueKey(8);

                    var flat = AddFlat(apartmentFlatInfo, pUserId);
                    
                    if (flat.MemberFlats == null)
                        flat.MemberFlats = new List<MemberFlat>();

                    var existingUser = await context.Users.FirstOrDefaultAsync(pX => pX.Email.ToLower().Equals(apartmentFlatInfo.OwnerEmail.ToLower()));
                    if (existingUser != null)
                    {
                        var flatUser = await context.FlatUsers.FirstOrDefaultAsync(pX => pX.UserId.Equals(existingUser.Id));
                        flat.MemberFlats.Add(new MemberFlat
                        {
                            FlatUser = flatUser,
                            LastUpdated = DateTime.Now,
                            LastUpdatedBy = pUserId
                        });
                    }
                    else
                    {
                        var flatUser = AddFlatOwner(apartmentFlatInfo, pUserId);
                        var user = CreateUserLoginForOwner(apartmentFlatInfo);
                        user.PasswordHash = _passwordHasher.HashPassword(password);
                        user.ActivationCode = activationCode;
                        flatUser.User = user;
                        userAlreadyConfigured = false;
                        context.Notifications.Add(new Notification
                        {
                            CreatedDate = DateTime.Now,
                            HasUserRead = false,
                            LastUpdated = DateTime.Now,
                            LastUpdatedBy = pUserId,
                            Message = "Welcome to Smart Complex.",
                            TargetUserId = user.Id
                        });

                        flat.MemberFlats.Add(new MemberFlat
                        {
                            FlatUser = flatUser,
                            LastUpdated = DateTime.Now,
                            LastUpdatedBy = pUserId
                        });
                    }

                    context.Flats.Add(flat);

                    await context.SaveChangesAsync();
                    if(!userAlreadyConfigured)
                        pConfigure(apartmentFlatInfo, password, activationCode);
                }

                context.Notifications.Add(new Notification
                {
                    CreatedDate = DateTime.Now,
                    HasUserRead = false,
                    LastUpdated = DateTime.Now,
                    LastUpdatedBy = pUserId,
                    Message = $"All the '{pFlatUploadInfoList.Length}' flats in apartment '{apartment.Name}' are uploaded and configured succesfully.",
                    TargetUserId = pUserId
                });
                await context.SaveChangesAsync();
            }
        }

        private User CreateUserLoginForOwner(FlatUploadInfo pApartmentFlatInfo)
        {
            return new User
            {
                AccessFailedCount = 0,
                ActivationCode = "",
                Email = pApartmentFlatInfo.OwnerEmail,
                EmailConfirmed = false,
                IsActivated = false,
                IsAdminUser = true,
                IsDeleted = false,
                LockoutEnabled = true,
                PasswordHash = "",
                PhoneNumber = pApartmentFlatInfo.OwnerMobile,
                PhoneNumberConfirmed = false,
                SecurityStamp = "",
                TwoFactorEnabled = false,
                UserName = pApartmentFlatInfo.OwnerEmail
            };
        }

        private FlatUser AddFlatOwner(FlatUploadInfo pApartmentFlatInfo, Int64 pUserId)
        {
            return new FlatUser
            {
                FirstName = pApartmentFlatInfo.OwnerName,
                BloodGroupId = null,
                IsDeleted = false,
                IsLocked = false,
                IsOwner = true,
                LockReason = null,
                Mobile = pApartmentFlatInfo.OwnerMobile,
                LastUpdated = DateTime.Now,
                LastUpdatedBy = pUserId,
                LastName = "",
                LockedDate = null,
                Email = pApartmentFlatInfo.OwnerEmail
            };
        }

        private Flat AddFlat(FlatUploadInfo pApartmentFlatInfo, Int64 pUserId)
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
                State = pApartment.State?.Name,
            };
        }
    }
}