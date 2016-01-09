using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ThanalSoft.SmartComplex.DataObjects.Complex
{
    [DataContract]
    [Table("tblMemberFlat")]
    public class MemberFlat : BaseModel
    {
        [DataMember]
        [Required]
        public int FlatId { get; set; }

        [DataMember]
        [Required]
        public int FlatUserId { get; set; }

        [ForeignKey("FlatUserId")]
        public virtual FlatUser FlatUser { get; set; }

        [ForeignKey("FlatId")]
        public virtual Flat Flat { get; set; }
    }
}
