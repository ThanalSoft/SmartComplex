using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThanalSoft.SmartComplex.Common.Models.Complex;
using ThanalSoft.SmartComplex.Entities.Complex;

namespace ThanalSoft.SmartComplex.Business.Repositories
{
    public interface IApartmentRepository : IRepositoryService<Apartment>
    {
        Task<IEnumerable<ApartmentUserInfo>> GetAllApartmentUsersAsync(int pApartmentId);

        Task<ApartmentUserInfo> GetApartmentUserAsync(Int64 pUserId);

        Task<ApartmentInfo[]> GetUserApartmentsAsync(Int64 pUserId);
    }
}