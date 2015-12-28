using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ThanalSoft.SmartComplex.Common;
using ThanalSoft.SmartComplex.Common.Models.Common;
using ThanalSoft.SmartComplex.Common.Models.Complex;
using ThanalSoft.SmartComplex.Web.Common;
using ThanalSoft.SmartComplex.Web.Models;

namespace ThanalSoft.SmartComplex.Web.Controllers
{
    public class ApartmentController : BaseSecuredController
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var response = await GetApartmentInfoList();
            return View(new ApartmentListViewModel
            {
                Apartments = response.Info.ToArray(),
                ActionResultStatus = (ActionResultStatusViewModel)TempData["Status"]
            });
        }

        [HttpGet]
        public async Task<PartialViewResult> IndexList()
        {
            var response = await GetApartmentInfoList();
            return PartialView("_IndexList", new ApartmentListViewModel
            {
                Apartments = response.Info.ToArray(),
                ActionResultStatus = (ActionResultStatusViewModel)TempData["Status"]
            });
        }
        
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            return View(new ApartmentViewModel
            {
                States = await GetStatesAsync()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ApartmentViewModel pModel)
        {
            if (!ModelState.IsValid)
            {
                pModel.States = await GetStatesAsync();
                return View(pModel);
            }
            try
            {
                var result = await new ApiConnector<GeneralReturnInfo>().SecurePostAsync("Apartment", "Create", LoggedInUser, pModel.ApartmentInfo);
                if (result.Result == ApiResponseResult.Success)
                {
                    TempData["Status"] = new ActionResultStatusViewModel("Apartment created successfully!", ActionStatus.Success);
                    return RedirectToAction("IndexList");
                }
                pModel.ActionResultStatus = new ActionResultStatusViewModel("Error! Reason: " + string.Format(result.Reason, "Apartment"), ActionStatus.Error);
            }
            catch (Exception ex)
            {
                pModel.ActionResultStatus = new ActionResultStatusViewModel("Error occured while creating Apartment. Exception: " + ex.Message, ActionStatus.Error);
            }
            pModel.States = await GetStatesAsync();
            return View(pModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(ApartmentViewModel pModel)
        {
            if (!ModelState.IsValid)
            {
                pModel.States = await GetStatesAsync();
                return View(pModel);
            }
            try
            {
                var result = await new ApiConnector<GeneralReturnInfo>().SecurePostAsync("Apartment", "Update", LoggedInUser, pModel.ApartmentInfo);
                if (result.Result == ApiResponseResult.Success)
                {
                    TempData["Status"] = new ActionResultStatusViewModel("Apartment updated successfully!", ActionStatus.Success);
                    return RedirectToAction("IndexList");
                }
                pModel.ActionResultStatus = new ActionResultStatusViewModel("Error! Reason: " + string.Format(result.Reason, "Apartment"), ActionStatus.Error);
            }
            catch (Exception ex)
            {
                pModel.ActionResultStatus = new ActionResultStatusViewModel("Error occured while updating Apartment. Exception: " + ex.Message, ActionStatus.Error);
            }
            pModel.States = await GetStatesAsync();
            return View(pModel);
        }

        [HttpGet]
        public async Task<ActionResult> Update(int pId)
        {
            var response = await new ApiConnector<GeneralReturnInfo<ApartmentInfo>>().SecureGetAsync("Apartment", "Get", LoggedInUser, pId.ToString());
            return View(new ApartmentViewModel
            {
                States = await GetStatesAsync(),
                ApartmentInfo = response.Info
            });
        }

        [HttpGet]
        public async Task<ActionResult> View(int pId)
        {
            var response = await new ApiConnector<GeneralReturnInfo<ApartmentInfo>>().SecureGetAsync("Apartment", "Get", LoggedInUser, pId.ToString());
            return View(new ApartmentViewModel
            {
                ApartmentInfo = response.Info
            });
        }

        private async Task<List<SelectListItem>> GetStatesAsync()
        {
            var response = await new ApiConnector<GeneralReturnInfo<StateInfo[]>>().SecureGetAsync("Common", "GetStates", LoggedInUser);
            var stateDdl = new List<SelectListItem>
            {
                new SelectListItem()
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

        private async Task<GeneralReturnInfo<ApartmentInfo[]>> GetApartmentInfoList()
        {
            var response = await new ApiConnector<GeneralReturnInfo<ApartmentInfo[]>>().SecureGetAsync("Apartment", "GetAll", LoggedInUser);
            return response;
        }
    }
}