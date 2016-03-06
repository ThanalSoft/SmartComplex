using System;
using System.Threading.Tasks;
using ThanalSoft.SmartComplex.Business.Repositories;
using ThanalSoft.SmartComplex.Entities.Common;
using ThanalSoft.SmartComplex.Entities.UserUtilities;

namespace ThanalSoft.SmartComplex.Api.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IApartmentRepository Apartments { get; }

        IFlatRepository Flats { get; }

        IUserRepository Users { get; }

        IGenericRepository<BloodGroup> BloodGroups { get; }

        IGenericRepository<Notification> Notifications { get; }

        Task<int> WorkCompleteAsync();
    }
}