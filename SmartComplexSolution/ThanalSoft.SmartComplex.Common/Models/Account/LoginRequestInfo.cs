using System.Runtime.Serialization;

namespace ThanalSoft.SmartComplex.Common.Models.Account
{
    [DataContract]
    public class LoginRequestInfo
    {
        public LoginRequestInfo(string pEmail, string pPassword)
        {
            Email = pEmail;
            Password = pPassword;
        }

        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Password { get; set; }
    }
}