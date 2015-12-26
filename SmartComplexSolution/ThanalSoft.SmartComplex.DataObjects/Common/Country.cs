using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using ThanalSoft.SmartComplex.DataObjects.Complex;

namespace ThanalSoft.SmartComplex.DataObjects.Common
{
    [DataContract]
    [Table("tblCountry")]
    public class Country : BaseModel
    {
        public Country()
        {
            States = new List<State>();
        }

        [DataMember]
        [Required]
        [StringLength(5)]
        public string Code { get; set; }

        [DataMember]
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public virtual ICollection<State> States { get; set; }

        public virtual ICollection<Apartment> Apartments { get; set; }

        [DataMember]
        [NotMapped]
        public override DateTime LastUpdated { get; set; }

        [DataMember]
        [NotMapped]
        public override Int64 LastUpdatedBy { get; set; }
    }
}