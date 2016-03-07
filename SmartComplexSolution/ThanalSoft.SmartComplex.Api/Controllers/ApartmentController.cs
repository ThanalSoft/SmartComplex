using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using ThanalSoft.SmartComplex.Api.UnitOfWork;
using ThanalSoft.SmartComplex.Business.Common;
using ThanalSoft.SmartComplex.Business.Complex;
using ThanalSoft.SmartComplex.Common;
using ThanalSoft.SmartComplex.Common.Exceptions;
using ThanalSoft.SmartComplex.Common.Models.Complex;
using ThanalSoft.SmartComplex.Common.String;
using ThanalSoft.SmartComplex.Entities.Complex;
using ThanalSoft.SmartComplex.Entities.Security;
using ThanalSoft.SmartComplex.Entities.UserUtilities;

namespace ThanalSoft.SmartComplex.Api.Controllers
{
    [RoutePrefix("api/Apartment")]
    public class ApartmentController : BaseSecureController
    {
        private readonly PasswordHasher _passwordHasher = new PasswordHasher();

        public ApartmentController(IUnitOfWork pUnitOfWork) : base(pUnitOfWork)
        {
        }

        #region Get Methods

        [HttpGet]
        public async Task<GeneralReturnInfo<ApartmentInfo[]>> GetAll()
        {
            var result = new GeneralReturnInfo<ApartmentInfo[]>();
            try
            {
                var apartments = await UnitOfWork.Apartments.AllAsync();
                result.Info = apartments.Select(MapToApartmentInfo).ToArray();
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        [HttpGet]
        public async Task<GeneralReturnInfo<ApartmentInfo>> Get(string id)
        {
            var result = new GeneralReturnInfo<ApartmentInfo>();
            try
            {
                var apartment = await UnitOfWork.Apartments.FindAsync(Convert.ToInt32(id));
                result.Info = MapToApartmentInfo(apartment);
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        [HttpGet]
        public async Task<GeneralReturnInfo<ApartmentUserInfo[]>> GetApartmentUsers(string id)
        {
            var result = new GeneralReturnInfo<ApartmentUserInfo[]>();
            try
            {
                var users = (await UnitOfWork.Apartments.GetAllApartmentUsersAsync(Convert.ToInt32(id))).ToArray();
                users = users.GroupBy(pX => pX.UserId).Select(pX => pX.First()).ToArray();
                foreach (var apartmentUserInfo in users)
                {
                    var roles = await UserManager.GetRolesAsync(apartmentUserInfo.UserId);
                    apartmentUserInfo.UserRoles = string.Join(", ", roles);
                }

                result.Info = users;
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        [HttpGet]
        public async Task<GeneralReturnInfo<ApartmentUserInfo>> GetApartmentUser(string id)
        {
            var result = new GeneralReturnInfo<ApartmentUserInfo>();
            try
            {
                var user = await UnitOfWork.Apartments.GetApartmentUserAsync(Convert.ToInt32(id));
                var roles = await UserManager.GetRolesAsync(Convert.ToInt64(id));
                user.UserRoles = string.Join(", ", roles);
                result.Info = user;
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        [HttpGet]
        public async Task<GeneralReturnInfo> MarkUserAdmin(string id)
        {
            var result = new GeneralReturnInfo();
            try
            {
                var userid = Convert.ToInt64(id);
                if (await UserManager.IsInRoleAsync(userid, "ApartmentAdmin"))
                    await UserManager.RemoveFromRoleAsync(userid, "ApartmentAdmin");
                else
                    await UserManager.AddToRoleAsync(userid, "ApartmentAdmin");
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        public async Task<GeneralReturnInfo<ApartmentInfo[]>> GetUserApartments(string id)
        {
            var result = new GeneralReturnInfo<ApartmentInfo[]>();
            try
            {
                result.Info = await UnitOfWork.Apartments.GetUserApartmentsAsync(Convert.ToInt64(id));
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        #endregion

        #region Post Methods

        [HttpPost]
        public async Task<GeneralReturnInfo> Create([FromBody]ApartmentInfo pApartmentInfo)
        {
            var result = new GeneralReturnInfo();
            try
            {
                var apartment = await UnitOfWork.Apartments.FindAsync(pX => pX.Name.Equals(pApartmentInfo.Name));
                if (apartment != null)
                    throw new ItemAlreadyExistsException(pApartmentInfo.Name, "Apartment");

                UnitOfWork.Apartments.Add(new Apartment
                {
                    Phone = pApartmentInfo.Phone,
                    StateId = pApartmentInfo.StateId,
                    Name = pApartmentInfo.Name,
                    Address = pApartmentInfo.Address,
                    City = pApartmentInfo.City,
                    IsDeleted = false,
                    IsLocked = false,
                    LastUpdated = DateTime.Now,
                    LastUpdatedBy = LoggedInUser,
                    PinCode = pApartmentInfo.PinCode,
                    CreatedDate = DateTime.Now,
                });

                await UnitOfWork.WorkCompleteAsync();
            }
            catch (ItemAlreadyExistsException ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        [HttpPost]
        public async Task<GeneralReturnInfo> Update([FromBody]ApartmentInfo pApartmentInfo)
        {
            var result = new GeneralReturnInfo();
            try
            {
                var original = await UnitOfWork.Apartments.FindAsync(pApartmentInfo.Id);
                if(original == null)
                    throw new KeyNotFoundException(pApartmentInfo.Id.ToString());

                var apartment = await UnitOfWork.Apartments.FindAsync(pX => pX.Name.Equals(pApartmentInfo.Name) && pX.Id != pApartmentInfo.Id);
                if (apartment != null)
                    throw new ItemAlreadyExistsException(pApartmentInfo.Name, "Apartment");
                
                original.Phone = pApartmentInfo.Phone;
                original.StateId = pApartmentInfo.StateId;
                original.Name = pApartmentInfo.Name;
                original.Address = pApartmentInfo.Address;
                original.City = pApartmentInfo.City;
                original.LastUpdated = DateTime.Now;
                original.LastUpdatedBy = LoggedInUser;
                original.PinCode = pApartmentInfo.PinCode;

                await UnitOfWork.WorkCompleteAsync();
            }
            catch (ItemAlreadyExistsException ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        [HttpPost]
        public async Task<GeneralReturnInfo> DeleteUndelete([FromBody] int id)
        {
            var result = new GeneralReturnInfo();
            try
            {
                var original = await UnitOfWork.Apartments.FindAsync(id);
                if (original == null)
                    throw new KeyNotFoundException(id.ToString());

                original.IsDeleted = !original.IsDeleted;
                original.LastUpdated = DateTime.Now;
                original.LastUpdatedBy = LoggedInUser;

                await UnitOfWork.WorkCompleteAsync();
            }
            catch (ItemAlreadyExistsException ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        [HttpPost]
        public async Task<GeneralReturnInfo> LockUnlock([FromBody]ApartmentInfo pApartmentInfo)
        {
            var result = new GeneralReturnInfo();
            try
            {
                var original = await UnitOfWork.Apartments.FindAsync(pApartmentInfo.Id);
                if (original == null)
                    throw new KeyNotFoundException(pApartmentInfo.Id.ToString());
                
                original.IsLocked = !original.IsLocked;
                original.LockedDate = !original.IsLocked ? (DateTime?)null : DateTime.Now;
                original.LockReason = !original.IsLocked ? null : pApartmentInfo.LockReason;
                original.LastUpdated = DateTime.Now;
                original.LastUpdatedBy = LoggedInUser;

                await UnitOfWork.WorkCompleteAsync();
            }
            catch (ItemAlreadyExistsException ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        [HttpPost]
        public async Task<GeneralReturnInfo> UploadFlats(FlatUploadInfo[] pApartmentFlatInfoList)
        {
            var result = new GeneralReturnInfo();
            try
            {
                if (pApartmentFlatInfoList == null || !pApartmentFlatInfoList.Any())
                    return result;

                var apartment = await UnitOfWork.Apartments.FindAsync(pApartmentFlatInfoList[0].ApartmentId);
                if (apartment == null)
                    throw new Exception("Apartment doesnt exists with ID: " + pApartmentFlatInfoList[0].ApartmentId);

                UnitOfWork.Notifications.Add(UserNotificationsHelper.GetNotification(LoggedInUser,
                    $"Request for uploading & configuring '{pApartmentFlatInfoList.Length}' flats in apartment '{apartment.Name}' has received. System started processing the same.",
                    LoggedInUser));

                await UnitOfWork.WorkCompleteAsync();

                foreach (var apartmentFlatInfo in pApartmentFlatInfoList)
                {
                    var userAlreadyConfigured = true;
                    var activationCode = Guid.NewGuid().ToString();
                    var password = KeyGenerator.GetUniqueKey(8);

                    var flat = AddFlat(apartmentFlatInfo);
                    if (flat.MemberFlats == null)
                        flat.MemberFlats = new List<MemberFlat>();

                    var existingUser = await UnitOfWork.Users.FindAsync(pX => pX.Email.Equals(apartmentFlatInfo.OwnerEmail));
                    if (existingUser != null)
                    {
                        var flatUser = await UnitOfWork.Users.FindAsync(pX => pX.Id.Equals(existingUser.Id));
                        flat.MemberFlats.Add(new MemberFlat
                        {
                            User = flatUser,
                            Flat = flat,
                            Apartment = apartment,
                            LastUpdated = DateTime.Now,
                            LastUpdatedBy = LoggedInUser,
                            IsOwner = true
                        });

                        UnitOfWork.Notifications.Add(UserNotificationsHelper.GetNotification(existingUser.Id,
                            $"You can now start managing Flat '{flat.Name}' in Apartment '{apartment.Name}'.",
                            LoggedInUser));
                    }
                    else
                    {
                        userAlreadyConfigured = false;

                        var flatUser = AddFlatOwner(apartmentFlatInfo);
                        flatUser.PasswordHash = _passwordHasher.HashPassword(password);
                        flatUser.ActivationCode = activationCode;

                        UnitOfWork.Notifications.Add(UserNotificationsHelper.GetNotification(flatUser.Id,
                            "Welcome to Smart Complex.",
                            LoggedInUser));

                        UnitOfWork.Notifications.Add(UserNotificationsHelper.GetNotification(flatUser.Id,
                            $"You can now start managing Flat '{flat.Name}' in Apartment '{apartment.Name}'.",
                            LoggedInUser));

                        flat.MemberFlats.Add(new MemberFlat
                        {
                            User = flatUser,
                            Flat = flat,
                            Apartment = apartment,
                            LastUpdated = DateTime.Now,
                            LastUpdatedBy = LoggedInUser,
                            IsOwner = true
                        });
                    }

                    UnitOfWork.Flats.Add(flat);

                    await UnitOfWork.WorkCompleteAsync();
                    
                    if(!userAlreadyConfigured)
                        ConfigureUser(apartmentFlatInfo, password, activationCode);
                }

                UnitOfWork.Notifications.Add(UserNotificationsHelper.GetNotification(LoggedInUser,
                            $"All the '{pApartmentFlatInfoList.Length}' flats in apartment '{apartment.Name}' are uploaded and configured succesfully.",
                            LoggedInUser));

                await UnitOfWork.WorkCompleteAsync();
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;

                UnitOfWork.Notifications.Add(UserNotificationsHelper.GetNotification(LoggedInUser,
                            $"Request for uploading & configuring flats failed for some reason. Please review and try again.",
                            LoggedInUser));

                await UnitOfWork.WorkCompleteAsync();
            }
            return result;
        }

        #endregion

        #region Private Methods

        [NonAction]
        private void ConfigureUser(FlatUploadInfo pUser, string pPassword, string pActivationCode)
        {
            if (string.IsNullOrEmpty(pUser.OwnerEmail))
                return;

            var user = UserManager.FindByEmail(pUser.OwnerEmail);
            
            AddOwnerRole(user);

            if (!string.IsNullOrEmpty(user.ActivationCode) && !user.IsActivated)
                SendUserEmail(pUser.OwnerEmail, pPassword, pActivationCode, user);
        }

        [NonAction]
        private void AddOwnerRole(LoginUser pUser)
        {
            UserManager.AddToRole(pUser.Id, "Owner");
        }

        [NonAction]
        private void SendUserEmail(string pEmail, string pPassword, string pActivationCode, LoginUser pUser)
        {
            var url = $"{ConfigurationManager.AppSettings["WEB_URL"]}/{"Account"}/{"ConfirmEmail"}/{pUser.Id}/?token={pActivationCode}";
            UserManager.SendEmail(pUser.Id, "Welcome to Smart Complex!", GetBody(url, pEmail, pPassword));
        }

        [NonAction]
        private string GetBody(string pUrl, string pEmail, string pPassword)
        {
            string content = $"<div style='color:#666 !important;'>Hi,<div><u><b><font size='3'><br></font></b></u></div><div><b><font size='3' face='Lucida Sans'>Thanks for choosing SmartComplex! Now leave SMARTLY in your COMPLEX.</font></b></div><div><br><br></div><div>Please click <strong style='font-size:135%;color:#49A6FD !important;'><a href='{ pUrl}'>here</a></strong> to confirm your email. Once the email validation is completed use the below credentials to login to your account.</div><div><br></div><div style='color:#000 !important;'><font face='Arial Black'>Username :&nbsp;{pEmail}</font></div><div style='color:#000 !important;'><font face='Arial Black'>Password &nbsp;:&nbsp;{pPassword}</font></div><div><br><br></div><div><b>Thank You!</b></div></div>";
            return content;
        }

        [NonAction]
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
                UserCount = pApartment.MemberFlats.Select(pX => pX.UserId).Distinct().Count(),
                FlatCount = pApartment.Flats.Count
            };
        }

        [NonAction]
        private Flat AddFlat(FlatUploadInfo pApartmentFlatInfo)
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
                LastUpdatedBy = LoggedInUser
            };
        }

        [NonAction]
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
    }

    #endregion

}