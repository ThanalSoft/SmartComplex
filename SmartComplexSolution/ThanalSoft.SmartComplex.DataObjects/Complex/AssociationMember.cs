using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using ThanalSoft.SmartComplex.DataObjects.Security;

namespace ThanalSoft.SmartComplex.DataObjects.Complex
{
    [DataContract]
    [Table("tblAssociationMember")]
    public class AssociationMember : BaseModel
    {
        [DataMember]
        [Required]
        public int AssociationId { get; set; }

        [DataMember]
        [Required]
        public Int64 UserId { get; set; }

        [DataMember]
        [Required]
        public int AssociationMemberRankId { get; set; }

        [DataMember]
        [Required]
        public DateTime From { get; set; }

        [DataMember]
        [Required]
        public DateTime To { get; set; }

        [DataMember]
        [Required]
        public bool IsActive { get; set; }

        [ForeignKey("AssociationId")]
        public virtual Association Association { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("AssociationMemberRankId")]
        public AssociationMemberRank AssociationMemberRank { get; set; }
    }
}