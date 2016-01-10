using System.Collections.Generic;
using System.Web.Mvc;
using ThanalSoft.SmartComplex.Common.Models.Complex;

namespace ThanalSoft.SmartComplex.Web.Models.Apartment
{
    public class ApartmentViewModel : BaseViewModel
    {
        public int ApartmentId { get; set; }

        public ApartmentInfo ApartmentInfo { get; set; }

        public List<SelectListItem> States { get; set; }
    }
}