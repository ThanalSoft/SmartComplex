using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ThanalSoft.SmartComplex.Entities.Common;
using ThanalSoft.SmartComplex.Entities.Complex;
using ThanalSoft.SmartComplex.Entities.UserUtilities;

namespace ThanalSoft.SmartComplex.Entities.Security
{
    [DataContract]
    public class LoginUser : IdentityUser<Int64, UserLogin, UserRole, UserClaim>
    {
        [DataMember]
        [Required]
        public bool IsAdminUser { get; set; }

        [DataMember]
        [StringLength(100)]
        public string ActivationCode { get; set; }

        [Required]
        [DataMember]
        public bool IsActivated { get; set; }

        [DataMember]
        public DateTime? ActivatedDate { get; set; }

        [DataMember]
        [Required]
        [StringLength(250)]
        public string FirstName { get; set; }

        [DataMember]
        [StringLength(250)]
        public string LastName { get; set; }

        [DataMember]
        public int? BloodGroupId { get; set; }

        [DataMember]
        [Required]
        public bool IsFreezed { get; set; }

        [DataMember]
        public DateTime? FreezedDate { get; set; }

        [DataMember]
        public string ReasonForFreeze { get; set; }
        
        [DataMember]
        [Required]
        public bool IsDeleted { get; set; }

        [ForeignKey("BloodGroupId")]
        public virtual BloodGroup BloodGroup { get; set; }

        public virtual ICollection<Broadcast> Broadcasts { get; set; }

        public virtual ICollection<BroadcastUser> BroadcastUsers { get; set; }

        public virtual ICollection<Event> Events { get; set; }

        public virtual ICollection<EventUser> EventUsers { get; set; }

        public virtual ICollection<Reminder> Reminders { get; set; }

        public virtual ICollection<AmenityCalendar> AmenityCalendars { get; set; }

        public virtual ICollection<AssociationMember> AssociationMembers { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }

        public virtual ICollection<MemberFlat> MemberFlats { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<LoginUser, long> pUserManager, string pAuthenticationType)
        {
            // Note the authenticationType must match the one defined in
            // CookieAuthenticationOptions.AuthenticationType 
            var userIdentity = await pUserManager.CreateIdentityAsync(this, pAuthenticationType);
            // Add custom user claims here 
            return userIdentity;
        }
    }
}