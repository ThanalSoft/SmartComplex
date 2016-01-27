using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ThanalSoft.SmartComplex.Common;
using ThanalSoft.SmartComplex.Common.Models.Common;
using ThanalSoft.SmartComplex.Common.Models.Complex;
using ThanalSoft.SmartComplex.Web.Areas.Apartment.Models;
using ThanalSoft.SmartComplex.Web.Common;
using ThanalSoft.SmartComplex.Web.Controllers;
using ThanalSoft.SmartComplex.Web.Models.Common;

namespace ThanalSoft.SmartComplex.Web.Areas.Apartment.Controllers
{
    [Authorize(Roles = "ApartmentAdmin,Owner,Tenant")]
    public class HomeController : BaseSecuredController
    {
        #region Get Methods

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var response = await GetUserApartments();
            return View(new ApartmentListViewModel
            {
                Apartments = response.Info,
                IsAsyncRequest = IsAjaxRequest
            });
        }

        public async Task<ActionResult> Get(int pApartmentId)
        {
            var response = await GetApartment(pApartmentId);

            return View(new ApartmentViewModel
            {
                IsAsyncRequest = IsAjaxRequest,
                Apartment = response.Info,
                ActionResultStatus = ViewResultStatus
            });
        }

        [HttpGet]
        public async Task<ActionResult> Update(int pApartmentId)
        {
            var response = await GetApartment(pApartmentId);
            return View(new ApartmentViewModel
            {
                IsAsyncRequest = IsAjaxRequest,
                ActionResultStatus = ViewResultStatus,
                States = await GetStates(),
                Apartment = response.Info
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(ApartmentViewModel pModel)
        {
            pModel.IsAsyncRequest = IsAjaxRequest;
            if (!ModelState.IsValid)
            {
                pModel.States = await GetStates();
                return View(pModel);
            }
            try
            {
                var result = await UpdateApartmentAsync(pModel.Apartment);
                if (result.Result == ApiResponseResult.Success)
                {
                    ViewResultStatus = new ActionResultStatusViewModel($"Apartment '{pModel.Apartment.Name}' updated successfully!", ActionStatus.Success);
                    return RedirectToAction("Get", new { pApartmentId = pModel.Apartment.Id});
                }
                pModel.ActionResultStatus = new ActionResultStatusViewModel("Error! Reason: " + result.Reason, ActionStatus.Error);
            }
            catch (Exception ex)
            {
                pModel.ActionResultStatus = new ActionResultStatusViewModel("Error occured while updating Apartment. Exception: " + ex.Message, ActionStatus.Error);
            }
            pModel.States = await GetStates();
            return View(pModel);
        }

        #endregion

        #region Private Methods

        [NonAction]
        private async Task<GeneralReturnInfo<ApartmentInfo[]>> GetUserApartments()
        {
            var response = await new ApiConnector<GeneralReturnInfo<ApartmentInfo[]>>().SecureGetAsync("Apartment", "GetUserApartments", User.UserId.ToString());
            return response;
        }

        [NonAction]
        private async Task<GeneralReturnInfo<ApartmentInfo>> GetApartment(int pId)
        {
            return await new ApiConnector<GeneralReturnInfo<ApartmentInfo>>().SecureGetAsync("Apartment", "Get", pId.ToString());
        }

        [NonAction]
        private async Task<List<SelectListItem>> GetStates()
        {
            var response = await new ApiConnector<GeneralReturnInfo<GeneralInfo[]>>().SecureGetAsync("Common", "GetStates");
            var stateDdl = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = null,
                    Text = "-- Select --"
                }
            };
            stateDdl.AddRange(response.Info.Select((pX => new SelectListItem
            {
                Text = pX.Name,
                Value = Convert.ToString(pX.Id)
            })));

            return stateDdl;
        }

        [NonAction]
        private async Task<GeneralReturnInfo> UpdateApartmentAsync(ApartmentInfo pApartmentInfo)
        {
            var result = await new ApiConnector<GeneralReturnInfo>().SecurePostAsync("Apartment", "Update", pApartmentInfo);
            return result;
        }

        #endregion
    }
}