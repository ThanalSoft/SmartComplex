namespace ThanalSoft.SmartComplex.Business.Repositories
{
    public interface IGenericRepository<TEntity> : IRepositoryService<TEntity> where TEntity : class
    {
    }
}