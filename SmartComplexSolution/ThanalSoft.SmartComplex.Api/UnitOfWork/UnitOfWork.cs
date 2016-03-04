using System;
using System.Threading.Tasks;
using ThanalSoft.SmartComplex.Business.Complex;
using ThanalSoft.SmartComplex.Business.Repositories;
using ThanalSoft.SmartComplex.DataAccess;

namespace ThanalSoft.SmartComplex.Api.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private SmartComplexDataObjectContext _context;
        private SmartComplexDataObjectContext Context => _context ?? (_context = new SmartComplexDataObjectContext());

        public UnitOfWork()
        {
            Apartments = new ApartmentRepository(Context);
        }

        #region Configure Repositories

        public IApartmentRepository Apartments { get; private set; }

        #endregion
        
        public async Task<int> WorkCompleteAsync()
        {
            return await Context.SaveChangesAsync();
        }

        private void Dispose(bool pDisposing)
        {
            if (pDisposing)
            {
                Context?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}