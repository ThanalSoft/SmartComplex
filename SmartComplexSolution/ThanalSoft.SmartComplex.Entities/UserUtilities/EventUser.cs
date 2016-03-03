using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using ThanalSoft.SmartComplex.Entities.Security;

namespace ThanalSoft.SmartComplex.Entities.UserUtilities
{
    [DataContract]
    [Table("tblEventUser")]
    public class EventUser : BaseModel
    {
        [DataMember]
        [Required]
        public int EventId { get; set; }

        [DataMember]
        [Required]
        public Int64 EventUserId { get; set; }

        [DataMember]
        [Required]
        public bool IsEventAdmin { get; set; }

        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }

        [ForeignKey("EventUserId")]
        public virtual LoginUser User { get; set; }
    }
}