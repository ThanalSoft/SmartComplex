using System;
using Microsoft.AspNet.Identity.EntityFramework;
using ThanalSoft.SmartComplex.DataAccess;
using ThanalSoft.SmartComplex.Entities.Security;

namespace ThanalSoft.SmartComplex.Api.Security
{
    public class SecureRoleStore : RoleStore<Role, Int64, UserRole>
    {
        public SecureRoleStore(SmartComplexDataObjectContext pContext) : base(pContext)
        {

        }
    }
}