using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using OFrameLibrary.Helpers;
using Owin;
using System;
using System.Diagnostics;

[assembly: OwinStartup(typeof(OFrameLibrary.Startup))]

namespace OFrameLibrary
{
    public class Startup
    {
        // public static Func<UserManager<AppUser>> UserManagerFactory { get; private set; }

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
                        validateInterval: TimeSpan.FromMinutes(0),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager)
                    )
                },
                ExpireTimeSpan = TimeSpan.FromDays(AppConfig.CookieExpireTimeSpan),
                SlidingExpiration = true
            });

            //UserManagerFactory = () =>
            //{
            //    var usermanager = new UserManager<AppUser>(new UserStore<AppUser>(new AppDbContext()));

            //    usermanager.PasswordValidator = new PasswordValidator()
            //    {
            //        RequireDigit = false,
            //        RequiredLength = 1,
            //        RequireLowercase = false,
            //        RequireNonLetterOrDigit = false,
            //        RequireUppercase = false
            //    };

            //    usermanager.UserValidator = new UserValidator<AppUser>(usermanager)
            //    {
            //        AllowOnlyAlphanumericUserNames = false,
            //        RequireUniqueEmail = true
            //    };

            //    usermanager.ClaimsIdentityFactory = new AppUserClaimsIdentityFactory();

            //    return usermanager;
            //};

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(AppConfig.TwoFactorAuthWaitTime));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            //Uncomment the following lines to enable logging in with third party login providers

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