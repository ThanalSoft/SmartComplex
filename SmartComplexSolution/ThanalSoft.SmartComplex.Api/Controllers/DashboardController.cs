using System;
using System.Threading.Tasks;
using System.Web.Http;
using ThanalSoft.SmartComplex.Api.UnitOfWork;
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
                var apartmentCount = await UnitOfWork.Apartments.CountAsync();
                var apartmentDeletedCount = await UnitOfWork.Apartments.CountAsync(pX => pX.IsDeleted);
                var users = await UnitOfWork.Users.CountAsync();
                var usersActivated = await UnitOfWork.Users.CountAsync(pX => !pX.IsActivated);
                var flatCount = await UnitOfWork.Flats.CountAsync();

                result.Info = new AdminDashboardInfo
                {
                    TotalApartments = apartmentCount,
                    TotalDeletedApartments = apartmentDeletedCount,
                    TotalFlats = flatCount,
                    TotalActiveUsers = users,
                    TotalInactiveUsers = usersActivated
                };
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