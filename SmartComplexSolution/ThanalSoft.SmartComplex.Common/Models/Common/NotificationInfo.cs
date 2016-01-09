using System;
using System.Runtime.Serialization;

namespace ThanalSoft.SmartComplex.Common.Models.Common
{
    [DataContract]
    public class NotificationInfo
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public Int64 TargetUserId { get; set; }

        [DataMember]
        public bool HasUserRead { get; set; }

        [DataMember]
        public DateTime? UserReadDate { get; set; }
    }
}