using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using OFrameLibrary.Helpers;
using Owin;
using System;

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
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,// OFrameLibrary.OFrame.Constants.Keys.ApplicationCookie,
                LoginPath = new PathString("/Authentication/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>
                    (
                        validateInterval: TimeSpan.FromMinutes(0),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager)
                    )
                },
                ExpireTimeSpan = TimeSpan.FromDays(365),
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
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            //Uncomment the following lines to enable logging in with third party login providers
            app.UseMicrosoftAccountAuthentication(
                clientId: "",
                clientSecret: "");

            app.UseTwitterAuthentication(
               consumerKey: "",
               consumerSecret: "");

            var fo = new Microsoft.Owin.Security.Facebook.FacebookAuthenticationOptions()
            {
                AppId = "",
                AppSecret = "",
                Provider = new Microsoft.Owin.Security.Facebook.FacebookAuthenticationProvider()
            };

            fo.Scope.Add("email");

            app.UseFacebookAuthentication(fo);

            var go = new GoogleOAuth2AuthenticationOptions()
            {
                //CallbackPath = Microsoft.Owin.PathString.FromUriComponent("/Authentication/ExternalLoginCallback"),
                ClientId = "",
                ClientSecret = "",
                Provider = new GoogleOAuth2AuthenticationProvider()
            };

            go.Scope.Add("email");

            app.UseGoogleAuthentication(go);
        }
    }
}