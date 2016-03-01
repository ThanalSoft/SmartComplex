using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using ThanalSoft.SmartComplex.DataObjects.Security;

namespace ThanalSoft.SmartComplex.Api.Security
{
    public class SecureOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public SecureOAuthProvider(string pUblicClientId)
        {
            if (pUblicClientId == null)
            {
                throw new ArgumentNullException("pUblicClientId");
            }

            _publicClientId = pUblicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext pContext)
        {
            var userManager = pContext.OwinContext.GetUserManager<SecureUserManager>();

            LoginUser user = await userManager.FindAsync(pContext.UserName, pContext.Password);

            if (user == null)
            {
                pContext.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, OAuthDefaults.AuthenticationType);
            ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager, CookieAuthenticationDefaults.AuthenticationType);

            AuthenticationProperties properties = CreateProperties(user.UserName);
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            pContext.Validated(ticket);
            pContext.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext pContext)
        {
            foreach (KeyValuePair<string, string> property in pContext.Properties.Dictionary)
            {
                pContext.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext pContext)
        {
            // Resource owner password credentials does not provide a client ID.
            if (pContext.ClientId == null)
            {
                pContext.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext pContext)
        {
            if (pContext.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(pContext.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == pContext.RedirectUri)
                {
                    pContext.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string pUserName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", pUserName }
            };
            return new AuthenticationProperties(data);
        }
    }
}