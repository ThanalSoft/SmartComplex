using System.Threading.Tasks;
using System.Web.Mvc;
using ThanalSoft.SmartComplex.Common;
using ThanalSoft.SmartComplex.Common.Models.Complex;
using ThanalSoft.SmartComplex.Web.Common;
using ThanalSoft.SmartComplex.Web.Models.Apartment;
using ThanalSoft.SmartComplex.Web.Models.Common;
using ThanalSoft.SmartComplex.Web.Models.Flat;

namespace ThanalSoft.SmartComplex.Web.Controllers
{
    public class FlatController : BaseSecuredController
    {
        public async Task<ActionResult> Index(int pApartmentId)
        {
            var flats = await GetFlats(pApartmentId);
            return View(new FlatListViewModel
            {
                Flats = flats.Info,
                ActionResultStatus = (ActionResultStatusViewModel)TempData["Status"]
            });
        }

        [HttpGet]
        public async Task<PartialViewResult> GetAllList(int pApartmentId)
        {
            var flats = await GetFlats(pApartmentId);
            return PartialView("_FlatList", new FlatListViewModel { Flats = flats.Info });
        }

        [HttpGet]
        public async Task<ActionResult> View(int pId)
        {
            var response = await GetFlat(pId);

            return View(new FlatViewModel
            {
                FlatInfo = response.Info
            });
        }

        private async Task<GeneralReturnInfo<FlatInfo>> GetFlat(int pId)
        {
            return await new ApiConnector<GeneralReturnInfo<FlatInfo>>().SecureGetAsync("Flat", "Get", LoggedInUser, pId.ToString());
        }

        private async Task<GeneralReturnInfo<FlatInfo[]>> GetFlats(int pApartmentId)
        {
            return await new ApiConnector<GeneralReturnInfo<FlatInfo[]>>().SecureGetAsync("Flat", "GetAll", LoggedInUser, pApartmentId.ToString());
        }
    }
}