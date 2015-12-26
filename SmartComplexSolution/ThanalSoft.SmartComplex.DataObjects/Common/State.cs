using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using ThanalSoft.SmartComplex.DataObjects.Complex;

namespace ThanalSoft.SmartComplex.DataObjects.Common
{
    [DataContract]
    [Table("tblState")]
    public class State : BaseModel
    {
        public State()
        {
            Apartments = new List<Apartment>();
        }

        [DataMember]
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [DataMember]
        [Required]
        public int CountryId { get; set; }

        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }

        public virtual ICollection<Apartment> Apartments { get; set; }

        [DataMember]
        [NotMapped]
        public override DateTime LastUpdated { get; set; }

        [DataMember]
        [NotMapped]
        public override Int64 LastUpdatedBy { get; set; }
    }
}