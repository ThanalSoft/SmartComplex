using System.Collections.Generic;
using System.Web.Mvc;
using ThanalSoft.SmartComplex.Common.Models.Complex;

namespace ThanalSoft.SmartComplex.Web.Models
{
    public class ApartmentViewModel : BaseViewModel
    {
        public ApartmentInfo ApartmentInfo { get; set; }

        public List<SelectListItem> States { get; set; }
    }
}