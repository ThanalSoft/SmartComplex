using System.Threading.Tasks;
using System.Web.Mvc;
using ThanalSoft.SmartComplex.Web.Common;

namespace ThanalSoft.SmartComplex.Web.Controllers
{
    public class HomeController : BaseSecuredController
    {
        public async Task<ActionResult> Index()
        {
            var loginResponse = await new ApiConnector<string>().SecureGetAsync("Account", "Test", LoggedInUser);

            return View();
        }
    }
}