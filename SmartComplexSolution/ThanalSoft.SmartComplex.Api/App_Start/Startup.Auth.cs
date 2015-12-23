using System;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using ThanalSoft.SmartComplex.Api.Security;
using ThanalSoft.SmartComplex.DataAccess;

namespace ThanalSoft.SmartComplex.Api
{
    public partial class Startup
    {
        private static OAuthAuthorizationServerOptions OAuthOptions { get; set; }

        private static string PublicClientId { get; set; }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        private void ConfigureAuth(IAppBuilder pApp)
        {
            pApp.CreatePerOwinContext(SmartComplexDataObjectContext.Create);
            pApp.CreatePerOwinContext<SecureUserManager>(SecureUserManager.Create);
            pApp.CreatePerOwinContext<SecureSignInManager>(SecureSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            pApp.UseCookieAuthentication(new CookieAuthenticationOptions());
            pApp.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Configure the application for OAuth based flow
            PublicClientId = "self";
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/secureaccess"),
                Provider = new SecureOAuthProvider(PublicClientId),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                AllowInsecureHttp = true // In production mode set AllowInsecureHttp = false
            };

            // Enable the application to use bearer tokens to authenticate users
            pApp.UseOAuthBearerTokens(OAuthOptions);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");

            //app.UseFacebookAuthentication(
            //    appId: "",
            //    appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }
    }
}
