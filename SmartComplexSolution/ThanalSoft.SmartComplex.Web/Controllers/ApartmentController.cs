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
            var response = await new ApiConnector<GeneralReturnInfo<ApartmentInfo[]>>().SecureGetAsync("Apartment", "GetAll", LoggedInUser);
            return View(new ApartmentListViewModel
            {
                Apartments = response.Info.ToArray(),
                ActionResultStatus = (ActionResultStatusViewModel) TempData["Status"]
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

                TempData["Status"] = new ActionResultStatusViewModel("Apartment is created successfully!", ActionStatus.Success);
            }
            catch (Exception)
            {
                TempData["Status"] = new ActionResultStatusViewModel("Error occured while creating Apartment.", ActionStatus.Error);
            }
            
            return RedirectToAction("Index");
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