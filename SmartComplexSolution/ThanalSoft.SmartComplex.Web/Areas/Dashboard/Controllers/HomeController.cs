using System.Web.Mvc;
using ThanalSoft.SmartComplex.Web.Areas.Dashboard.Models;
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

        public PartialViewResult GetProfileData()
        {
            var profile = new DashboardProfileViewModel
            {
                Name = User.Name,
                Email = User.Email
            };

            return PartialView("_UserProfile", profile);
        }
    }
}