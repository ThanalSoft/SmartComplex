using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using ThanalSoft.SmartComplex.DataObjects.Security;

namespace ThanalSoft.SmartComplex.DataObjects.UserUtilities
{
    [DataContract]
    [Table("tblBroadcastUser")]
    public class BroadcastUser : BaseModel
    {
        [DataMember]
        [Required]
        public Int64 ReceiverUserId { get; set; }

        [DataMember]
        [Required]
        public int BroadcastId { get; set; }

        [DataMember]
        [Required]
        public bool IsRead { get; set; }

        [ForeignKey("BroadcastId")]
        public virtual Broadcast Broadcast { get; set; }

        [ForeignKey("ReceiverUserId")]
        public virtual LoginUser ReceiverUser { get; set; }
    }
}