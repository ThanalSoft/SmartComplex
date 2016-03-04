using System;
using System.Threading.Tasks;
using ThanalSoft.SmartComplex.Business.Repositories;

namespace ThanalSoft.SmartComplex.Api.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IApartmentRepository Apartments { get; }

        Task<int> WorkCompleteAsync();
    }
}