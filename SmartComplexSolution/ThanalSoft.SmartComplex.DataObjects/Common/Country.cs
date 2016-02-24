using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ThanalSoft.SmartComplex.DataObjects.Common
{
    [DataContract]
    [Table("tblCountry")]
    public class Country : BaseModel
    {
        public Country()
        {
            States = new HashSet<State>();
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

        [DataMember]
        [NotMapped]
        public override DateTime LastUpdated { get; set; }

        [DataMember]
        [NotMapped]
        public override Int64 LastUpdatedBy { get; set; }
    }
}