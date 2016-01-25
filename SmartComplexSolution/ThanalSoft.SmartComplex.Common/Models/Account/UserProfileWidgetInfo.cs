using System.Runtime.Serialization;

namespace ThanalSoft.SmartComplex.Common.Models.Account
{
    [DataContract]
    public class UserProfileWidgetInfo
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Mobile { get; set; }
        [DataMember]
        public string BloodGroup { get; set; }
        [DataMember]
        public int MessageCount { get; set; }
        [DataMember]
        public int DiscussionCount { get; set; }
        [DataMember]
        public int ReminderCount { get; set; }
    }
}