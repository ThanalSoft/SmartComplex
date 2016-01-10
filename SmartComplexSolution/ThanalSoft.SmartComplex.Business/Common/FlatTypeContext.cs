using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ThanalSoft.SmartComplex.Common.Models.Common;
using ThanalSoft.SmartComplex.DataAccess;

namespace ThanalSoft.SmartComplex.Business.Common
{
    public class FlatTypeContext : BaseBusiness<FlatTypeContext>
    {
        public async Task<GeneralInfo[]> GetFlatTypesAsync()
        {
            using (var context = new SmartComplexDataObjectContext())
            {
                var data = await context.FlatTypes.Where(pX => pX.IsActive).ToArrayAsync();
                return data.Select(pX => new GeneralInfo
                {
                    Name = pX.Name,
                    Id = pX.Id
                }).ToArray();
            }
        }
    }
}