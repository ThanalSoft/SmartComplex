using System.Runtime.Serialization;

namespace ThanalSoft.SmartComplex.Common.Models.Complex
{
    [DataContract]
    public class FlatUploadInfo
    {
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
        public string OwnerName { get; set; }
        [DataMember]
        public string OwnerEmail { get; set; }
        [DataMember]
        public string OwnerMobile { get; set; }
    }
}