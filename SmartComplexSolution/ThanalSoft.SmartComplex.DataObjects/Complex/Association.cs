using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ThanalSoft.SmartComplex.DataObjects.Complex
{
    [DataContract]
    [Table("tblAssociation")]
    public  class Association : BaseModel
    {
        [DataMember]
        [Required]
        public int ApartmentId { get; set; }

        [DataMember]
        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [DataMember]
        [Required]
        [StringLength(150)]
        public string Description { get; set; }

        [DataMember]
        [Required]
        public int CreationDate { get; set; }

        [ForeignKey("ApartmentId")]
        public virtual Apartment Apartment { get; set; }

        public virtual ICollection<AssociationMember> AssociationMembers { get; set; }
    }
}