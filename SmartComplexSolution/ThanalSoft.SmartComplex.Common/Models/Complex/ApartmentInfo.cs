using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ThanalSoft.SmartComplex.Common.Models.Complex
{
    [DataContract]
    public class ApartmentInfo
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required]
        [StringLength(250)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [DataMember]
        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [DataMember]
        [Required]
        [StringLength(150)]
        [Display(Name = "City")]
        public string City { get; set; }

        [DataMember]
        [Required]
        [Display(Name = "State")]
        public int StateId { get; set; }

        [DataMember]
        [Required]
        [Display(Name = "Pin Code")]
        public int PinCode { get; set; }

        [DataMember]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [DataMember]
        [Display(Name = "Created On")]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public string State { get; set; }

        [DataMember]
        public bool IsLocked { get; set; }

        [DataMember]
        public DateTime? LockedDate { get; set; }

        [DataMember]
        [Display(Name = "Reason")]
        public string LockReason { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }

        [DataMember]
        public bool HasFlats { get; set; }
    }
}