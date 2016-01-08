using System;
using System.Runtime.Serialization;

namespace ThanalSoft.SmartComplex.Common.Models.Complex
{
    [DataContract]
    public class FlatUserInfo
    {
        [DataMember]
        public bool EmailConfirmed { get; set; }
        [DataMember]
        public bool IsActivated { get; set; }
        [DataMember]
        public string PhoneNumber { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Mobile { get; set; }
        [DataMember]
        public bool IsOwner { get; set; }
        [DataMember]
        public bool IsLocked { get; set; }
        [DataMember]
        public DateTime? LockedDate { get; set; }
        [DataMember]
        public string LockReason { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
    }
}