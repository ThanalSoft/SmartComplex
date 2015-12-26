using System.Runtime.Serialization;

namespace ThanalSoft.SmartComplex.Common.Models.Common
{
    [DataContract]
    public class StateInfo
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
}