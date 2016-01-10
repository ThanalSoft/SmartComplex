using System.Data.Entity;
using System.Threading.Tasks;
using ThanalSoft.SmartComplex.Common.Models.Dashboard;
using ThanalSoft.SmartComplex.DataAccess;

namespace ThanalSoft.SmartComplex.Business.Common
{
    public class DashboardContext : BaseBusiness<DashboardContext>
    {
        public async Task<AdminDashboardInfo> GetAdminDashboard()
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                var apartmentCount = await context.Apartments.CountAsync();
                var apartmentDeletedCount = await context.Apartments.CountAsync(pX => pX.IsDeleted);
                var users = await context.Users.CountAsync();
                var usersActivated = await context.Users.CountAsync(pX => !pX.IsActivated);
                var flatCount = await context.Flats.CountAsync();

                return new AdminDashboardInfo
                {
                    TotalApartments = apartmentCount,
                    TotalDeletedApartments = apartmentDeletedCount,
                    TotalFlats = flatCount,
                    TotalActiveUsers = users,
                    TotalInactiveUsers = usersActivated
                };
            }
        }
    }
}