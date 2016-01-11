using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using ThanalSoft.SmartComplex.Common.Attributes;

namespace ThanalSoft.SmartComplex.Common.Models.Complex
{
    [DataContract]
    [Table(true)]
    public class FlatInfo
    {
        [TableColumn("Id", IDColumn = true, HiddenColumn = true)]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int ApartmentId { get; set; }

        [DataMember]
        public string ApartmentName { get; set; }

        [TableColumn("Name")]
        [Display(Name = "Name")]
        [StringLength(50)]
        [DataMember]
        [Required]
        public string Name { get; set; }

        [TableColumn("Floor")]
        [Display(Name = "Floor")]
        [DataMember]
        [Required]
        public int? Floor { get; set; }

        [TableColumn("Block")]
        [Display(Name = "Block")]
        [DataMember]
        [StringLength(10)]
        public string Block { get; set; }

        [TableColumn("Phase")]
        [Display(Name = "Phase")]
        [DataMember]
        [StringLength(10)]
        public string Phase { get; set; }

        [TableColumn("Sqft")]
        [Display(Name = "Square Feet")]
        [DataMember]
        public int? SquareFeet { get; set; }

        [DataMember]
        [Display(Name = "Flat Type")]
        public int? FlatTypeId { get; set; }

        [TableColumn("Flat Type")]
        [DataMember]
        public string FlatType { get; set; }

        [TableColumn("Ext No.")]
        [Display(Name = "Extension No.")]
        [DataMember]
        public int? ExtensionNumber { get; set; }

        [DataMember]
        public FlatUserInfo[] ApartmentFlatUsers { get; set; }
    }
}