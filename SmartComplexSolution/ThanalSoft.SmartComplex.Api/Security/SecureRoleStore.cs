using System;
using Microsoft.AspNet.Identity.EntityFramework;
using ThanalSoft.SmartComplex.DataAccess;
using ThanalSoft.SmartComplex.DataObjects.Security;

namespace ThanalSoft.SmartComplex.Api.Security
{
    public class SecureRoleStore : RoleStore<Role, Int64, UserRole>
    {
        public SecureRoleStore(SmartComplexDataObjectContext pContext) : base(pContext)
        {

        }
    }
}