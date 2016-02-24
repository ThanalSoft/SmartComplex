using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using ThanalSoft.SmartComplex.DataObjects.Security;

namespace ThanalSoft.SmartComplex.DataObjects.Common
{
    [DataContract]
    [Table("tblBloodGroup")]
    public class BloodGroup : BaseModel
    {
        [DataMember]
        [StringLength(5)]
        [Required]
        public string Group { get; set; }

        [DataMember]
        [NotMapped]
        public override DateTime LastUpdated { get; set; }

        [DataMember]
        [NotMapped]
        public override Int64 LastUpdatedBy { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}