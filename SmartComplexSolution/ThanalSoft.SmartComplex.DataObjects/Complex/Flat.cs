using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ThanalSoft.SmartComplex.DataObjects.Complex
{
    [DataContract]
    [Table("tblFlat")]
    public class Flat : BaseModel
    {
        [Required]
        [DataMember]
        public int ApartmentId { get; set; }

        [DataMember]
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [DataMember]
        public int? FlatTypeId { get; set; }

        [DataMember]
        [Required]
        public int Floor { get; set; }

        [Required]
        [StringLength(10)]
        public string Block { get; set; }

        [DataMember]
        [StringLength(10)]
        public string Phase { get; set; }

        [DataMember]
        public int? ExtensionNumber { get; set; }

        [DataMember]
        public int? SquareFeet { get; set; }

        [ForeignKey("ApartmentId")]
        public Apartment Apartment { get; set; }
        
        [ForeignKey("FlatTypeId")]
        public FlatType FlatType { get; set; }

        public virtual ICollection<MemberFlat> MemberFlats { get; set; }

    }
}