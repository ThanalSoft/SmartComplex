using System.Threading.Tasks;
using System.Web.Mvc;
using ThanalSoft.SmartComplex.Common;
using ThanalSoft.SmartComplex.Common.Models.Common;
using ThanalSoft.SmartComplex.Web.Common;

namespace ThanalSoft.SmartComplex.Web.Controllers
{
    public class HomeController : BaseSecuredController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetNotifications()
        {
            var notifications = await new ApiConnector<GeneralReturnInfo<NotificationInfo[]>>().SecureGetAsync("Common", "GetNotifications", LoggedInUser, LoggedInUser.UserId.ToString());
            return PartialView("Partials/_Notification", notifications.Info);
        }

        [HttpGet]
        public async Task<ActionResult> ReadNotifications()
        {
            var notifications = await new ApiConnector<GeneralReturnInfo<NotificationInfo[]>>().SecureGetAsync("Common", "ReadNotifications", LoggedInUser, LoggedInUser.UserId.ToString());
            return PartialView("Partials/_Notification", notifications.Info);
        }
    }
}