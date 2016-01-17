using ThanalSoft.SmartComplex.Web.Models.Common;

namespace ThanalSoft.SmartComplex.Web.Models
{
    public abstract class BaseViewModel
    {
        public ActionResultStatusViewModel ActionResultStatus { get; set; }

        public bool IsAsyncRequest { get; set; }
    }
}