using System.Collections.Generic;
using System.Web.Mvc;
using ThanalSoft.SmartComplex.Common.Models.Complex;

namespace ThanalSoft.SmartComplex.Web.Models.Flat
{
    public class FlatViewModel : BaseViewModel
    {
        public FlatInfo FlatInfo { get; set; }

        public IEnumerable<SelectListItem> FlatTypes { get; set; }
    }
}