using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ThanalSoft.SmartComplex.Business.Repositories;
using ThanalSoft.SmartComplex.Common.Models.Complex;
using ThanalSoft.SmartComplex.DataAccess;
using ThanalSoft.SmartComplex.Entities.Complex;

namespace ThanalSoft.SmartComplex.Business.Complex
{
    public class FlatRepository : RepositoryService<Flat>, IFlatRepository
    {
        public FlatRepository(SmartComplexDataObjectContext pContext) : base(pContext)
        {

        }

        public async Task<FlatInfo> GetFlat(int pFlatId)
        {
            var flatInfo = await Context.Flats
                .Include(pX => pX.Apartment)
                .Include(pX => pX.FlatType)
                .Where(pX => pX.Id.Equals(pFlatId)).FirstAsync();

            return MapToFlatInfo(flatInfo);
        }

        public async Task<FlatInfo[]> GetAllApartmentFlats(int pApartmentId)
        {
            var flats = await Context.Flats
                    .Include(pX => pX.Apartment)
                    .Include(pX => pX.FlatType)
                    .Where(pX => pX.ApartmentId.Equals(pApartmentId)).ToListAsync();

            return flats.Select(MapToFlatInfo).ToArray();
        }

        public async Task<FlatInfo[]> GetUserFlats(Int64 pUserId)
        {
            var flats = await Context.Flats
                    .Include(pX => pX.Apartment)
                    .Include(pX => pX.FlatType)
                    .Where(pX => pX.MemberFlats.Any(pZ => pZ.UserId.Equals(pUserId))).ToListAsync();

            return flats.Select(MapToFlatInfo).ToArray();
        }

        private FlatInfo MapToFlatInfo(Flat pFlat)
        {
            var info = new FlatInfo
            {
                Name = pFlat.Name,
                ApartmentId = pFlat.ApartmentId,
                Phase = pFlat.Phase,
                Floor = pFlat.Floor,
                Block = pFlat.Block,
                ExtensionNumber = pFlat.ExtensionNumber,
                SquareFeet = pFlat.SquareFeet,
                Id = pFlat.Id,
                ApartmentName = pFlat.Apartment?.Name,
                FlatType = pFlat.FlatType?.Name,
                FlatTypeId = pFlat.FlatTypeId
            };
            return info;
        }
    }
}