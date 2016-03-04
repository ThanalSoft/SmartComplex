using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ThanalSoft.SmartComplex.Business
{
    public interface IRepositoryService<TEntity> where TEntity : class
    {
        Task<TEntity> FindAsync(object pId);
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> pPredicate);

        Task<IEnumerable<TEntity>> AllAsync();
        Task<IEnumerable<TEntity>> AllAsync(Expression<Func<TEntity, bool>> pPredicate);

        void Add(TEntity pEntity);
        void AddRange(IEnumerable<TEntity> pEntities);

        void Update(object pId, TEntity pEntity);

        void Remove(TEntity pEntity);
        void RemoveRange(IEnumerable<TEntity> pEntities);
    }
}