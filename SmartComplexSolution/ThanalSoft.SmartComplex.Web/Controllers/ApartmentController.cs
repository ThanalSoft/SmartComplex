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
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<PartialViewResult> IndexList()
        {
            var response = await new ApiConnector<GeneralReturnInfo<ApartmentInfo[]>>().SecureGetAsync("Apartment", "GetAll", LoggedInUser);

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
                await new ApiConnector<GeneralReturnInfo>().SecurePostAsync("Apartment", "Create", LoggedInUser, pModel.ApartmentInfo);

                TempData["Status"] = new ActionResultStatusViewModel("Apartment created successfully!", ActionStatus.Success);
            }
            catch (Exception)
            {
                TempData["Status"] = new ActionResultStatusViewModel("Error occured while creating Apartment.", ActionStatus.Error);
            }
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(int id)
        {
            var response = await new ApiConnector<GeneralReturnInfo<ApartmentInfo>>().SecureGetAsync("Apartment", "Get", LoggedInUser, id.ToString());
            return View(new ApartmentViewModel
            {
                States = await GetStatesAsync(),
                 ApartmentInfo = response.Info
            });
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> View(int id)
        {
            var response = await new ApiConnector<GeneralReturnInfo<ApartmentInfo>>().SecureGetAsync("Apartment", "Get", LoggedInUser, id.ToString());
            return View(new ApartmentViewModel
            {
                States = await GetStatesAsync(),
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
    }
}