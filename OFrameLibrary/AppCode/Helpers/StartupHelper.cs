using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFrameLibrary.Helpers
{
    public static class StartupHelper
    {
        public static void UseGoogleAuthentication(IAppBuilder app)
        {
            if (!string.IsNullOrWhiteSpace(AppConfig.GoogleAPIKey) && !string.IsNullOrWhiteSpace(AppConfig.GoogleSecretKey))
            {
                var go = new GoogleOAuth2AuthenticationOptions
                {
                    ClientId = AppConfig.GoogleAPIKey,
                    ClientSecret = AppConfig.GoogleSecretKey,
                    Provider = new GoogleOAuth2AuthenticationProvider()
                };

                go.Scope.Add("email");

                app.UseGoogleAuthentication(go);
            }
        }

        public static void UseFacebookAuthentication(IAppBuilder app)
        {
            if (!string.IsNullOrWhiteSpace(AppConfig.FacebookAPIKey) && !string.IsNullOrWhiteSpace(AppConfig.FacebookSecretKey))
            {
                var fo = new Microsoft.Owin.Security.Facebook.FacebookAuthenticationOptions
                {
                    AppId = AppConfig.FacebookAPIKey,
                    AppSecret = AppConfig.FacebookSecretKey,
                    Provider = new Microsoft.Owin.Security.Facebook.FacebookAuthenticationProvider()
                };

                fo.Scope.Add("email");

                app.UseFacebookAuthentication(fo);
            }
        }

        public static void UseTwitterAuthentication(IAppBuilder app)
        {
            if (!string.IsNullOrWhiteSpace(AppConfig.TwitterAPIKey) && !string.IsNullOrWhiteSpace(AppConfig.TwitterSecretKey))
            {
                app.UseTwitterAuthentication(
                           consumerKey: AppConfig.TwitterAPIKey,
                           consumerSecret: AppConfig.TwitterSecretKey);
            }
        }

        public static void UseMicrosoftAccountAuthentication(IAppBuilder app)
        {
            if (!string.IsNullOrWhiteSpace(AppConfig.MicrosoftAPIKey) && !string.IsNullOrWhiteSpace(AppConfig.MicrosoftSecretKey))
            {
                app.UseMicrosoftAccountAuthentication(
                            clientId: AppConfig.MicrosoftAPIKey,
                            clientSecret: AppConfig.MicrosoftSecretKey);
            }
        }

        public static void UseTwoFactorRememberBrowserCookie(IAppBuilder app)
        {
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);
        }

        public static void UseTwoFactorSignInCookie(IAppBuilder app)
        {
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(AppConfig.TwoFactorAuthWaitTime));
        }

        public static void UseExternalSignInCookie(IAppBuilder app)
        {
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
        }

        public static void UseCookieAuthentication(IAppBuilder app)
        {
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
        }

        public static void CreateApplicationRoleManager(IAppBuilder app)
        {
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
        }

        public static void CreateApplicationSignInManager(IAppBuilder app)
        {
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
        }

        public static void CreateApplicationUserManager(IAppBuilder app)
        {
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
        }

        public static void CreateAppDBContext(IAppBuilder app)
        {
            if (!string.IsNullOrWhiteSpace(AppConfig.AppDBContext))
            {
                app.CreatePerOwinContext(AppDbContext.Create);
            }
        }
    }
}
