using System.Runtime.Serialization;

namespace ThanalSoft.SmartComplex.Common.Models.Dashboard
{
    [DataContract]
    public class AdminDashboardInfo
    {
        [DataMember]
        public int TotalApartments { get; set; }
        [DataMember]
        public int TotalDeletedApartments { get; set; }
        [DataMember]
        public int TotalFlats { get; set; }
        [DataMember]
        public int TotalActiveUsers { get; set; }
        [DataMember]
        public int TotalInactiveUsers { get; set; }
        [DataMember]
        public int TotalLoggedinUsers { get; set; }
    }
}