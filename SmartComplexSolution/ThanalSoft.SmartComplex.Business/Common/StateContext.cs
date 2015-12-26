using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ThanalSoft.SmartComplex.Common.Models.Common;
using ThanalSoft.SmartComplex.DataAccess;

namespace ThanalSoft.SmartComplex.Business.Common
{
    public class StateContext : BaseBusiness<StateContext>
    {
        public async Task<StateInfo[]> GetStatesAsync()
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                var data = await context.States.ToArrayAsync();
                return data.Select(pX => new StateInfo
                {
                    Name = pX.Name,
                    Id = pX.Id
                }).ToArray();
            }
        }
    }
}