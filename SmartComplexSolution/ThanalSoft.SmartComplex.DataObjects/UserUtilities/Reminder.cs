using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using ThanalSoft.SmartComplex.DataObjects.Security;

namespace ThanalSoft.SmartComplex.DataObjects.UserUtilities
{
    [DataContract]
    [Table("tblReminder")]
    public class Reminder : BaseModel
    {
        [DataMember]
        [Required]
        [StringLength(200)]
        public string Message { get; set; }

        [DataMember]
        [StringLength(300)]
        public string Description { get; set; }

        [DataMember]
        [Required]
        public Int64 CreatorId { get; set; }

        [DataMember]
        [Required]
        public DateTime CreatedTime { get; set; }

        [DataMember]
        [Required]
        public DateTime ExpiryTime { get; set; }

        [DataMember]
        [Required]
        public Int16 ReminderCount { get; set; }

        [DataMember]
        [Required]
        public DateTime ReminderTime { get; set; }

        [DataMember]
        [Required]
        public bool IsActive { get; set; }

        [ForeignKey("CreatorId")]
        public virtual LoginUser User { get; set; }
    }
}