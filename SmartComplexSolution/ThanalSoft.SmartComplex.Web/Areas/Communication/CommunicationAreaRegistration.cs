﻿using System.Web.Mvc;

namespace ThanalSoft.SmartComplex.Web.Areas.Communication
{
    public class CommunicationAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Communication";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Communication_default",
                "Communication/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}