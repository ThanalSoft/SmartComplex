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
using ThanalSoft.SmartComplex.DataObjects.Common;
using ThanalSoft.SmartComplex.DataObjects.Complex;
using ThanalSoft.SmartComplex.DataObjects.Security;

namespace ThanalSoft.SmartComplex.Business.Complex
{
    public class ApartmentContext : BaseBusiness<ApartmentContext>
    {
        static readonly PasswordHasher _hasher = new PasswordHasher();
        public async Task<ApartmentInfo[]> GetAllAsync()
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                var data = await context.Apartments.OrderByDescending(pX => pX.CreatedDate).ToArrayAsync();
                return data.Select(pX => MapToApartmentInfo(pX, context.States.Find(pX.StateId), false)).ToArray();
            }
        }

        public async Task<ApartmentInfo> GetAsync(int pApartmentId)
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                var data = await context.Apartments.FirstAsync(pX => pX.Id.Equals(pApartmentId));
                var hasFlatsLoaded = await context.Flats.AnyAsync(pX => pX.ApartmentId.Equals(pApartmentId));
                var state = data.State;

                return MapToApartmentInfo(data, state, hasFlatsLoaded);
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

        public async Task<ApartmentFlatInfo[]> GetFlatsAsync(int pApartmentId)
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                var data = await context.Flats.Where(pX => pX.ApartmentId.Equals(pApartmentId)).OrderBy(pX => pX.Name).ToArrayAsync();
                return data.Select(pFlat => MapToFlatInfo(pFlat, null)).ToArray();
            }
        }

        public async Task<ApartmentFlatInfo> GetFlatAsync(int pFlatId)
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                var flatInfoTask = context.Flats.Where(pX => pX.Id.Equals(pFlatId)).FirstAsync();
                var flatUsersTask = context.FlatUsers.Where(pX => pX.Id.Equals(pFlatId)).OrderBy(pX => pX.IsDeleted).ThenBy(pX => pX.FirstName).ToArrayAsync();
                await Task.WhenAll(flatInfoTask, flatUsersTask);

                return MapToFlatInfo(flatInfoTask.Result, flatUsersTask.Result);
            }
        }

        public async Task CreateFlatAsync(ApartmentFlatInfo pApartmentFlatInfo, Int64 pUserId)
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                var flat = AddFlat(pApartmentFlatInfo, pUserId);
                context.Flats.Add(flat);

                await context.SaveChangesAsync();
            }
        }

        public async Task UploadFlatsAsync(ApartmentFlatInfo[] pApartmentFlatInfoList, Int64 pUserId, Action<string, string, string> pSendEmail)
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                foreach (var apartmentFlatInfo in pApartmentFlatInfoList)
                {
                    var flat = AddFlat(apartmentFlatInfo, pUserId);
                    var activationCode = Guid.NewGuid().ToString();
                    var password = KeyGenerator.GetUniqueKey(8);
                    if (apartmentFlatInfo.ApartmentFlatUsers != null && apartmentFlatInfo.ApartmentFlatUsers.Any())
                    {
                        foreach (var apartmentFlatUserInfo in apartmentFlatInfo.ApartmentFlatUsers)
                        {
                            var flatUser = new FlatUser
                            {
                                FirstName = apartmentFlatUserInfo.Name,
                                BloodGroupId = apartmentFlatUserInfo.BloodGroupId,
                                IsDeleted = apartmentFlatUserInfo.IsDeleted,
                                IsLocked = apartmentFlatUserInfo.IsLocked,
                                IsOwner = apartmentFlatUserInfo.IsOwner,
                                LockReason = apartmentFlatUserInfo.LockReason,
                                Mobile = apartmentFlatUserInfo.Mobile,
                                LastUpdated = DateTime.Now,
                                LastUpdatedBy = pUserId
                            };

                            var existingUser = await context.Users.FirstOrDefaultAsync(pX => pX.Email.Equals(apartmentFlatUserInfo.Email));
                            if (existingUser != null)
                            {
                                flatUser.User = existingUser;
                                if (flat.FlatUsers == null)
                                {
                                    flat.FlatUsers = new List<FlatUser>
                                    {
                                        flatUser
                                    };
                                }
                            }
                            else
                            {
                                flatUser.User = new User
                                {
                                    Email = apartmentFlatUserInfo.Email,
                                    PhoneNumber = apartmentFlatUserInfo.Mobile,
                                    AccessFailedCount = 0,
                                    ActivatedDate = null,
                                    ActivationCode = activationCode,
                                    EmailConfirmed = false,
                                    PasswordHash = _hasher.HashPassword(password),
                                    IsActivated = false,
                                    LockoutEnabled = true,
                                    LockoutEndDateUtc = null,
                                    PhoneNumberConfirmed = true,
                                    TwoFactorEnabled = false,
                                    IsAdminUser = false,
                                    UserName = apartmentFlatUserInfo.Email,
                                    IsDeleted = false
                                };

                                if (flat.FlatUsers == null)
                                {
                                    flat.FlatUsers = new List<FlatUser>
                                    {
                                        flatUser
                                    };
                                }
                            }
                        }
                    }
                    context.Flats.Add(flat);
                    await context.SaveChangesAsync();

                    if (apartmentFlatInfo.ApartmentFlatUsers != null)
                        pSendEmail(apartmentFlatInfo.ApartmentFlatUsers[0].Email, password, activationCode);
                }
            }
        }

        private ApartmentFlatInfo MapToFlatInfo(Flat pFlat, FlatUser[] pFlatUsers)
        {
            var info = new ApartmentFlatInfo
            {
                Name = pFlat.Name,
                ApartmentId = pFlat.ApartmentId,
                Phase = pFlat.Phase,
                Floor = pFlat.Floor,
                Block = pFlat.Block,
                ExtensionNumber = pFlat.ExtensionNumber,
                SquareFeet = pFlat.SquareFeet
            };
            if (pFlatUsers != null && pFlatUsers.Any())
            {
                info.ApartmentFlatUsers = pFlatUsers.Select(pX => new ApartmentFlatUserInfo
                {
                    IsLocked = pX.IsLocked,
                    LockReason = pX.LockReason,
                    IsDeleted = pX.IsDeleted,
                    Name = pX.FirstName + (string.IsNullOrEmpty(pX.LastName) ? " " + pX.LastName : ""),
                    LockedDate = pX.LockedDate,
                    FlatId = pX.FlatId,
                    IsOwner = pX.IsOwner,
                    Mobile = pX.Mobile,
                    BloodGroupId = pX.BloodGroupId
                }).ToArray();
            }
            return info;
        }

        private static ApartmentInfo MapToApartmentInfo(Apartment pApartment, State pState, bool pHasFlatsLoaded)
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
                State = pState.Name,
                HasFlats = pHasFlatsLoaded
            };
        }

        private static Flat AddFlat(ApartmentFlatInfo pApartmentFlatInfo, Int64 pUserId)
        {
            var flat = new Flat
            {
                ApartmentId = pApartmentFlatInfo.ApartmentId,
                Block = pApartmentFlatInfo.Block,
                ExtensionNumber = pApartmentFlatInfo.ExtensionNumber,
                Floor = pApartmentFlatInfo.Floor,
                Name = pApartmentFlatInfo.Name,
                Phase = pApartmentFlatInfo.Phase,
                SquareFeet = pApartmentFlatInfo.SquareFeet,
                LastUpdated = DateTime.Now,
                LastUpdatedBy = pUserId
            };

            return flat;
        }
    }
}