using ThanalSoft.SmartComplex.Business.Repositories;
using ThanalSoft.SmartComplex.DataAccess;
using ThanalSoft.SmartComplex.Entities.Complex;

namespace ThanalSoft.SmartComplex.Business.Complex
{
    public class ApartmentRepository : RepositoryService<Apartment>, IApartmentRepository
    {
        public ApartmentRepository(SmartComplexDataObjectContext pContext) : base(pContext)
        {

        }
    }
}