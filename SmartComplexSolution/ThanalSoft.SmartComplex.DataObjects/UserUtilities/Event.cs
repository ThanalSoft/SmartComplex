using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using ThanalSoft.SmartComplex.DataObjects.Security;

namespace ThanalSoft.SmartComplex.DataObjects.UserUtilities
{
    [DataContract]
    [Table("tblEvent")]
    public class Event : BaseModel
    {
        [DataMember]
        [StringLength(100)]
        [Required]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        [Required]
        public DateTime From { get; set; }

        [DataMember]
        [Required]
        public DateTime To { get; set; }

        [DataMember]
        [Required]
        public Int64 CreatorId { get; set; }

        [DataMember]
        [Required]
        public DateTime CreatedTime { get; set; }

        [DataMember]
        [Required]
        public bool IsDeleted { get; set; }

        [ForeignKey("CreatorId")]
        public virtual User CreatorUser { get; set; }

        public virtual ICollection<EventUser> EventUsers { get; set; }
    }
}