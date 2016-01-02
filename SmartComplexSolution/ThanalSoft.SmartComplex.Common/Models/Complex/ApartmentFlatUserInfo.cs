using System;
using System.Runtime.Serialization;

namespace ThanalSoft.SmartComplex.Common.Models.Complex
{
    [DataContract]
    public class ApartmentFlatUserInfo
    {
        [DataMember]
        public int FlatId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Mobile { get; set; }

        [DataMember]
        public bool IsOwner { get; set; }

        [DataMember]
        public bool IsLocked { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }

        [DataMember]
        public int? BloodGroupId { get; set; }

        [DataMember]
        public string BloodGroup { get; set; }

        [DataMember]
        public DateTime? LockedDate { get; set; }

        [DataMember]
        public string LockReason { get; set; }

        [DataMember]
        public string Email { get; set; }
    }
}
