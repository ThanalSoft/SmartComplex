using System;
using System.Collections.Generic;
using System.Data.OleDb;
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
    [Authorize(Roles = "Administrator")]
    public class ManageController : BaseSecuredController
    {

        #region Get Methods

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var response = await GetAllApartmentsAsync();

            return View(new ApartmentListViewModel
            {
                Apartments = response.Info,
                ActionResultStatus = ViewResultStatus,
                IsAsyncRequest = IsAjaxRequest
            });
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            return View(new ApartmentViewModel
            {
                IsAsyncRequest = IsAjaxRequest,
                ActionResultStatus = ViewResultStatus,
                States = await GetStates()
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

        [HttpGet]
        public async Task<ActionResult> UploadFlats(int pApartmentId)
        {
            var response = await GetApartment(pApartmentId);
            return View(new ApartmentViewModel
            {
                IsAsyncRequest = IsAjaxRequest,
                ActionResultStatus = ViewResultStatus,
                Apartment = response.Info
            });
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
        public async Task<ActionResult> GetApartmentUser(int pUserId)
        {
            var response = await GetApartmentUserData(pUserId);
            return View(new ApartmentUserViewModel
            {
                User = response.Info,
                ActionResultStatus = ViewResultStatus,
                IsAsyncRequest = IsAjaxRequest
            });
        }

        #endregion

        #region Post Methods

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ApartmentViewModel pModel)
        {
            pModel.IsAsyncRequest = IsAjaxRequest;
            if (!ModelState.IsValid)
            {
                pModel.States = await GetStates();
                return View(pModel);
            }
            try
            {
                var result = await CreateApartmentAsync(pModel.Apartment);
                if (result.Result == ApiResponseResult.Success)
                {
                    ViewResultStatus = new ActionResultStatusViewModel($"Apartment '{pModel.Apartment.Name}' created successfully!", ActionStatus.Success);
                    return RedirectToAction("GetAll");
                }
                pModel.ActionResultStatus = new ActionResultStatusViewModel("Error! Reason: " + result.Reason, ActionStatus.Error);
            }
            catch (Exception ex)
            {
                pModel.ActionResultStatus = new ActionResultStatusViewModel("Error occured while creating Apartment. Exception: " + ex.Message, ActionStatus.Error);
            }
            pModel.States = await GetStates();
            return View(pModel);
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
                    return RedirectToAction("GetAll");
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadFlats(ApartmentViewModel pModel)
        {
            if (Request.Files["fileUpload"]?.ContentLength > 0)
            {
                var fileName = Request.Files["fileUpload"].FileName;
                var extension = System.IO.Path.GetExtension(Request.Files["fileUpload"].FileName);
                if (extension != ".xlsx" && extension != ".xls")
                {
                    TempData["Status"] = new ActionResultStatusViewModel("Invalid file. Please upload excel file.", ActionStatus.Error);
                    return RedirectToAction("UploadFlats", "Manage", new { pApartmentId = pModel.Apartment.Id });
                }
                try
                {
                    string path = $"{Server.MapPath("~/TestUploads/ExcelUploadFolder")}/{Guid.NewGuid()}-Apartment-{pModel.Apartment.Id}-{fileName}";
                   
                    Request.Files["fileUpload"].SaveAs(path);

                    //Create connection string to Excel work book
                    var excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=Excel 12.0;Persist Security Info=False";
                    //Create Connection to Excel work book
                    using (var excelConnection = new OleDbConnection(excelConnectionString))
                    {
                        //Create OleDbCommand to fetch data from Excel
                        var cmd = new OleDbCommand("SELECT [FlatName],[Floor],[Block],[Phase],[OwnerName],[OwnerEmail],[OwnerMobile] FROM [FlatListSheet$]", excelConnection);
                        excelConnection.Open();
                        var dReader = cmd.ExecuteReader();
                        var flatUploadDataInfoList = new List<FlatUploadInfo>();
                        if (dReader != null && dReader.HasRows)
                        {
                            while (dReader.Read())
                            {
                                if (string.IsNullOrEmpty(dReader["FlatName"]?.ToString())) break;

                                flatUploadDataInfoList.Add(new FlatUploadInfo
                                {
                                    Block = dReader["Block"]?.ToString(),
                                    Name = dReader["FlatName"]?.ToString(),
                                    Floor = Convert.ToInt32(dReader["Floor"].ToString()),
                                    Phase = dReader["Phase"]?.ToString(),
                                    ApartmentId = pModel.Apartment.Id,
                                    OwnerEmail = dReader["OwnerEmail"]?.ToString(),
                                    OwnerName = dReader["OwnerName"]?.ToString(),
                                    OwnerMobile = dReader["OwnerMobile"]?.ToString(),
                                });
                            }

                            UploadFlatsAsync(flatUploadDataInfoList);
                        }
                        else
                            ViewResultStatus = new ActionResultStatusViewModel("File not formed correctly. Contact Administrator!", ActionStatus.Error);

                        cmd.Dispose();
                        excelConnection.Close();
                    }
                }
                catch (Exception ex)
                {
                    ViewResultStatus = new ActionResultStatusViewModel("Error while uploading data. Reason: " + ex.Message, ActionStatus.Error);
                    return RedirectToAction("Get", "Manage", new { pApartmentId = pModel.Apartment.Id });
                }
            }
            ViewResultStatus = new ActionResultStatusViewModel("File is under processing. Once the file is processed completly you will be notified.", ActionStatus.Success);
            return RedirectToAction("Get", "Manage", new { pApartmentId = pModel.Apartment.Id });
        }

        #endregion

        #region Private Methods

        private async Task<GeneralReturnInfo<ApartmentInfo[]>> GetAllApartmentsAsync()
        {
            var response = await new ApiConnector<GeneralReturnInfo<ApartmentInfo[]>>().SecureGetAsync("Apartment", "GetAll");
            return response;
        }

        private async Task<GeneralReturnInfo<ApartmentInfo>> GetApartment(int pId)
        {
            return await new ApiConnector<GeneralReturnInfo<ApartmentInfo>>().SecureGetAsync("Apartment", "Get", pId.ToString());
        }

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

        private async Task<GeneralReturnInfo> CreateApartmentAsync(ApartmentInfo pApartmentInfo)
        {
            var result = await new ApiConnector<GeneralReturnInfo>().SecurePostAsync("Apartment", "Create", pApartmentInfo);
            return result;
        }

        private async Task<GeneralReturnInfo> UpdateApartmentAsync(ApartmentInfo pApartmentInfo)
        {
            var result = await new ApiConnector<GeneralReturnInfo>().SecurePostAsync("Apartment", "Update", pApartmentInfo);
            return result;
        }

        private async Task UploadFlatsAsync(List<FlatUploadInfo> pFlatUploadDataInfoList)
        {
            await new ApiConnector<GeneralReturnInfo<FlatUploadInfo[]>>().SecurePostAsync("Apartment", "UploadFlats", pFlatUploadDataInfoList);
        }

        private async Task<GeneralReturnInfo<ApartmentUserInfo[]>> GetApartmentUsers(int pApartmentId)
        {
            return await new ApiConnector<GeneralReturnInfo<ApartmentUserInfo[]>>().SecureGetAsync("Apartment", "GetApartmentUsers", pApartmentId.ToString());
        }

        private async Task<GeneralReturnInfo<ApartmentUserInfo>> GetApartmentUserData(int pUserId)
        {
            return await new ApiConnector<GeneralReturnInfo<ApartmentUserInfo>>().SecureGetAsync("Apartment", "GetApartmentUser", pUserId.ToString());
        }

        #endregion
    }
}