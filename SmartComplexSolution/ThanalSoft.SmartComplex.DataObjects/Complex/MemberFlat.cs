using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using ThanalSoft.SmartComplex.DataObjects.Security;

namespace ThanalSoft.SmartComplex.DataObjects.Complex
{
    [DataContract]
    [Table("tblMemberFlat")]
    public class MemberFlat : BaseModel
    {
        [DataMember]
        [Required]
        public int FlatId { get; set; }

        [DataMember]
        [Required]
        public Int64 UserId { get; set; }

        [DataMember]
        [Required]
        public bool IsOwner { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("FlatId")]
        public virtual Flat Flat { get; set; }
    }
}