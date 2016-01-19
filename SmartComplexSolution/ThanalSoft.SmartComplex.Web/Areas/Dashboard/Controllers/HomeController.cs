using System.Web.Mvc;
using ThanalSoft.SmartComplex.Web.Controllers;

namespace ThanalSoft.SmartComplex.Web.Areas.Dashboard.Controllers
{
    public class HomeController : BaseSecuredController
    {
        public ActionResult Index()
        {
            
            return View();
        }
    }
}