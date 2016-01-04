using System.Runtime.Serialization;

namespace ThanalSoft.SmartComplex.Common.Models.Account
{
    [DataContract]
    public class ConfirmEmailAccount
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string Token { get; set; }
    }
}