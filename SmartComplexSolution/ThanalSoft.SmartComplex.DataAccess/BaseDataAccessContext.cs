using System;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using ThanalSoft.SmartComplex.DataObjects.Common;
using ThanalSoft.SmartComplex.DataObjects.Complex;
using ThanalSoft.SmartComplex.DataObjects.Security;
using ThanalSoft.SmartComplex.DataObjects.UserUtilities;

namespace ThanalSoft.SmartComplex.DataAccess
{
    public class BaseDataAccessContext : IdentityDbContext<User, Role, Int64, UserLogin, UserRole, UserClaim>
    {
        public BaseDataAccessContext() : base("name=SmartComplexDB")
        {
            
        }

        public virtual DbSet<BloodGroup> BloodGroups { get; set; }

        public virtual DbSet<Country> Countries { get; set; }

        public virtual DbSet<State> States { get; set; }

        #region Complex

        public virtual DbSet<AmenityType> AminityTypes { get; set; }

        public virtual DbSet<AssociationMemberRank> AssociationMemberRanks { get; set; }

        public virtual DbSet<Apartment> Apartments { get; set; }

        public virtual DbSet<ApartmentAmenity> ApartmentAmenities { get; set; }
        
        public virtual DbSet<Flat> Flats { get; set; }

        public virtual DbSet<FlatUser> FlatUsers { get; set; }

        public virtual DbSet<Association> Associations { get; set; }

        public virtual DbSet<AssociationMember> AssociationMembers { get; set; }

        public virtual DbSet<MemberFlat> MemberFlats { get; set; }

        #endregion

        #region User Utilities

        public virtual DbSet<AmenityCalendar> AmenityCalendars { get; set; }

        public virtual DbSet<Broadcast> Broadcasts { get; set; }
        
        public virtual DbSet<Event> Events { get; set; }

        public virtual DbSet<Reminder> Reminders { get; set; }

        public virtual DbSet<BroadcastUser> BroadcastUsers { get; set; }

        public virtual DbSet<EventUser> EventUsers { get; set; }

        public virtual DbSet<Notification> Notifications { get; set; }

        #endregion

    }
}