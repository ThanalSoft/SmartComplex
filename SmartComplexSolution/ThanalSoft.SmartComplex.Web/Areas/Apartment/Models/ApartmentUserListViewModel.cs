﻿using ThanalSoft.SmartComplex.Common.Models.Complex;
using ThanalSoft.SmartComplex.Web.Models;

namespace ThanalSoft.SmartComplex.Web.Areas.Apartment.Models
{
    public class ApartmentUserListViewModel : BaseViewModel
    {
        public ApartmentUserInfo[] Users { get; set; }

        public int ApartmentId { get; set; }
    }
}