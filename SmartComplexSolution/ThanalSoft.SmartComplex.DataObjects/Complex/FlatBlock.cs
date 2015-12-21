using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ThanalSoft.SmartComplex.DataObjects.Complex
{
    [DataContract]
    [Table("tblFlatBlock")]
    public class FlatBlock : BaseModel
    {
        [DataMember]
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [DataMember]
        public int ApartmentId { get; set; }

        [ForeignKey("ApartmentId")]
        public Apartment Apartment { get; set; }
    }
}