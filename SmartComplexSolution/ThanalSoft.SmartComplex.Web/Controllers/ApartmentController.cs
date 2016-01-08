using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ThanalSoft.SmartComplex.Common;
using ThanalSoft.SmartComplex.Common.Models.Common;
using ThanalSoft.SmartComplex.Common.Models.Complex;
using ThanalSoft.SmartComplex.Web.Common;
using ThanalSoft.SmartComplex.Web.Models.Apartment;
using ThanalSoft.SmartComplex.Web.Models.Common;

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
        public async Task<ActionResult> Create()
        {
            return View(new ApartmentViewModel
            {
                States = await GetStatesAsync()
            });
        }

        [HttpGet]
        public async Task<ActionResult> Update(int pId)
        {
            var response = await GetApartment(pId);
            return View(new ApartmentViewModel
            {
                States = await GetStatesAsync(),
                ApartmentInfo = response.Info
            });
        }

        [HttpGet]
        public async Task<ActionResult> View(int pId)
        {
            var response = await GetApartment(pId);
            return View(new ApartmentViewModel
            {
                ApartmentInfo = response.Info
            });
        }

        [HttpGet]
        public PartialViewResult UploadFlats(int pId)
        {
            return PartialView("_UploadFlats", new FlatManagementViewModel { ApartmentId = pId });
        }

        [HttpGet]
        public async Task<PartialViewResult> GetAllList()
        {
            var response = await GetApartmentInfoList();
            return PartialView("_ApartmentList", new ApartmentListViewModel
            {
                Apartments = response.Info.ToArray(),
                ActionResultStatus = (ActionResultStatusViewModel)TempData["Status"]
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
                    return RedirectToAction("GetAllList");
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
                    return RedirectToAction("GetAllList");
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<string> DeleteUndelete(int pId)
        {
            var response = await new ApiConnector<GeneralReturnInfo<ApartmentInfo>>().SecurePostAsync("Apartment", "DeleteUndelete", LoggedInUser, pId);
            if (response.Result == ApiResponseResult.Success)
                return ApiResponseResult.Success.ToString();

            return ApiResponseResult.Error.ToString();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<string> Lock(ApartmentViewModel pModel)
        {
            var response = await new ApiConnector<GeneralReturnInfo<ApartmentInfo>>().SecurePostAsync("Apartment", "LockUnlock", LoggedInUser, new ApartmentInfo { Id = pModel.ApartmentInfo.Id, LockReason = pModel.ApartmentInfo.LockReason });
            if (response.Result == ApiResponseResult.Success)
                return ApiResponseResult.Success.ToString();

            return ApiResponseResult.Error.ToString();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<string> Unlock(int pId)
        {
            var response = await new ApiConnector<GeneralReturnInfo<ApartmentInfo>>().SecurePostAsync("Apartment", "LockUnlock", LoggedInUser, new ApartmentInfo { Id = pId });
            if (response.Result == ApiResponseResult.Success)
                return ApiResponseResult.Success.ToString();

            return ApiResponseResult.Error.ToString();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadFlats(FlatManagementViewModel pModel)
        {
            if (Request.Files["fileUpload"]?.ContentLength > 0)
            {
                string fileName = Request.Files["fileUpload"].FileName;
                string extension = System.IO.Path.GetExtension(Request.Files["fileUpload"].FileName);
                if (extension != ".xlsx" && extension != ".xls")
                {
                    TempData["Status"] = new ActionResultStatusViewModel("Invalid file. Please upload excel file.", ActionStatus.Error);
                    return RedirectToAction("Index", "Apartment", new { pApartmentId = pModel.ApartmentId });
                }
                try
                {
                    string path = $"{Server.MapPath("~/TestUploads/ExcelUploadFolder")}/{Guid.NewGuid()}-Apartment-{pModel.ApartmentId}-{fileName}";
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);

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
                                    ApartmentId = pModel.ApartmentId,
                                    OwnerEmail = dReader["OwnerEmail"]?.ToString(),
                                    OwnerName = dReader["OwnerName"]?.ToString(),
                                    OwnerMobile = dReader["OwnerMobile"]?.ToString(),
                                });
                            }

                            var response = await new ApiConnector<GeneralReturnInfo<FlatUploadInfo[]>>().SecurePostAsync("Apartment", "UploadFlats", LoggedInUser, flatUploadDataInfoList);
                            pModel.ActionResultStatus = response.Result == ApiResponseResult.Success
                                ? await GetSuccessModel(pModel)
                                : new ActionResultStatusViewModel("File upload error! Reason: " + response.Reason, ActionStatus.Success);
                        }
                        else
                            pModel.ActionResultStatus = new ActionResultStatusViewModel("File not formed correctly. Contact Administrator!", ActionStatus.Error);

                        cmd.Dispose();
                        excelConnection.Close();
                    }
                }
                catch (Exception ex)
                {
                    TempData["Status"] = new ActionResultStatusViewModel("Error while uploading data. Reason: " + ex.Message, ActionStatus.Error);
                    return RedirectToAction("Index", "Apartment", new { pApartmentId = pModel.ApartmentId });
                }
            }
            TempData["Status"] = pModel.ActionResultStatus;
            return RedirectToAction("Index", "Apartment", new { pApartmentId = pModel.ApartmentId });
        }

        private async Task<ActionResultStatusViewModel> GetSuccessModel(FlatManagementViewModel pModel)
        {
            pModel.Apartment = (await GetApartment(pModel.ApartmentId)).Info;
            return new ActionResultStatusViewModel("File uploaded successfully. Flats are added to the Apartment.", ActionStatus.Success);
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

        private async Task<GeneralReturnInfo<ApartmentInfo>> GetApartment(int pId)
        {
            return await new ApiConnector<GeneralReturnInfo<ApartmentInfo>>().SecureGetAsync("Apartment", "Get", LoggedInUser, pId.ToString());
        }


    }
}