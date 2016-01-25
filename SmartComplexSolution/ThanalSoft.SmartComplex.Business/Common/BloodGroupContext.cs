using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ThanalSoft.SmartComplex.Common.Models.Common;
using ThanalSoft.SmartComplex.DataAccess;

namespace ThanalSoft.SmartComplex.Business.Common
{
    public class BloodGroupContext : BaseBusiness<BloodGroupContext>
    {
        public async Task<GeneralInfo[]> GetBloodGroupsAsync()
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                var data = await context.BloodGroups.ToArrayAsync();
                return data.Select(pX => new GeneralInfo
                {
                    Name = pX.Group,
                    Id = pX.Id
                }).ToArray();
            }
        }
    }
}