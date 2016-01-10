using System.Runtime.Serialization;

namespace ThanalSoft.SmartComplex.Common.Models.Common
{
    public class GeneralInfo
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}