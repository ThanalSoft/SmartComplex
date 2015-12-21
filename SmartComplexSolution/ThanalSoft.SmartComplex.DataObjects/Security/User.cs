using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ThanalSoft.SmartComplex.DataObjects.Complex;
using ThanalSoft.SmartComplex.DataObjects.UserUtilities;

namespace ThanalSoft.SmartComplex.DataObjects.Security
{
    [DataContract]
    public class User : IdentityUser<Int64, UserLogin, UserRole, UserClaim>
    {
        [DataMember]
        [StringLength(10)]
        public string ActivationCode { get; set; }

        [Required]
        [DataMember]
        public bool IsActivated { get; set; }

        [DataMember]
        public DateTime? ActivatedDate { get; set; }

        [DataMember]
        [Required]
        public bool IsAdminUser { get; set; }

        public virtual ICollection<FlatUser> FlatUsers { get; set; }

        public virtual ICollection<Broadcast> Broadcasts { get; set; }

        public virtual ICollection<BroadcastUser> BroadcastUsers { get; set; }

        public virtual ICollection<Event> Events { get; set; }

        public virtual ICollection<EventUser> EventUsers { get; set; }

        public virtual ICollection<Reminder> Reminders { get; set; }

        public virtual ICollection<AmenityCalendar> AmenityCalendars { get; set; }

        public virtual ICollection<AssociationMember> AssociationMembers { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User,Int64> pUserManager)
        {
            // Note the authenticationType must match the one defined in
            // CookieAuthenticationOptions.AuthenticationType 
            var userIdentity = await pUserManager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here 
            return userIdentity;
        }
    }
}