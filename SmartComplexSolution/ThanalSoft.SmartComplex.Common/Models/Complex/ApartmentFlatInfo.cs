using System.Runtime.Serialization;

namespace ThanalSoft.SmartComplex.Common.Models.Complex
{
    [DataContract]
    public class ApartmentFlatInfo
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int ApartmentId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Floor { get; set; }

        [DataMember]
        public string Block { get; set; }

        [DataMember]
        public string Phase { get; set; }

        [DataMember]
        public int ExtensionNumber { get; set; }

        [DataMember]
        public int SquareFeet { get; set; }

        [DataMember]
        public ApartmentFlatUserInfo[] ApartmentFlatUsers { get; set; }
    }
}