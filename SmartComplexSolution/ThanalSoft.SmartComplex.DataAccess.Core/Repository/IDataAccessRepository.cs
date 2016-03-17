using System.Collections.Generic;
using System.Linq;
using ThanalSoft.SmartComplex.DataAccess.Core.DataContext;

namespace ThanalSoft.SmartComplex.DataAccess.Core.Repository
{
    public interface IDataAccessRepository<TEntity> where TEntity : class, IDataObjectState
    {
        TEntity Find(object pKey);
        TEntity Find(params object[] pKeyValues);
        IQueryable<TEntity> SelectQuery(string pQuery, params object[] pArameters);
        void Insert(TEntity pEntity);
        void InsertRange(IEnumerable<TEntity> pEntities);
        void AddRange(IEnumerable<TEntity> pEntities);
        void Update(TEntity pEntity);
        void Delete(object pId);
        void Delete(TEntity pEntity);
    }
}
