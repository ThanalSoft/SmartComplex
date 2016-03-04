using System;
using System.Threading.Tasks;
using System.Web.Http;
using ThanalSoft.SmartComplex.Api.UnitOfWork;
using ThanalSoft.SmartComplex.Business.Common;
using ThanalSoft.SmartComplex.Common;
using ThanalSoft.SmartComplex.Common.Models.Dashboard;

namespace ThanalSoft.SmartComplex.Api.Controllers
{
    [RoutePrefix("api/Dashboard")]
    public class DashboardController : BaseSecureController
    {
        public DashboardController(IUnitOfWork pUnitOfWork) : base(pUnitOfWork)
        {
        }

        [HttpGet]
        public async Task<GeneralReturnInfo<AdminDashboardInfo>> GetAdministratorDashboard()
        {
            var result = new GeneralReturnInfo<AdminDashboardInfo>();
            try
            {
                result.Info = await DashboardContext.Instance.GetAdminDashboard();
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }
    }
}