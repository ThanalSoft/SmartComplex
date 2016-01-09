using System;
using System.Runtime.Serialization;

namespace ThanalSoft.SmartComplex.Common.Models.Account
{
    [DataContract]
    public class LoginUserInfo
    {
        [DataMember]
        public Int64 UserId { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string UserIdentity { get; set; }
        [DataMember]
        public string[] Roles { get; set; }
    }
}