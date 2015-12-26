using System;
using System.Threading.Tasks;
using System.Web.Http;
using ThanalSoft.SmartComplex.Business.Common;
using ThanalSoft.SmartComplex.Common;
using ThanalSoft.SmartComplex.Common.Models.Common;

namespace ThanalSoft.SmartComplex.Api.Controllers
{
    [RoutePrefix("api/Common")]
    public class CommonController : BaseSecureApiController
    {
        [HttpGet]
        public async Task<GeneralReturnInfo<StateInfo[]>> GetStates()
        {
            var result = new GeneralReturnInfo<StateInfo[]>();
            try
            {
                result.Info = await StateContext.Instance.GetStatesAsync();
            }
            catch (Exception ex)
            {
                result.Result = "Error";
                result.Reason = ex.Message;
            }
            return result;
        }
    }
}