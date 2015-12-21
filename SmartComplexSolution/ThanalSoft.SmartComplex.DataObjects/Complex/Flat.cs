using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ThanalSoft.SmartComplex.DataObjects.Complex
{
    [DataContract]
    [Table("tblFlat")]
    public class Flat : BaseModel
    {
        [DataMember]
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [DataMember]
        [Required]
        public int Floor { get; set; }

        [DataMember]
        public int FlatBlockId { get; set; }
        
        [DataMember]
        public int ExtensionNumber { get; set; }

        [DataMember]
        public int SquareFeet { get; set; }

        [ForeignKey("FlatBlockId")]
        public virtual FlatBlock FlatBlock { get; set; }

        public virtual ICollection<FlatUser> FlatUsers { get; set; }

    }
}