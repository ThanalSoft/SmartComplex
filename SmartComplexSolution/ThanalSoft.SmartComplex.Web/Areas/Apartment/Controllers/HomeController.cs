using System.Threading.Tasks;
using System.Web.Mvc;
using ThanalSoft.SmartComplex.Common;
using ThanalSoft.SmartComplex.Common.Models.Complex;
using ThanalSoft.SmartComplex.Web.Areas.Apartment.Models;
using ThanalSoft.SmartComplex.Web.Common;
using ThanalSoft.SmartComplex.Web.Controllers;

namespace ThanalSoft.SmartComplex.Web.Areas.Apartment.Controllers
{
    [Authorize(Roles = "ApartmentAdmin")]
    public class HomeController : BaseSecuredController
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var response = await GetAllApartments();

            return View(new ApartmentListViewModel
            {
                Apartments = response.Info,
            });
        }

        #region Private Methods

        private async Task<GeneralReturnInfo<ApartmentInfo[]>> GetAllApartments()
        {
            var response = await new ApiConnector<GeneralReturnInfo<ApartmentInfo[]>>().SecureGetAsync("Apartment", "GetAll");
            return response;
        }

        #endregion
    }
}