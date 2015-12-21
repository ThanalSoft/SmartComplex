using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ThanalSoft.SmartComplex.DataObjects.Complex
{
    [DataContract]
    [Table("tblApartmentAmenity")]
    public class ApartmentAmenity : BaseModel
    {
        [DataMember]
        [Required]
        public int ApartmentId { get; set; }

        [DataMember]
        [Required]
        public int AminityTypeId { get; set; }
        
        [DataMember]
        [Required]
        public bool IsBillable { get; set; }

        [ForeignKey("ApartmentId")]
        public virtual Apartment Apartment{ get; set; }

        [ForeignKey("AminityTypeId")]
        public virtual AmenityType AmenityType { get; set; }
    }
}
