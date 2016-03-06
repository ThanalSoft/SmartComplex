using ThanalSoft.SmartComplex.DataAccess;

namespace ThanalSoft.SmartComplex.Business.Repositories
{
    public class GenericRepository<TEntity> : RepositoryService<TEntity>, IGenericRepository<TEntity> where TEntity : class
    {
        public GenericRepository(SmartComplexDataObjectContext pContext) : base(pContext)
        {
        }
    }
}