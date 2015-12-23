﻿using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using ThanalSoft.SmartComplex.DataObjects.Security;

namespace ThanalSoft.SmartComplex.Api.Security
{
    public class SecureSignInManager : SignInManager<User, Int64>
    {
        public SecureSignInManager(UserManager<User, Int64> pUserManager, 
            IAuthenticationManager pAuthenticationManager) : base(pUserManager, pAuthenticationManager)
        {
        }

        public override async Task<ClaimsIdentity> CreateUserIdentityAsync(User pUser)
        {
            return await GenerateUserIdentityAsync(pUser, (SecureUserManager)UserManager);
        }

        private async Task<ClaimsIdentity> GenerateUserIdentityAsync(User pUser, SecureUserManager pUserManager)
        {
            var userIdentity = await pUser.GenerateUserIdentityAsync(pUserManager, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public static SecureSignInManager Create(IdentityFactoryOptions<SecureSignInManager> pOptions, IOwinContext pContext)
        {
            return new SecureSignInManager(pContext.GetUserManager<SecureUserManager>(), pContext.Authentication);
        }
    }
}