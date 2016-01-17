using System.Web.Mvc;
using ThanalSoft.SmartComplex.Web.Models.Common;
using ThanalSoft.SmartComplex.Web.Security;

namespace ThanalSoft.SmartComplex.Web.Controllers
{
    [Authorize]
    public class BaseSecuredController : Controller
    {
        protected virtual new SmartComplexPrincipal User => HttpContext.User as SmartComplexPrincipal;

        public bool IsAjaxRequest => Request.IsAjaxRequest();

        public ActionResultStatusViewModel ViewResultStatus
        {
            get { return TempData["Status"] as ActionResultStatusViewModel; }
            set { TempData["Status"] = value; }
        }
    }
}