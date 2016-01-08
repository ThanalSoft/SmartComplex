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

        [TableColumn("Name")]
        [DataMember]
        public string Name { get; set; }

        [TableColumn("Floor")]
        [DataMember]
        public int Floor { get; set; }

        [TableColumn("Block")]
        [DataMember]
        public string Block { get; set; }

        [TableColumn("Phase")]
        [DataMember]
        public string Phase { get; set; }

        [TableColumn("Ext. No")]
        [DataMember]
        public int? ExtensionNumber { get; set; }

        [TableColumn("Sqft")]
        [DataMember]
        public int? SquareFeet { get; set; }

        [DataMember]
        public FlatUserInfo[] ApartmentFlatUsers { get; set; }
    }
}