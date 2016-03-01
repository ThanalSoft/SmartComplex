using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using ThanalSoft.SmartComplex.DataObjects.Security;

namespace ThanalSoft.SmartComplex.DataObjects.UserUtilities
{
    [DataContract]
    [Table("tblBroadcast")] 
    public class Broadcast : BaseModel
    {
        [Required]
        [DataMember]
        [StringLength(100)]
        public string Content { get; set; }

        [Required]
        [DataMember]
        public string Description { get; set; }

        [Required]
        [DataMember]
        public Int64 CreatorId { get; set; }

        [Required]
        [DataMember]
        public DateTime CreatedTime { get; set; }

        [Required]
        [DataMember]
        public bool IsAlert { get; set; }

        [Required]
        [DataMember]
        public bool IsDeleted { get; set; }

        [ForeignKey("CreatorId")]
        public virtual LoginUser Creator { get; set; }

        //public virtual ICollection<BroadcastUser> BroadcastUsers { get; set; }
    }
}