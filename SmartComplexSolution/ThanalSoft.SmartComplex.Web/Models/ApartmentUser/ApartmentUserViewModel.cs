using System.Collections.Generic;
using System.Web.Mvc;
using ThanalSoft.SmartComplex.Common.Models.Complex;

namespace ThanalSoft.SmartComplex.Web.Models.ApartmentUser
{
    public class ApartmentUserViewModel : BaseViewModel
    {
        public ApartmentUserInfo Users { get; set; }

        public IEnumerable<SelectListItem> BloodGroups { get; set; }
    }
}