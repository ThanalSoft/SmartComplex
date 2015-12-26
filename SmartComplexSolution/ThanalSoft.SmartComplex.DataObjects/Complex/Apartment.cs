using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using ThanalSoft.SmartComplex.DataObjects.Common;

namespace ThanalSoft.SmartComplex.DataObjects.Complex
{
    [DataContract]
    [Table("tblApartment")]
    public class Apartment : BaseModel
    {
        [DataMember]
        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [DataMember]
        [Required]
        public string Address { get; set; }

        [DataMember]
        [Required]
        [StringLength(150)]
        public string City { get; set; }

        [DataMember]
        [Required]
        public int StateId { get; set; }

        [DataMember]
        [Required]
        public int PinCode { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        [Required]
        public bool IsLocked { get; set; }

        [DataMember]
        [Required]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public DateTime? LockedDate { get; set; }

        [DataMember]
        public string LockReason { get; set; }

        [DataMember]
        [Required]
        public bool IsDeleted { get; set; }

        [ForeignKey("StateId")]
        public virtual State State { get; set; }
        
        public virtual ICollection<ApartmentAmenity> ApartmentAmenities { get; set; }

        public virtual ICollection<FlatBlock> FlatBlocks { get; set; }

        public virtual ICollection<Association> Associations { get; set; }


    }
}