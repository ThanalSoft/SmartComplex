using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ThanalSoft.SmartComplex.DataAccess;

namespace ThanalSoft.SmartComplex.Business
{
    public class RepositoryService<TEntity> : IRepositoryService<TEntity> where TEntity : class
    {
        protected SmartComplexDataObjectContext Context { get; private set; }

        public RepositoryService(SmartComplexDataObjectContext pContext)
        {
            Context = pContext;
        }

        public async Task<TEntity> FindAsync(object pId)
        {
            return await Context.Set<TEntity>().FindAsync(pId);
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> pPredicate)
        {
            return await Context.Set<TEntity>().Where(pPredicate).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> AllAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> AllAsync(Expression<Func<TEntity, bool>> pPredicate)
        {
            return await Context.Set<TEntity>().Where(pPredicate).ToListAsync();
        }

        public void Add(TEntity pEntity)
        {
            Context.Set<TEntity>().Add(pEntity);
        }

        public void AddRange(IEnumerable<TEntity> pEntities)
        {
            Context.Set<TEntity>().AddRange(pEntities);
        }

        public void Update(object pId, TEntity pEntity)
        {
            throw new NotImplementedException();
        }

        public void Remove(TEntity pEntity)
        {
            Context.Set<TEntity>().Remove(pEntity);
        }

        public void RemoveRange(IEnumerable<TEntity> pEntities)
        {
            Context.Set<TEntity>().RemoveRange(pEntities);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> pPredicate)
        {
            return await Context.Set<TEntity>().CountAsync(pPredicate);
        }

        public async Task<int> CountAsync()
        {
            return await Context.Set<TEntity>().CountAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> pPredicate)
        {
            return await Context.Set<TEntity>().AnyAsync(pPredicate);
        }

        public async Task<bool> AnyAsync()
        {
            return await Context.Set<TEntity>().AnyAsync();
        }
    }
}