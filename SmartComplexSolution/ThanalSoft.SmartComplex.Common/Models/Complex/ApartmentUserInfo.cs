using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using ThanalSoft.SmartComplex.Common.Attributes;

namespace ThanalSoft.SmartComplex.Common.Models.Complex
{
    [DataContract]
    [Table(true)]
    public class ApartmentUserInfo
    {
        [DataMember]
        [TableColumn("Id", HiddenColumn = true, IDColumn = true)]
        public int Id { get; set; }
        
        [DataMember]
        [Required]
        [StringLength(250)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [DataMember]
        [Required]
        [StringLength(250)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DataMember]
        [TableColumn("Name")]
        public string Name => FirstName + (string.IsNullOrEmpty(LastName) ? "" : " " + LastName);

        [DataMember]
        [StringLength(25)]
        [Required]
        [Display(Name = "Mobile")]
        [TableColumn("Mobile")]
        public string Mobile { get; set; }

        [DataMember]
        [StringLength(256)]
        [Required]
        [Display(Name = "Email")]
        [TableColumn("Email")]
        public string Email { get; set; }

        [DataMember]
        [Display(Name = "Blood Group")]
        [TableColumn("Blood Group")]
        public string BloodGroup { get; set; }

        [DataMember]
        [Required]
        [Display(Name = "Owner")]
        [TableColumn("Owner")]
        public bool IsOwner { get; set; }

        [DataMember]
        [Display(Name = "Blood Group")]
        public int? BloodGroupId { get; set; }

        [DataMember]
        [Required]
        [Display(Name = "Locked")]
        [TableColumn("Locked")]
        public bool IsLocked { get; set; }

        [DataMember]
        [Display(Name = "Locked Date")]
        public DateTime? LockedDate { get; set; }

        [DataMember]
        [Display(Name = "Lock Reason")]
        public string LockReason { get; set; }

        [DataMember]
        public FlatInfo[] UserFlats { get; set; }

        [DataMember]
        public int ApartmentId { get; set; }

        [DataMember]
        public string ApartmentName { get; set; }

    }
}