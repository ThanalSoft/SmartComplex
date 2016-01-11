using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ThanalSoft.SmartComplex.Common;
using ThanalSoft.SmartComplex.Common.Models.Common;
using ThanalSoft.SmartComplex.Common.Models.Complex;
using ThanalSoft.SmartComplex.Web.Common;
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
            return PartialView("_FlatList", new FlatListViewModel
            {
                Flats = flats.Info,
                ActionResultStatus = (ActionResultStatusViewModel)TempData["Status"]
            });
        }

        [HttpGet]
        public async Task<ActionResult> View(int pId)
        {
            var response = await GetFlat(pId);

            return View(new FlatViewModel
            {
                FlatInfo = response.Info,
                ActionResultStatus = (ActionResultStatusViewModel)TempData["Status"]
            });
        }

        [HttpGet]
        public async Task<ActionResult> Create(int pApartmentId)
        {
            return View(new FlatViewModel
            {
                FlatInfo = new FlatInfo
                {
                    ApartmentId = pApartmentId,
                },
                FlatTypes = await GetFlatTypes()
            });
        }

        [HttpGet]
        public async Task<ActionResult> Update(int pId)
        {
            var response = await GetFlat(pId);
            return View(new FlatViewModel
            {
                FlatTypes = await GetFlatTypes(),
                FlatInfo = response.Info
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(FlatViewModel pModel)
        {
            if (!ModelState.IsValid)
            {
                return View(new FlatViewModel
                {
                    FlatInfo = new FlatInfo
                    {
                        ApartmentId = pModel.FlatInfo.ApartmentId,
                    },
                    FlatTypes = await GetFlatTypes()
                });
            }
            try
            {
                var result = await new ApiConnector<GeneralReturnInfo>().SecurePostAsync("Flat", "Create", LoggedInUser, pModel.FlatInfo);
                if (result.Result == ApiResponseResult.Success)
                {
                    TempData["Status"] = new ActionResultStatusViewModel("Flat created successfully!", ActionStatus.Success);
                    return RedirectToAction("GetAllList","Flat", new { pApartmentId = pModel.FlatInfo.ApartmentId });
                }
                pModel.ActionResultStatus = new ActionResultStatusViewModel("Error! Reason: " + string.Format(result.Reason, "Flat"), ActionStatus.Error);
            }
            catch (Exception ex)
            {
                pModel.ActionResultStatus = new ActionResultStatusViewModel("Error occured while creating Flat. Exception: " + ex.Message, ActionStatus.Error);
            }
            pModel.FlatTypes = await GetFlatTypes();
            return View(pModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(FlatViewModel pModel)
        {
            if (!ModelState.IsValid)
            {
                var response = await GetFlat(pModel.FlatInfo.Id);
                return View(new FlatViewModel
                {
                    FlatInfo = response.Info,
                    FlatTypes = await GetFlatTypes()
                });
            }
            try
            {
                var result = await new ApiConnector<GeneralReturnInfo>().SecurePostAsync("Flat", "Update", LoggedInUser, pModel.FlatInfo);
                if (result.Result == ApiResponseResult.Success)
                {
                    TempData["Status"] = new ActionResultStatusViewModel("Flat updated successfully!", ActionStatus.Success);
                    return RedirectToAction("View", "Flat", new { pId = pModel.FlatInfo.Id });
                }
                pModel.ActionResultStatus = new ActionResultStatusViewModel("Error! Reason: " + string.Format(result.Reason, "Flat"), ActionStatus.Error);
            }
            catch (Exception ex)
            {
                pModel.ActionResultStatus = new ActionResultStatusViewModel("Error occured while creating Flat. Exception: " + ex.Message, ActionStatus.Error);
            }
            pModel.FlatTypes = await GetFlatTypes();
            return View(pModel);
        }

        private async Task<List<SelectListItem>> GetFlatTypes()
        {
            var response = await new ApiConnector<GeneralReturnInfo<GeneralInfo[]>>().SecureGetAsync("Common", "GetFlatTypes", LoggedInUser);
            var ddlItems = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "0",
                    Text = "-- Select --"
                }
            };
            ddlItems.AddRange(response.Info.Select((pX => new SelectListItem
            {
                Text = pX.Name,
                Value = Convert.ToString(pX.Id)
            })));

            return ddlItems;
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