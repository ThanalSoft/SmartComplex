using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using ThanalSoft.SmartComplex.Common;
using ThanalSoft.SmartComplex.Common.Models.Common;
using ThanalSoft.SmartComplex.Common.Models.Dashboard;
using ThanalSoft.SmartComplex.Web.Common;
using ThanalSoft.SmartComplex.Web.Models.Home;

namespace ThanalSoft.SmartComplex.Web.Controllers
{
    public class HomeController : BaseSecuredController
    {
        public async Task<ActionResult> Index()
        {
            var viewModel = new DashboardViewModel();
            
                var dahboard = await new ApiConnector<GeneralReturnInfo<AdminDashboardInfo>>().SecureGetAsync("Dashboard", "GetAdministratorDashboard");
                viewModel.AdminDashboardInfo = dahboard.Info;
            

            return View(viewModel);
        }

        [HttpGet]
        public async Task<int> GetUserNotificationCount()
        {
            if (Thread.CurrentPrincipal == null)
                RedirectToAction("Index", "Account");

            var notifications = await new ApiConnector<GeneralReturnInfo<int>>().SecureGetAsync("Common", "GetUserNotificationCount", User.UserId.ToString());
            return notifications.Info;
        }

        [HttpGet]
        public async Task<ActionResult> ReadNotifications()
        {
            var notifications = await new ApiConnector<GeneralReturnInfo<NotificationInfo[]>>().SecureGetAsync("Common", "ReadUserNotifications", User.UserId.ToString());
            return PartialView("Partials/_Notification", notifications.Info);
        }

        public async Task<ActionResult> Notifications()
        {
            var notifications = await new ApiConnector<GeneralReturnInfo<NotificationInfo[]>>().SecureGetAsync("Common", "GetUserNotifications", User.UserId.ToString());
            return View(notifications.Info);
        }
    }
}