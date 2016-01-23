using System;
using System.Runtime.Serialization;

namespace ThanalSoft.SmartComplex.Common.Models.Account
{
    [DataContract]
    public class UserProfileInfo
    {
        [DataMember]
        public Int64 UserId { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Mobile { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string NewPassword { get; set; }
        [DataMember]
        public int? BloodGroupId { get; set; }
    }
}