using System.Runtime.Serialization;
using System.Web.Http;

namespace ThanalSoft.SmartComplex.Common
{
    [DataContract]
    public class GeneralReturnInfo<TInfo>
    {
        public GeneralReturnInfo()
        {
            Result = ApiResponseResult.Success;
        }

        [DataMember]
        public TInfo Info { get; set; }

        [DataMember]
        public ApiResponseResult Result { get; set; }

        [DataMember]
        public string Reason { get; set; }

        [DataMember]
        public IHttpActionResult HttpActionResult { get; set; }
    }

    [DataContract]
    public class GeneralReturnInfo : GeneralReturnInfo<NullInfo>
    {
    }

    [DataContract]
    public class NullInfo
    {

    }

    public enum ApiResponseResult
    {
        Success,
        Error
    }
}