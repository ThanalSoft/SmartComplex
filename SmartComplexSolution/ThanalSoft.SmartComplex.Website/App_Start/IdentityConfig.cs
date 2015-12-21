using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using ThanalSoft.SmartComplex.DataAccess;
using ThanalSoft.SmartComplex.DataObjects.Security;
using ThanalSoft.SmartComplex.Website.Models;

namespace ThanalSoft.SmartComplex.Website
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class SmartComplexUserManager : UserManager<User, Int64>
    {
        public SmartComplexUserManager(IUserStore<User, Int64> pStore)
            : base(pStore)
        {
        }

        public static SmartComplexUserManager Create(IdentityFactoryOptions<SmartComplexUserManager> pOptions, IOwinContext pContext) 
        {
            var manager = new SmartComplexUserManager(new UserStore<User, Role, Int64, UserLogin, UserRole, UserClaim>(pContext.Get<SmartComplexDataObjectContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<User, Int64>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<User, Int64>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<User, Int64>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = pOptions.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<User, Int64>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class SmartComplexSignInManager : SignInManager<User, Int64>
    {
        public SmartComplexSignInManager(SmartComplexUserManager pUserManager, IAuthenticationManager pAuthenticationManager)
            : base(pUserManager, pAuthenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(User pUser)
        {
            return pUser.GenerateUserIdentityAsync((SmartComplexUserManager)UserManager);
        }

        public static SmartComplexSignInManager Create(IdentityFactoryOptions<SmartComplexSignInManager> pOptions, IOwinContext pContext)
        {
            return new SmartComplexSignInManager(pContext.GetUserManager<SmartComplexUserManager>(), pContext.Authentication);
        }
    }
}
