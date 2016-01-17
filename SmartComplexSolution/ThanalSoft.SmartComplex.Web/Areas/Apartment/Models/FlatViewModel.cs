using System.Collections.Generic;
using System.Web.Mvc;
using ThanalSoft.SmartComplex.Common.Models.Complex;
using ThanalSoft.SmartComplex.Web.Models;

namespace ThanalSoft.SmartComplex.Web.Areas.Apartment.Models
{
    public class FlatViewModel : BaseViewModel
    {
        public FlatInfo Flat { get; set; }

        public IEnumerable<SelectListItem> FlatTypes { get; set; }
    }
}