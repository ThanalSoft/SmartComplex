using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ThanalSoft.SmartComplex.Entities.Complex
{
    [DataContract]
    [Table("tblFlatType")]
    public class FlatType : BaseModel
    {
        [DataMember]
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [DataMember]
        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        [DataMember]
        [Required]
        public bool IsActive { get; set; }

        [DataMember]
        [NotMapped]
        public override DateTime LastUpdated { get; set; }

        [DataMember]
        [NotMapped]
        public override Int64 LastUpdatedBy { get; set; }

        public virtual ICollection<Flat> Flats { get; set; }

    }
}