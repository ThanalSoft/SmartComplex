﻿using System;
using Microsoft.AspNet.Identity.EntityFramework;
using ThanalSoft.SmartComplex.DataAccess;
using ThanalSoft.SmartComplex.DataObjects.Security;

namespace ThanalSoft.SmartComplex.Api.Security
{
    public class SecureUserStore : UserStore<User, Role, Int64, UserLogin, UserRole, UserClaim>
    {
        public SecureUserStore(SmartComplexDataObjectContext pContext) : base(pContext)
        {
        }
    }
}