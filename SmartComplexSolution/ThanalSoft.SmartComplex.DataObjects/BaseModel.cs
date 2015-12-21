using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ThanalSoft.SmartComplex.DataObjects
{
    [DataContract]
    public class BaseModel
    {
        [DataMember]
        [Key]
        [Column(Order = 1)]
        public int Id { get; set; }

        [DataMember]
        public virtual DateTime LastUpdated { get; set; }

        [DataMember]
        public virtual int LastUpdatedBy { get; set; }
    }
}