using System.Linq;
using System.Security.Principal;

namespace ThanalSoft.SmartComplex.Web.Security
{
    public class SmartComplexPrincipal : ISmartComplexPrincipal
    {
        public SmartComplexPrincipal(string pEmail)
        {
            this.Identity = new GenericIdentity(pEmail);
        }

        public bool IsInRole(string pRole)
        {
            return Roles.Any(pX => pX.Equals(pRole));
        }

        public IIdentity Identity { get; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserIdentity { get; set; }
        public string[] Roles { get; set; }
    }
}