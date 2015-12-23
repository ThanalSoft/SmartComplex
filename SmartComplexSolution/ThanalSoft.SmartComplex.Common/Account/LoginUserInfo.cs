using System;
using System.Runtime.Serialization;

namespace ThanalSoft.SmartComplex.Common.Account
{
    [DataContract]
    public class LoginUserInfo
    {
        [DataMember]
        public Int64 UserId { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string UserIdentity { get; set; }
    }
}