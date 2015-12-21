using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ThanalSoft.SmartComplex.DataObjects.Complex
{
    [DataContract]
    [Table("tblAssociationMemberRank")]
    public class AssociationMemberRank : BaseModel
    {
        [DataMember]
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [DataMember]
        [NotMapped]
        public override DateTime LastUpdated { get; set; }

        [DataMember]
        [NotMapped]
        public override int LastUpdatedBy { get; set; }

        public virtual ICollection<AssociationMember> AssociationMembers { get; set; }
    }
}