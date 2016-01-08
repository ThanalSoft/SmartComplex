using System.Threading.Tasks;
using System.Web.Mvc;
using ThanalSoft.SmartComplex.Common;
using ThanalSoft.SmartComplex.Common.Models.Complex;
using ThanalSoft.SmartComplex.Web.Common;
using ThanalSoft.SmartComplex.Web.Models.Apartment;

namespace ThanalSoft.SmartComplex.Web.Controllers
{
    public class FlatController : BaseSecuredController
    {
        [HttpGet]
        public async Task<PartialViewResult> GetAll(int pApartmentId)
        {
            var flats = await GetFlats(pApartmentId);
            return PartialView("_FlatList", new FlatListViewModel { Flats = flats.Info });
        }

        private async Task<GeneralReturnInfo<FlatInfo[]>> GetFlats(int pApartmentId)
        {
            return await new ApiConnector<GeneralReturnInfo<FlatInfo[]>>().SecureGetAsync("Flat", "GetAll", LoggedInUser, pApartmentId.ToString());
        }
    }
}