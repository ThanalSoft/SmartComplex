using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using ThanalSoft.SmartComplex.DataObjects.Common;
using ThanalSoft.SmartComplex.DataObjects.Security;

namespace ThanalSoft.SmartComplex.DataObjects.Complex
{
    [DataContract]
    [Table("tblFlatUser")]
    public class FlatUser : BaseModel
    {
        [DataMember]
        [Required]
        public Int64 UserId { get; set; }

        [DataMember]
        [Required]
        public int FlatId { get; set; }

        [DataMember]
        [Required]
        [StringLength(250)]
        public string FirstName { get; set; }

        [DataMember]
        [StringLength(250)]
        public string LastName { get; set; }

        [DataMember]
        [StringLength(250)]
        [Required]
        public string Mobile { get; set; }

        [DataMember]
        [Required]
        public bool IsOwner { get; set; }

        [DataMember]
        public int? BloodGroupId { get; set; }

        [DataMember]
        [Required]
        public bool IsLocked { get; set; }

        [DataMember]
        public DateTime? LockedDate { get; set; }

        [DataMember]
        public string LockReason { get; set; }

        [DataMember]
        [Required]
        public bool IsDeleted { get; set; }

        [ForeignKey("FlatId")]
        public virtual Flat Flat { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("BloodGroupId")]
        public virtual BloodGroup BloodGroup { get; set; }
        
    }
}