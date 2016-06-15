using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using OFrameLibrary;
using OFrameLibrary.Helpers;
using Owin;
using System;

[assembly: OwinStartup(typeof(Startup))]

namespace OFrameLibrary
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(AppDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString(AppConfig.LoginPath),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>
                    (
                        validateInterval: TimeSpan.FromMinutes(AppConfig.CookieValidateInterval),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager)
                    )
                },
                ExpireTimeSpan = TimeSpan.FromDays(AppConfig.CookieExpireTimeSpan),
                SlidingExpiration = AppConfig.CookieSlidingExpiration
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(AppConfig.TwoFactorAuthWaitTime));

            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            app.UseMicrosoftAccountAuthentication(
                clientId: AppConfig.MicrosoftAPIKey,
                clientSecret: AppConfig.MicrosoftSecretKey);

            app.UseTwitterAuthentication(
               consumerKey: AppConfig.TwitterAPIKey,
               consumerSecret: AppConfig.TwitterSecretKey);

            var fo = new Microsoft.Owin.Security.Facebook.FacebookAuthenticationOptions()
            {
                AppId = AppConfig.FacebookAPIKey,
                AppSecret = AppConfig.FacebookSecretKey,
                Provider = new Microsoft.Owin.Security.Facebook.FacebookAuthenticationProvider()
            };

            fo.Scope.Add("email");

            app.UseFacebookAuthentication(fo);

            var go = new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = AppConfig.GoogleAPIKey,
                ClientSecret = AppConfig.GoogleSecretKey,
                Provider = new GoogleOAuth2AuthenticationProvider()
            };

            go.Scope.Add("email");

            app.UseGoogleAuthentication(go);
        }
    }
}