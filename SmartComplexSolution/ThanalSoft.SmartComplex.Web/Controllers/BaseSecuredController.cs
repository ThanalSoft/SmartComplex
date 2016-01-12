using System.Web.Mvc;
using ThanalSoft.SmartComplex.Web.Security;

namespace ThanalSoft.SmartComplex.Web.Controllers
{
    [Authorize(Roles = "Administrator,Owner,Tenant,MaintenanceAdmin,ApartmentAdmin")]
    public class BaseSecuredController : Controller
    {
        protected virtual new SmartComplexPrincipal User => HttpContext.User as SmartComplexPrincipal;
    }
}