using System.Threading.Tasks;
using System.Web.Mvc;
using ThanalSoft.SmartComplex.Common;
using ThanalSoft.SmartComplex.Common.Models.Complex;
using ThanalSoft.SmartComplex.Web.Common;
using ThanalSoft.SmartComplex.Web.Models.ApartmentUser;
using ThanalSoft.SmartComplex.Web.Models.Common;
using ThanalSoft.SmartComplex.Web.Models.Flat;

namespace ThanalSoft.SmartComplex.Web.Controllers
{
    public class ApartmentUserController : BaseSecuredController
    {
        [HttpGet]
        public async Task<PartialViewResult> GetAllList(int pApartmentId)
        {
            var flats = await GetUsers(pApartmentId);
            return PartialView("_ApartmentUserList", new ApartmentUserListViewModel
            {
                Users = flats.Info,
                ActionResultStatus = (ActionResultStatusViewModel)TempData["Status"]
            });
        }

        private async Task<GeneralReturnInfo<ApartmentUserInfo[]>> GetUsers(int pApartmentId)
        {
            return await new ApiConnector<GeneralReturnInfo<ApartmentUserInfo[]>>().SecureGetAsync("Apartment", "GetApartmentUsers", LoggedInUser, pApartmentId.ToString());
        }
    }
}