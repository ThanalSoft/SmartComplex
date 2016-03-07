using System;
using System.Threading.Tasks;
using ThanalSoft.SmartComplex.Business.Complex;
using ThanalSoft.SmartComplex.Business.Repositories;
using ThanalSoft.SmartComplex.Business.User;
using ThanalSoft.SmartComplex.DataAccess;
using ThanalSoft.SmartComplex.Entities.Common;
using ThanalSoft.SmartComplex.Entities.Complex;
using ThanalSoft.SmartComplex.Entities.UserUtilities;

namespace ThanalSoft.SmartComplex.Api.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private SmartComplexDataObjectContext _context;
        private SmartComplexDataObjectContext Context => _context ?? (_context = new SmartComplexDataObjectContext());

        public UnitOfWork()
        {
            Users = new UserRepository(Context);
            Apartments = new ApartmentRepository(Context);
            Flats = new FlatRepository(Context);

            FlatTypes = new GenericRepository<FlatType>(Context);
            Countries = new GenericRepository<Country>(Context);
            States = new GenericRepository<State>(Context);
            BloodGroups = new GenericRepository<BloodGroup>(Context);
            Notifications = new GenericRepository<Notification>(Context);
        }

        #region Configure Repositories

        public IApartmentRepository Apartments { get; private set; }

        public IFlatRepository Flats { get; private set; }

        public IUserRepository Users { get; private set; }

        #region Generic Repositories

        public IGenericRepository<FlatType> FlatTypes { get; private set; }

        public IGenericRepository<BloodGroup> BloodGroups { get; private set; }

        public IGenericRepository<Notification> Notifications { get; private set; }

        public IGenericRepository<State> States { get; private set; }

        public IGenericRepository<Country> Countries { get; private set; }

        #endregion

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