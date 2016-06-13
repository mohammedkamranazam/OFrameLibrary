using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using OFrameLibrary.Helpers;
using System;

namespace OFrameLibrary.Helpers
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<AppDbContext>()));
            
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = AppConfig.AllowAlphaNumericUserNames,
                RequireUniqueEmail = AppConfig.RequireUniqueEmail
            };

            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = AppConfig.PasswordRequiredLength,
                RequireDigit = AppConfig.RequireDigit,
                RequireLowercase = AppConfig.RequireLowerCase,
                RequireNonLetterOrDigit = AppConfig.RequireNonLetterOrDigit,
                RequireUppercase = AppConfig.RequireUpperCase
            };

            manager.ClaimsIdentityFactory = new AppUserClaimsIdentityFactory();

            manager.UserLockoutEnabledByDefault = AppConfig.UserLockoutEnabledByDefault;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(AppConfig.DefaultAccountLockoutTimeSpan);
            manager.MaxFailedAccessAttemptsBeforeLockout = AppConfig.MaxFailedAccessAttemptsBeforeLockout;

            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "{0}"
            });

            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "{0}"
            });

            manager.EmailService = new EmailService();

            manager.SmsService = new SmsService();

            var dataProtectionProvider = options.DataProtectionProvider;

            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create(AppConfig.SiteName))
                {
                    TokenLifespan = TimeSpan.FromMinutes(AppConfig.TokenLifeSpan)
                };
            }

            return manager;
        }
    }
}