using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using ThanalSoft.SmartComplex.DataObjects.Security;

namespace ThanalSoft.SmartComplex.DataObjects.UserUtilities
{
    [DataContract]
    [Table("tblNotification")]
    public class Notification : BaseModel
    {
        [DataMember]
        [Required]
        [StringLength(200)]
        public string Message { get; set; }

        [DataMember]
        [Required]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        [Required]
        public Int64 TargetUserId { get; set; }

        [DataMember]
        [Required]
        public bool HasUserRead { get; set; }

        [DataMember]
        public DateTime? UserReadDate { get; set; }
        
        [ForeignKey("TargetUserId")]
        public virtual LoginUser User { get; set; }
    }
}