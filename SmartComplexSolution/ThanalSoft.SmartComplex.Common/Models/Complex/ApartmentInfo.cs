using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using ThanalSoft.SmartComplex.Common.Attributes;

namespace ThanalSoft.SmartComplex.Common.Models.Complex
{
    [DataContract]
    [Table(true)]
    public class ApartmentInfo
    {
        [DataMember]
        [TableColumn("Id", true, true)]
        public int Id { get; set; }

        [DataMember]
        [Required]
        [StringLength(250)]
        [Display(Name = "Name")]
        [TableColumn("Name")]
        public string Name { get; set; }

        [DataMember]
        [Required]
        [Display(Name = "Address")]
        [TableColumn("Address")]
        public string Address { get; set; }

        [DataMember]
        [Required]
        [StringLength(150)]
        [Display(Name = "City")]
        [TableColumn("City")]
        public string City { get; set; }

        [DataMember]
        [Required]
        [Display(Name = "State")]
        public int StateId { get; set; }

        [DataMember]
        [TableColumn("State")]
        public string State { get; set; }

        [DataMember]
        [Required]
        [Display(Name = "Pin Code")]
        [TableColumn("Pin Code")]
        public int PinCode { get; set; }

        [DataMember]
        [Display(Name = "Phone")]
        [TableColumn("Phone")]
        public string Phone { get; set; }

        [DataMember]
        [Display(Name = "Created On")]
        public DateTime CreatedDate { get; set; }
        
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