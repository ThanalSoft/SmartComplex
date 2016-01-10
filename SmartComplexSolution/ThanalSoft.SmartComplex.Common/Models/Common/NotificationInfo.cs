using System;
using System.Runtime.Serialization;
using ThanalSoft.SmartComplex.Common.Attributes;

namespace ThanalSoft.SmartComplex.Common.Models.Common
{
    [DataContract]
    [Table(false)]
    public class NotificationInfo
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [TableColumn("Message")]
        public string Message { get; set; }

        [DataMember]
        [TableColumn("When")]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public Int64 TargetUserId { get; set; }

        [DataMember]
        public bool HasUserRead { get; set; }

        [DataMember]
        public DateTime? UserReadDate { get; set; }
    }
}