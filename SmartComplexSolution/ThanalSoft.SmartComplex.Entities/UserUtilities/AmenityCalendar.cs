using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using ThanalSoft.SmartComplex.Entities.Complex;
using ThanalSoft.SmartComplex.Entities.Security;

namespace ThanalSoft.SmartComplex.Entities.UserUtilities
{
    [DataContract]
    [Table("tblAmenityCalendar")]
    public class AmenityCalendar : BaseModel
    {
        [Required]
        [DataMember]
        public int AminityTypeId { get; set; }

        [Required]
        [DataMember]
        public Int64 BookedUserId { get; set; }

        [Required]
        [DataMember]
        public DateTime From { get; set; }

        [Required]
        [DataMember]
        public DateTime To { get; set; }

        [Required]
        [DataMember]
        public string Reason { get; set; }

        [ForeignKey("AminityTypeId")]
        public virtual AmenityType AmenityType { get; set; }

        [ForeignKey("BookedUserId")]
        public virtual LoginUser User { get; set; }
    }
}