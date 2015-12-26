using ThanalSoft.SmartComplex.Common.Models.Complex;

namespace ThanalSoft.SmartComplex.Web.Models
{
    public class ApartmentListViewModel
    {
        public ApartmentInfo[] Apartments { get; set; }
        public ActionResultStatusViewModel ActionResultStatus { get; set; }
    }
}