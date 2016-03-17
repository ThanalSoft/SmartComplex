using System;
using System.Threading;
using System.Threading.Tasks;

namespace ThanalSoft.SmartComplex.DataAccess.Core.DataContext
{
    public interface IDataContext : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken pCancellationToken);
        Task<int> SaveChangesAsync();
        void SyncObjectState<TEntity>(TEntity pEntity) where TEntity : class, IDataObjectState;
        void SyncObjectsStatePostCommit();
    }
}