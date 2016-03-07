using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using ThanalSoft.SmartComplex.Api.UnitOfWork;
using ThanalSoft.SmartComplex.Common;
using ThanalSoft.SmartComplex.Common.Exceptions;
using ThanalSoft.SmartComplex.Common.Models.Complex;
using ThanalSoft.SmartComplex.Entities.Complex;

namespace ThanalSoft.SmartComplex.Api.Controllers
{
    [RoutePrefix("api/Flat")]
    public class FlatController : BaseSecureController
    {
        public FlatController(IUnitOfWork pUnitOfWork) : base(pUnitOfWork)
        {
        }

        #region Get Methods

        [HttpGet]
        public async Task<GeneralReturnInfo<FlatInfo>> Get(int id)
        {
            var result = new GeneralReturnInfo<FlatInfo>();
            try
            {
                result.Info = await UnitOfWork.Flats.GetFlat(id);
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        [HttpGet]
        public async Task<GeneralReturnInfo<FlatInfo[]>> GetAll(int id)
        {
            var result = new GeneralReturnInfo<FlatInfo[]>();
            try
            {
                result.Info = await UnitOfWork.Flats.GetAllApartmentFlats(id);
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        [HttpGet]
        public async Task<GeneralReturnInfo<FlatInfo[]>> GetUserFlats(Int64 id)
        {
            var result = new GeneralReturnInfo<FlatInfo[]>();
            try
            {
                result.Info = await UnitOfWork.Flats.GetUserFlats(id);
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        #endregion

        #region Post Methods

        [HttpPost]
        public async Task<GeneralReturnInfo> Create(FlatInfo pApartmentFlatInfo)
        {
            var result = new GeneralReturnInfo();
            try
            {
                UnitOfWork.Flats.Add(AddFlat(pApartmentFlatInfo));
                await UnitOfWork.WorkCompleteAsync();
            }
            catch (ItemAlreadyExistsException ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        [HttpPost]
        public async Task<GeneralReturnInfo> Update(FlatInfo pApartmentFlatInfo)
        {
            var result = new GeneralReturnInfo();
            try
            {
                var original = await UnitOfWork.Flats.FindAsync(pApartmentFlatInfo.Id);

                if (original == null)
                    throw new KeyNotFoundException(pApartmentFlatInfo.Id.ToString());

                if (await UnitOfWork.Flats.AnyAsync(pX => pX.Name.Equals(pApartmentFlatInfo.Name, StringComparison.OrdinalIgnoreCase)
                                    && pX.ApartmentId.Equals(pApartmentFlatInfo.ApartmentId) && pX.Id != original.Id))
                    throw new ItemAlreadyExistsException(pApartmentFlatInfo.Name, "Flat");

                original.Block = pApartmentFlatInfo.Block;
                original.ExtensionNumber = pApartmentFlatInfo.ExtensionNumber;
                original.FlatTypeId = pApartmentFlatInfo.FlatTypeId <= 0 ? null : pApartmentFlatInfo.FlatTypeId;
                original.Floor = pApartmentFlatInfo.Floor ?? 0;
                original.Name = pApartmentFlatInfo.Name;
                original.Phase = pApartmentFlatInfo.Phase;
                original.SquareFeet = pApartmentFlatInfo.SquareFeet;
                original.LastUpdated = DateTime.Now;
                original.LastUpdatedBy = LoggedInUser;

                await UnitOfWork.WorkCompleteAsync();
            }
            catch (ItemAlreadyExistsException ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            catch (Exception ex)
            {
                result.Result = ApiResponseResult.Error;
                result.Reason = ex.Message;
            }
            return result;
        }

        #endregion

        #region Private Methods

        [NonAction]
        private Flat AddFlat(FlatInfo pApartmentFlatInfo)
        {
            return new Flat
            {
                ApartmentId = pApartmentFlatInfo.ApartmentId,
                Block = pApartmentFlatInfo.Block,
                ExtensionNumber = pApartmentFlatInfo.ExtensionNumber,
                Floor = pApartmentFlatInfo.Floor ?? 0,
                Name = pApartmentFlatInfo.Name,
                Phase = pApartmentFlatInfo.Phase,
                SquareFeet = pApartmentFlatInfo.SquareFeet,
                LastUpdated = DateTime.Now,
                LastUpdatedBy = LoggedInUser,
                FlatTypeId = pApartmentFlatInfo.FlatTypeId <= 0 ? null : pApartmentFlatInfo.FlatTypeId
            };
        }

        #endregion
        
    }
}