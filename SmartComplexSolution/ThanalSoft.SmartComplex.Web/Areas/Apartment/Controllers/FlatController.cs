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
    [Authorize(Roles = "Administrator,ApartmentAdmin,Owner,Tenant")]
    public class FlatController : BaseSecuredController
    {

        #region Get Methods

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var userResponse = await GetUserFlats();
            if (userResponse.Info.Length > 1)
                return View(new FlatListViewModel
                {
                    IsAsyncRequest = IsAjaxRequest,
                    Flats = userResponse.Info,
                    ActionResultStatus = ViewResultStatus
                });

            return View("Get", new FlatViewModel
            {
                Flat = userResponse.Info[0],
                ActionResultStatus = ViewResultStatus,
                IsAsyncRequest = IsAjaxRequest
            });
        }

        [HttpGet]
        public async Task<ActionResult> Get(int pFlatId)
        {
            var response = await GetFlat(pFlatId);
            return View(new FlatViewModel
            {
                Flat = response.Info,
                ActionResultStatus = ViewResultStatus,
                IsAsyncRequest = IsAjaxRequest,
            });
        }
        
        [HttpGet]
        public async Task<ActionResult> Create(int pApartmentId)
        {
            return View(new FlatViewModel
            {
                Flat = new FlatInfo
                {
                    ApartmentId = pApartmentId
                },
                FlatTypes = await GetFlatTypes(),
                IsAsyncRequest = IsAjaxRequest,
                ActionResultStatus = ViewResultStatus
            });
        }

        [HttpGet]
        public async Task<ActionResult> Update(int pFlatId)
        {
            var response = await GetFlat(pFlatId);
            return View(new FlatViewModel
            {
                Flat = response.Info,
                FlatTypes = await GetFlatTypes(),
                IsAsyncRequest = IsAjaxRequest,
                ActionResultStatus = ViewResultStatus
            });
        }

        #endregion

        #region Post Methods

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(FlatViewModel pModel)
        {
            if (!ModelState.IsValid)
            {
                return View(new FlatViewModel
                {
                    Flat = new FlatInfo
                    {
                        ApartmentId = pModel.Flat.ApartmentId
                    },
                    FlatTypes = await GetFlatTypes(),
                    IsAsyncRequest = IsAjaxRequest,
                    ActionResultStatus = ViewResultStatus
                });
            }
            try
            {
                var result = await CreateFlatAsync(pModel);
                if (result.Result == ApiResponseResult.Success)
                {
                    ViewResultStatus = new ActionResultStatusViewModel("Flat created successfully!", ActionStatus.Success);
                    return RedirectToAction("Index", new { pApartmentId = pModel.Flat.ApartmentId });
                }
                pModel.ActionResultStatus = new ActionResultStatusViewModel("Error! Reason: " + result.Reason, ActionStatus.Error);
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
                var response = await GetFlat(pModel.Flat.Id);
                return View(new FlatViewModel
                {
                    Flat = response.Info,
                    FlatTypes = await GetFlatTypes(),
                    IsAsyncRequest = IsAjaxRequest,
                    ActionResultStatus = ViewResultStatus
                });
            }
            try
            {
                var result = await UpdateFlat(pModel);
                if (result.Result == ApiResponseResult.Success)
                {
                    ViewResultStatus = new ActionResultStatusViewModel("Flat updated successfully!", ActionStatus.Success);
                    return RedirectToAction("Get", "Flat", new { pFlatId = pModel.Flat.Id });
                }
                pModel.ActionResultStatus = new ActionResultStatusViewModel("Error! Reason: " + result.Reason, ActionStatus.Error);
            }
            catch (Exception ex)
            {
                pModel.ActionResultStatus = new ActionResultStatusViewModel("Error occured while creating Flat. Exception: " + ex.Message, ActionStatus.Error);
            }
            pModel.FlatTypes = await GetFlatTypes();
            return View(pModel);
        }
        
        #endregion

        #region Private Methods

        [NonAction]
        private async Task<List<SelectListItem>> GetFlatTypes()
        {
            var response = await new ApiConnector<GeneralReturnInfo<GeneralInfo[]>>().SecureGetAsync("Common", "GetFlatTypes");
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
        [NonAction]
        private async Task<GeneralReturnInfo<FlatInfo>> GetFlat(int pId)
        {
            return await new ApiConnector<GeneralReturnInfo<FlatInfo>>().SecureGetAsync("Flat", "Get", pId.ToString());
        }
        [NonAction]
        private async Task<GeneralReturnInfo> CreateFlatAsync(FlatViewModel pModel)
        {
            return await new ApiConnector<GeneralReturnInfo>().SecurePostAsync("Flat", "Create", pModel.Flat);
        }
        [NonAction]
        private async Task<GeneralReturnInfo> UpdateFlat(FlatViewModel pModel)
        {
            return await new ApiConnector<GeneralReturnInfo>().SecurePostAsync("Flat", "Update", pModel.Flat);
        }
        [NonAction]
        private async Task<GeneralReturnInfo<FlatInfo[]>> GetUserFlats()
        {
            var response = await new ApiConnector<GeneralReturnInfo<FlatInfo[]>>().SecureGetAsync("Flat", "GetUserFlats", User.UserId.ToString());
            return response;
        }

        #endregion

    }
}