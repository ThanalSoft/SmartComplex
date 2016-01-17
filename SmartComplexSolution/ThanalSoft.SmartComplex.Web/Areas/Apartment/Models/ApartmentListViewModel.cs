using ThanalSoft.SmartComplex.Common.Models.Complex;
using ThanalSoft.SmartComplex.Web.Models;

namespace ThanalSoft.SmartComplex.Web.Areas.Apartment.Models
{
    public class ApartmentListViewModel : BaseViewModel
    {
        public ApartmentInfo[] Apartments { get; set; }
    }
}