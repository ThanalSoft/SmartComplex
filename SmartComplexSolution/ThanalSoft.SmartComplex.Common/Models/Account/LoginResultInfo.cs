using System.Runtime.Serialization;

namespace ThanalSoft.SmartComplex.Common.Models.Account
{
    [DataContract]
    public class LoginResultInfo
    {
        [DataMember]
        public LoginStatus LoginStatus { get; set; }
        [DataMember]
        public LoginUserInfo LoginUserInfo { get; set; }
    }

    [DataContract]
    public enum LoginStatus
    {
        [EnumMember]
        Success,
        [EnumMember]
        LockedOut,
        [EnumMember]
        RequiresVerification,
        [EnumMember]
        Failure,
    }
}