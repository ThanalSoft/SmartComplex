using System;
using System.Threading.Tasks;
using ThanalSoft.SmartComplex.Common.Models.Complex;
using ThanalSoft.SmartComplex.Entities.Complex;

namespace ThanalSoft.SmartComplex.Business.Repositories
{
    public interface IFlatRepository : IRepositoryService<Flat>
    {
        Task<FlatInfo> GetFlat(int pFlatId);
        Task<FlatInfo[]> GetAllApartmentFlats(int pApartmentId);
        Task<FlatInfo[]> GetUserFlats(Int64 pUserId);
    }
}