using ThanalSoft.SmartComplex.Common.Models.Complex;

namespace ThanalSoft.SmartComplex.Web.Models.Apartment
{
    public class FlatManagementViewModel : BaseViewModel
    {
        public int ApartmentId { get; set; }

        public ApartmentFlatInfo[] ApartmentFlatInfoList { get; set; }

    }
}