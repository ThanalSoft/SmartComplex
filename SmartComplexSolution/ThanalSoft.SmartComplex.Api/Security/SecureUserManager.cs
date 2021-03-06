﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using ThanalSoft.SmartComplex.Api.Services;
using ThanalSoft.SmartComplex.DataAccess;
using ThanalSoft.SmartComplex.Entities.Security;

namespace ThanalSoft.SmartComplex.Api.Security
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage pMessage)
        {
            return new SentMailService().SendEmailAsync(pMessage.Destination, pMessage.Subject, pMessage.Body);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage pMessage)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    public class SecureUserManager : UserManager<LoginUser, Int64>
    {
        public SecureUserManager() : base(new SecureUserStore(new SmartComplexDataObjectContext()))
        {

            UserValidator = new UserValidator<LoginUser, Int64>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
        }

        public static SecureUserManager Create(IdentityFactoryOptions<SecureUserManager> pOptions, IOwinContext pContext)
        {
            var manager = new SecureUserManager();
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<LoginUser, Int64>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 8,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(10);
            manager.MaxFailedAccessAttemptsBeforeLockout = 6;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<LoginUser, Int64>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<LoginUser, Int64>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = pOptions.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<LoginUser, Int64>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }
}