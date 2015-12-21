using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using ThanalSoft.SmartComplex.DataObjects.UserUtilities;

namespace ThanalSoft.SmartComplex.DataObjects.Complex
{
    [DataContract]
    [Table("tblAmenityType")]
    public class AmenityType : BaseModel
    {
        [DataMember]
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [DataMember]
        [Required]
        public bool IsActive { get; set; }

        [DataMember]
        [NotMapped]
        public override DateTime LastUpdated { get; set; }

        [DataMember]
        [NotMapped]
        public override int LastUpdatedBy { get; set; }

        public virtual ICollection<ApartmentAmenity> ApartmentAmenities { get; set; }

        public virtual ICollection<AmenityCalendar> AminityCalendars { get; set; }

    }
}