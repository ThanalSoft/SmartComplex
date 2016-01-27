using System.Collections.Generic;
using System.Web.Mvc;
using ThanalSoft.SmartComplex.Common.Models.Complex;
using ThanalSoft.SmartComplex.Web.Models;

namespace ThanalSoft.SmartComplex.Web.Areas.Apartment.Models
{
    public class ApartmentViewModel : BaseViewModel
    {
        public ApartmentInfo Apartment { get; set; }

        public List<SelectListItem> States { get; set; }

        public bool HideBack { get; set; }
    }
}