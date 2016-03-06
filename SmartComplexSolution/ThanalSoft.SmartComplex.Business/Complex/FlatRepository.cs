using ThanalSoft.SmartComplex.Business.Repositories;
using ThanalSoft.SmartComplex.DataAccess;
using ThanalSoft.SmartComplex.Entities.Complex;

namespace ThanalSoft.SmartComplex.Business.Complex
{
    public class FlatRepository : RepositoryService<Flat>, IFlatRepository
    {
        public FlatRepository(SmartComplexDataObjectContext pContext) : base(pContext)
        {
        }
    }
}