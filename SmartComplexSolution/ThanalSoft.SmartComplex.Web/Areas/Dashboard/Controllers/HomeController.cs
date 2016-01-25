using System.Threading.Tasks;
using System.Web.Mvc;
using ThanalSoft.SmartComplex.Common;
using ThanalSoft.SmartComplex.Common.Models.Account;
using ThanalSoft.SmartComplex.Common.Models.Complex;
using ThanalSoft.SmartComplex.Web.Areas.Dashboard.Models;
using ThanalSoft.SmartComplex.Web.Common;
using ThanalSoft.SmartComplex.Web.Controllers;

namespace ThanalSoft.SmartComplex.Web.Areas.Dashboard.Controllers
{
    public class HomeController : BaseSecuredController
    {
        public ActionResult Index()
        {
            var profile = new DashboardProfileViewModel
            {
                Name = User.Name,
                Email = User.Email
            };

            return View(new DashboardViewModel
            {
                DashboardProfileViewModel = profile
            });
        }

        public async Task<PartialViewResult> GetUserProfileWidget()
        {
            var result = await GetUserProfileWidgetInfo();
            var profile = new DashboardProfileViewModel
            {
                Name = result.Info.Name,
                Email = result.Info.Email,
                Phone = result.Info.Mobile,
                BloodGroup = string.IsNullOrEmpty(result.Info.BloodGroup) ? "Not Specified" : result.Info.BloodGroup,
                DiscussionCount = result.Info.DiscussionCount,
                MessageCount = result.Info.MessageCount,
                ReminderCount = result.Info.ReminderCount
            };

            return PartialView("_UserProfileWidgetInfo", profile);
        }

        #region Private Methods

        [NonAction]
        private async Task<GeneralReturnInfo<UserProfileWidgetInfo>> GetUserProfileWidgetInfo()
        {
            var response = await new ApiConnector<GeneralReturnInfo<UserProfileWidgetInfo>>().SecureGetAsync("Account", "GetUserProfileWidgetInfo", User.UserId.ToString());
            return response;
        }

        #endregion
    }
}