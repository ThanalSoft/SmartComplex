using System.Web.Mvc;

namespace ThanalSoft.SmartComplex.Web.Controllers
{
    public class HomeController : BaseSecuredController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}