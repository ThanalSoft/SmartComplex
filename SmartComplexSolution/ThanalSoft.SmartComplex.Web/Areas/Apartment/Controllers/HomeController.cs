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
            if(response.Info.Length > 1)
                return View(new ApartmentListViewModel
                {
                    Apartments = response.Info,
                    IsAsyncRequest = IsAjaxRequest,
                });

            return View("Get", new ApartmentViewModel
            {
                IsAsyncRequest = IsAjaxRequest,
                Apartment = response.Info[0],
                ActionResultStatus = ViewResultStatus
            });
        }

        [HttpGet]
        public async Task<ActionResult> Back()
        {
            var response = await GetUserApartments();
            if (response.Info.Length > 1)
                return View("Index", new ApartmentListViewModel
                {
                    Apartments = response.Info,
                    IsAsyncRequest = IsAjaxRequest,
                });

            return JavaScript("document.location.replace('" + Url.Action("Index", "Home", new {area = "Dashboard"}) + "');");
        }

        public async Task<ActionResult> Get(int pApartmentId, bool pShowBack = false)
        {
            var response = await GetApartment(pApartmentId);

            return View(new ApartmentViewModel
            {
                IsAsyncRequest = IsAjaxRequest,
                Apartment = response.Info,
                ActionResultStatus = ViewResultStatus,
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

        [HttpGet]
        public async Task<ActionResult> GetAllApartmentUsers(int pApartmentId)
        {
            var response = await GetApartmentUsers(pApartmentId);
            return View(new ApartmentUserListViewModel
            {
                Users = response.Info,
                ActionResultStatus = ViewResultStatus,
                IsAsyncRequest = IsAjaxRequest,
                ApartmentId = pApartmentId
            });
        }

        [HttpGet]
        public async Task<ActionResult> GetAllApartmentFlats(int pApartmentId)
        {
            var response = await GetApartmentFlats(pApartmentId);
            return View(new FlatListViewModel
            {
                IsAsyncRequest = IsAjaxRequest,
                Flats = response.Info,
                ApartmentId = pApartmentId,
                ActionResultStatus = ViewResultStatus
            });
        }

        [HttpGet]
        public async Task<ActionResult> CreateFlat(int pApartmentId)
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
        public async Task<ActionResult> GetFlat(int pFlatId)
        {
            var response = await GetFlatAsync(pFlatId);
            return View(new FlatViewModel
            {
                Flat = response.Info,
                ActionResultStatus = ViewResultStatus,
                IsAsyncRequest = IsAjaxRequest,
            });
        }

        #endregion

        #region Post Methods

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateFlat(FlatViewModel pModel)
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
                    return RedirectToAction("GetAllApartmentFlats", new { pApartmentId = pModel.Flat.ApartmentId });
                }
                pModel.ActionResultStatus = new ActionResultStatusViewModel("Error! Reason: " + result.Reason, ActionStatus.Error);
            }
            catch (Exception ex)
            {
                pModel.ActionResultStatus = new ActionResultStatusViewModel("Error occured while creating Flat. Exception: " + ex.Message, ActionStatus.Error);
            }
            pModel.FlatTypes = await GetFlatTypes();
            pModel.IsAsyncRequest = IsAjaxRequest;
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

        [NonAction]
        private async Task<GeneralReturnInfo<FlatInfo[]>> GetApartmentFlats(int pApartmentId)
        {
            return await new ApiConnector<GeneralReturnInfo<FlatInfo[]>>().SecureGetAsync("Flat", "GetAll", pApartmentId.ToString());
        }

        [NonAction]
        private async Task<GeneralReturnInfo<ApartmentUserInfo[]>> GetApartmentUsers(int pApartmentId)
        {
            return await new ApiConnector<GeneralReturnInfo<ApartmentUserInfo[]>>().SecureGetAsync("Apartment", "GetApartmentUsers", pApartmentId.ToString());
        }

        [NonAction]
        private async Task<GeneralReturnInfo<FlatInfo>> GetFlatAsync(int pId)
        {
            return await new ApiConnector<GeneralReturnInfo<FlatInfo>>().SecureGetAsync("Flat", "Get", pId.ToString());
        }

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
        private async Task<GeneralReturnInfo> CreateFlatAsync(FlatViewModel pModel)
        {
            return await new ApiConnector<GeneralReturnInfo>().SecurePostAsync("Flat", "Create", pModel.Flat);
        }

        #endregion
    }
}