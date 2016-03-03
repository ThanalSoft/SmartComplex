using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using ThanalSoft.SmartComplex.Entities.Security;

namespace ThanalSoft.SmartComplex.Api.Security
{
    public class SecureSignInManager : SignInManager<LoginUser, Int64>
    {
        public SecureSignInManager(UserManager<LoginUser, Int64> pUserManager, 
            IAuthenticationManager pAuthenticationManager) : base(pUserManager, pAuthenticationManager)
        {
        }

        public override async Task<ClaimsIdentity> CreateUserIdentityAsync(LoginUser pUser)
        {
            return await GenerateUserIdentityAsync(pUser, (SecureUserManager)UserManager);
        }

        private async Task<ClaimsIdentity> GenerateUserIdentityAsync(LoginUser pUser, SecureUserManager pUserManager)
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