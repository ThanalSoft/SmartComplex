using System.Runtime.Serialization;

namespace ThanalSoft.SmartComplex.Web.Models.Common
{
    [DataContract]
    public class ActionResultStatusViewModel
    {
        public ActionResultStatusViewModel(string pMessage, ActionStatus pActionStatus)
        {
            Message = pMessage;
            ActionStatus = pActionStatus;
        }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public ActionStatus ActionStatus { get; set; }
    }

    [DataContract]
    public enum ActionStatus
    {
        [EnumMember]Success,
        [EnumMember]Information,
        [EnumMember]Error,
        [EnumMember]Warning
    }
}