namespace ThanalSoft.SmartComplex.Web.Models
{
    public class ActionResultStatusViewModel
    {
        public ActionResultStatusViewModel(string pData, ActionStatus pActionStatus)
        {
            Data = pData;
            ActionStatus = pActionStatus;
        }
        public string Data { get; set; }

        public ActionStatus ActionStatus { get; set; }
    }

    public enum ActionStatus
    {
        Success,
        Information,
        Error,
        Warning
    }
}