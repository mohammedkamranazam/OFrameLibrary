using OFrameLibrary.SettingsHelpers;
using OFrameLibrary.Util;
using System.Configuration;
using System.Runtime.Caching;
using System.Web;

namespace OFrameLibrary
{
    public static class AppConfig
    {
        //public static string XXXXXX
        //{
        //    get
        //    {
        //        return KeywordsHelper.GetKeywordValue("XXXXXX");
        //    }

        //    set
        //    {
        //        KeywordsHelper.SetKeywordValue("XXXXXX", value);
        //    }
        //}

        public static string PageCacheFile
        {
            get
            {
                return HttpRuntime.AppDomainAppPath + KeywordsHelper.GetKeywordValue("PageCacheFile");
            }
        }

        public static string AppDBContext
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("AppDBContext");
            }

            set
            {
                KeywordsHelper.SetKeywordValue("AppDBContext", value);
            }
        }

        public static bool EnableBundleOptimization
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("EnableBundleOptimization").BoolParse();
            }

            set
            {
                KeywordsHelper.SetKeywordValue("EnableBundleOptimization", value.ToString());
            }
        }

        public static int TwoFactorAuthWaitTime
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("TwoFactorAuthWaitTime").IntParse();
            }

            set
            {
                KeywordsHelper.SetKeywordValue("TwoFactorAuthWaitTime", value.ToString());
            }
        }

        public static int CookieExpireTimeSpan
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("CookieExpireTimeSpan").IntParse();
            }

            set
            {
                KeywordsHelper.SetKeywordValue("CookieExpireTimeSpan", value.ToString());
            }
        }

        public static int CookieValidateInterval
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("CookieValidateInterval").IntParse();
            }

            set
            {
                KeywordsHelper.SetKeywordValue("CookieValidateInterval", value.ToString());
            }
        }

        public static bool CookieSlidingExpiration
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("CookieSlidingExpiration").BoolParse();
            }

            set
            {
                KeywordsHelper.SetKeywordValue("CookieSlidingExpiration", value.ToString());
            }
        }

        public static bool AllowAlphaNumericUserNames
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("AllowAlphaNumericUserNames").BoolParse();
            }

            set
            {
                KeywordsHelper.SetKeywordValue("AllowAlphaNumericUserNames", value.ToString());
            }
        }

        public static bool RequireUniqueEmail
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("RequireUniqueEmail").BoolParse();
            }

            set
            {
                KeywordsHelper.SetKeywordValue("RequireUniqueEmail", value.ToString());
            }
        }

        public static int PasswordRequiredLength
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("PasswordRequiredLength").IntParse();
            }

            set
            {
                KeywordsHelper.SetKeywordValue("PasswordRequiredLength", value.ToString());
            }
        }

        public static bool RequireDigit
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("RequireDigit").BoolParse();
            }

            set
            {
                KeywordsHelper.SetKeywordValue("RequireDigit", value.ToString());
            }
        }

        public static bool RequireLowerCase
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("RequireLowerCase").BoolParse();
            }

            set
            {
                KeywordsHelper.SetKeywordValue("RequireLowerCase", value.ToString());
            }
        }

        public static bool RequireNonLetterOrDigit
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("RequireNonLetterOrDigit").BoolParse();
            }

            set
            {
                KeywordsHelper.SetKeywordValue("RequireNonLetterOrDigit", value.ToString());
            }
        }

        public static bool RequireUpperCase
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("RequireUpperCase").BoolParse();
            }

            set
            {
                KeywordsHelper.SetKeywordValue("RequireUpperCase", value.ToString());
            }
        }

        public static bool UserLockoutEnabledByDefault
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("UserLockoutEnabledByDefault").BoolParse();
            }

            set
            {
                KeywordsHelper.SetKeywordValue("UserLockoutEnabledByDefault", value.ToString());
            }
        }

        public static int DefaultAccountLockoutTimeSpan
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("DefaultAccountLockoutTimeSpan").IntParse();
            }

            set
            {
                KeywordsHelper.SetKeywordValue("DefaultAccountLockoutTimeSpan", value.ToString());
            }
        }

        public static int MaxFailedAccessAttemptsBeforeLockout
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("MaxFailedAccessAttemptsBeforeLockout").IntParse();
            }

            set
            {
                KeywordsHelper.SetKeywordValue("MaxFailedAccessAttemptsBeforeLockout", value.ToString());
            }
        }

        public static int TokenLifeSpan
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("TokenLifeSpan").IntParse();
            }

            set
            {
                KeywordsHelper.SetKeywordValue("TokenLifeSpan", value.ToString());
            }
        }

        public static string EventLogSourceName
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("EventLogSourceName");
            }

            set
            {
                KeywordsHelper.SetKeywordValue("EventLogSourceName", value);
            }
        }

        public static string SMSServiceName
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("SMSServiceName");
            }

            set
            {
                KeywordsHelper.SetKeywordValue("SMSServiceName", value);
            }
        }

        public static string SMSAccountIdentification
        {
            get
            {
                var value = KeywordsHelper.GetKeywordValue("SMSAccountIdentification");

                if (string.IsNullOrWhiteSpace(value))
                {
                    return string.Empty;
                }

                try
                {
                    var sm = new SymCryptography();
                    return sm.Decrypt(value);
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                var sm = new SymCryptography();
                KeywordsHelper.SetKeywordValue("SMSAccountIdentification", sm.Encrypt(value));
            }
        }

        public static string SMSAccountPassword
        {
            get
            {
                var value = KeywordsHelper.GetKeywordValue("SMSAccountPassword");

                if (string.IsNullOrWhiteSpace(value))
                {
                    return string.Empty;
                }

                try
                {
                    var sm = new SymCryptography();
                    return sm.Decrypt(value);
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                var sm = new SymCryptography();
                KeywordsHelper.SetKeywordValue("SMSAccountPassword", sm.Encrypt(value));
            }
        }

        public static string SMSAccountFrom
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("SMSAccountFrom");
            }

            set
            {
                KeywordsHelper.SetKeywordValue("SMSAccountFrom", value);
            }
        }

        public static string EmailServiceName
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("EmailServiceName");
            }

            set
            {
                KeywordsHelper.SetKeywordValue("EmailServiceName", value);
            }
        }

        public static string SendGridServer
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("SendGridServer");
            }

            set
            {
                KeywordsHelper.SetKeywordValue("SendGridServer", value);
            }
        }

        public static string SendGridUsername
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("SendGridUsername");
            }

            set
            {
                KeywordsHelper.SetKeywordValue("SendGridUsername", value);
            }
        }

        public static string SendGridPassword
        {
            get
            {
                var value = KeywordsHelper.GetKeywordValue("SendGridPassword");

                if (string.IsNullOrWhiteSpace(value))
                {
                    return string.Empty;
                }

                try
                {
                    var sm = new SymCryptography();
                    return sm.Decrypt(value);
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                var sm = new SymCryptography();
                KeywordsHelper.SetKeywordValue("SendGridPassword", sm.Encrypt(value));
            }
        }

        public static string MailLabel
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("MailLabel");
            }

            set
            {
                KeywordsHelper.SetKeywordValue("MailLabel", value);
            }
        }

        public static string LoginPath
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("LoginPath");
            }

            set
            {
                KeywordsHelper.SetKeywordValue("LoginPath", value);
            }
        }

        public static string ApplicationRedisAuthenticationKey
        {
            get
            {
                var value = KeywordsHelper.GetKeywordValue("ApplicationRedisAuthenticationKey");

                if (string.IsNullOrWhiteSpace(value))
                {
                    return string.Empty;
                }

                try
                {
                    var sm = new SymCryptography();
                    return sm.Decrypt(value);
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                var sm = new SymCryptography();
                KeywordsHelper.SetKeywordValue("ApplicationRedisAuthenticationKey", sm.Encrypt(value));
            }
        }

        public static string RedisHost
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("RedisHost");
            }
            set
            {
                KeywordsHelper.SetKeywordValue("RedisHost", value);
            }
        }

        public static bool RedisIsSsl
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("RedisIsSsl").BoolParse();
            }
            set
            {
                KeywordsHelper.SetKeywordValue("RedisIsSsl", value.ToString());
            }
        }

        public static string RedisPassword
        {
            get
            {
                var value = KeywordsHelper.GetKeywordValue("RedisPassword");

                if (string.IsNullOrWhiteSpace(value))
                {
                    return string.Empty;
                }

                try
                {
                    var sm = new SymCryptography();
                    return sm.Decrypt(value);
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                var sm = new SymCryptography();
                KeywordsHelper.SetKeywordValue("RedisPassword", sm.Encrypt(value));
            }
        }

        public static bool DisableBcc
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("DisableBcc").BoolParse();
            }
            set
            {
                KeywordsHelper.SetKeywordValue("DisableBcc", value.ToString());
            }
        }

        public static bool DisableBypassListManagement
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("DisableBypassListManagement").BoolParse();
            }
            set
            {
                KeywordsHelper.SetKeywordValue("DisableBypassListManagement", value.ToString());
            }
        }

        public static bool DisableClickTracking
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("DisableClickTracking").BoolParse();
            }
            set
            {
                KeywordsHelper.SetKeywordValue("DisableClickTracking", value.ToString());
            }
        }

        public static bool DisableFooter
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("DisableFooter").BoolParse();
            }
            set
            {
                KeywordsHelper.SetKeywordValue("DisableFooter", value.ToString());
            }
        }

        public static bool DisableGoogleAnalytics
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("DisableGoogleAnalytics").BoolParse();
            }
            set
            {
                KeywordsHelper.SetKeywordValue("DisableGoogleAnalytics", value.ToString());
            }
        }

        public static bool DisableGravatar
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("DisableGravatar").BoolParse();
            }
            set
            {
                KeywordsHelper.SetKeywordValue("DisableGravatar", value.ToString());
            }
        }

        public static bool DisableOpenTracking
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("DisableOpenTracking").BoolParse();
            }
            set
            {
                KeywordsHelper.SetKeywordValue("DisableOpenTracking", value.ToString());
            }
        }

        public static bool DisableSpamCheck
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("DisableSpamCheck").BoolParse();
            }
            set
            {
                KeywordsHelper.SetKeywordValue("DisableSpamCheck", value.ToString());
            }
        }

        public static bool DisableTemplate
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("DisableTemplate").BoolParse();
            }
            set
            {
                KeywordsHelper.SetKeywordValue("DisableTemplate", value.ToString());
            }
        }

        public static bool DisableUnsubscribe
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("DisableUnsubscribe").BoolParse();
            }
            set
            {
                KeywordsHelper.SetKeywordValue("DisableUnsubscribe", value.ToString());
            }
        }

        public static string MAITelemetricKey
        {
            get
            {
                var value = KeywordsHelper.GetKeywordValue("MAITelemetricKey");

                if (string.IsNullOrWhiteSpace(value))
                {
                    return string.Empty;
                }

                try
                {
                    var sm = new SymCryptography();
                    return sm.Decrypt(value);
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                var sm = new SymCryptography();
                KeywordsHelper.SetKeywordValue("MAITelemetricKey", sm.Encrypt(value));
            }
        }

        public static string MicrosoftApplicationInsightScript
        {
            get
            {
                return HttpUtility.HtmlDecode(KeywordsHelper.GetKeywordValue("MicrosoftApplicationInsightScript"));
            }

            set
            {
                KeywordsHelper.SetKeywordValue("MicrosoftApplicationInsightScript", HttpUtility.HtmlEncode(value));
            }
        }

        public static string HeaderTitle
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("HeaderTitle");
            }

            set
            {
                KeywordsHelper.SetKeywordValue("HeaderTitle", value);
            }
        }

        public static string HeaderTagLine
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("HeaderTagLine");
            }

            set
            {
                KeywordsHelper.SetKeywordValue("HeaderTagLine", value);
            }
        }

        public static string EmptyPanelImage
        {
            get
            {
                return string.Format(KeywordsHelper.GetKeywordValue("EmptyPanelImage"), MainTheme);
            }
        }

        public static string DiscussCode
        {
            set
            {
                KeywordsHelper.SetKeywordValue("DiscussCode", HttpUtility.HtmlEncode(value));
            }

            get
            {
                return HttpUtility.HtmlDecode(KeywordsHelper.GetKeywordValue("DiscussCode"));
            }
        }

        public static bool AllowGuestBuy
        {
            set
            {
                KeywordsHelper.SetKeywordValue("AllowGuestBuy", value.ToString());
            }

            get
            {
                return KeywordsHelper.GetKeywordValue("AllowGuestBuy").BoolParse();
            }
        }

        public static bool AppInstalled
        {
            set
            {
                KeywordsHelper.SetKeywordValue("AppInstalled", value.ToString());
            }

            get
            {
                return KeywordsHelper.GetKeywordValue("AppInstalled").BoolParse();
            }
        }

        public static bool IsSiteMultiLingual
        {
            set
            {
                KeywordsHelper.SetKeywordValue("IsSiteMultiLingual", value.ToString());
            }

            get
            {
                return KeywordsHelper.GetKeywordValue("IsSiteMultiLingual").BoolParse();
            }
        }

        public static bool EnableOAuthRegistration
        {
            set
            {
                KeywordsHelper.SetKeywordValue("EnableOAuthRegistration", value.ToString());
            }

            get
            {
                return KeywordsHelper.GetKeywordValue("EnableOAuthRegistration").BoolParse();
            }
        }

        public static string BingWebmasterCenter
        {
            set
            {
                KeywordsHelper.SetKeywordValue("BingWebmasterCenter", value);
            }

            get
            {
                return KeywordsHelper.GetKeywordValue("BingWebmasterCenter");
            }
        }

        public static string CheckOutTheme
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("CheckOutTheme");
            }
        }

        public static int CookieTimeOutMinutes
        {
            set
            {
                KeywordsHelper.SetKeywordValue("CookieTimeOutMinutes", value.ToString());
            }

            get
            {
                return KeywordsHelper.GetKeywordValue("CookieTimeOutMinutes").IntParse();
            }
        }

        public static string DefaultLocale
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("DefaultLocale");
            }

            set
            {
                KeywordsHelper.SetKeywordValue("DefaultLocale", value);
            }
        }

        public static string EmailTemplate1
        {
            get
            {
                return HttpRuntime.AppDomainAppPath + KeywordsHelper.GetKeywordValue("EmailTemplate1");
            }

            set
            {
                KeywordsHelper.SetKeywordValue("EmailTemplate1", value);
            }
        }

        public static string EmailTemplate10
        {
            get
            {
                return HttpRuntime.AppDomainAppPath + KeywordsHelper.GetKeywordValue("EmailTemplate10");
            }

            set
            {
                KeywordsHelper.SetKeywordValue("EmailTemplate10", value);
            }
        }

        public static string EmailTemplate2
        {
            get
            {
                return HttpRuntime.AppDomainAppPath + KeywordsHelper.GetKeywordValue("EmailTemplate2");
            }

            set
            {
                KeywordsHelper.SetKeywordValue("EmailTemplate2", value);
            }
        }

        public static string EmailTemplate3
        {
            get
            {
                return HttpRuntime.AppDomainAppPath + KeywordsHelper.GetKeywordValue("EmailTemplate3");
            }

            set
            {
                KeywordsHelper.SetKeywordValue("EmailTemplate3", value);
            }
        }

        public static string EmailTemplate4
        {
            get
            {
                return HttpRuntime.AppDomainAppPath + KeywordsHelper.GetKeywordValue("EmailTemplate4");
            }

            set
            {
                KeywordsHelper.SetKeywordValue("EmailTemplate4", value);
            }
        }

        public static string EmailTemplate5
        {
            get
            {
                return HttpRuntime.AppDomainAppPath + KeywordsHelper.GetKeywordValue("EmailTemplate5");
            }

            set
            {
                KeywordsHelper.SetKeywordValue("EmailTemplate5", value);
            }
        }

        public static string EmailTemplate6
        {
            get
            {
                return HttpRuntime.AppDomainAppPath + KeywordsHelper.GetKeywordValue("EmailTemplate6");
            }

            set
            {
                KeywordsHelper.SetKeywordValue("EmailTemplate6", value);
            }
        }

        public static string EmailTemplate7
        {
            get
            {
                return HttpRuntime.AppDomainAppPath + KeywordsHelper.GetKeywordValue("EmailTemplate7");
            }

            set
            {
                KeywordsHelper.SetKeywordValue("EmailTemplate7", value);
            }
        }

        public static string EmailTemplate8
        {
            get
            {
                return HttpRuntime.AppDomainAppPath + KeywordsHelper.GetKeywordValue("EmailTemplate8");
            }

            set
            {
                KeywordsHelper.SetKeywordValue("EmailTemplate8", value);
            }
        }

        public static string EmailTemplate9
        {
            get
            {
                return HttpRuntime.AppDomainAppPath + KeywordsHelper.GetKeywordValue("EmailTemplate9");
            }

            set
            {
                KeywordsHelper.SetKeywordValue("EmailTemplate9", value);
            }
        }

        public static bool EnableQA
        {
            set
            {
                KeywordsHelper.SetKeywordValue("EnableQA", value.ToString());
            }

            get
            {
                return KeywordsHelper.GetKeywordValue("EnableQA").BoolParse();
            }
        }

        public static bool EnableSsl
        {
            set
            {
                KeywordsHelper.SetKeywordValue("EnableSsl", value.ToString());
            }

            get
            {
                return KeywordsHelper.GetKeywordValue("EnableSsl").BoolParse();
            }
        }

        public static string ErrorAdminEmail
        {
            set
            {
                KeywordsHelper.SetKeywordValue("ErrorAdminEmail", value);
            }

            get
            {
                return KeywordsHelper.GetKeywordValue("ErrorAdminEmail");
            }
        }

        public static string FemaleAvatar
        {
            get
            {
                return string.Format(KeywordsHelper.GetKeywordValue("FemaleAvatar"), ZiceTheme);
            }
        }

        public static string FileEditorSearchPatterns
        {
            set
            {
                KeywordsHelper.SetKeywordValue("FileEditorSearchPatterns", value);
            }

            get
            {
                return KeywordsHelper.GetKeywordValue("FileEditorSearchPatterns");
            }
        }

        public static string GoogleAnalyticsCode
        {
            set
            {
                KeywordsHelper.SetKeywordValue("GoogleAnalyticsCode", HttpUtility.HtmlEncode(value));
            }

            get
            {
                return HttpUtility.HtmlDecode(KeywordsHelper.GetKeywordValue("GoogleAnalyticsCode"));
            }
        }

        public static string GoogleWebmasterTool
        {
            set
            {
                KeywordsHelper.SetKeywordValue("GoogleWebmasterTool", value);
            }

            get
            {
                return KeywordsHelper.GetKeywordValue("GoogleWebmasterTool");
            }
        }

        public static string KeywordsFile
        {
            get
            {
                return HttpRuntime.AppDomainAppPath + ConfigurationManager.AppSettings["KeywordsFile"];
            }
        }

        public static string LanguagesFile
        {
            get
            {
                return HttpRuntime.AppDomainAppPath + KeywordsHelper.GetKeywordValue("LanguagesFile");
            }
        }

        public static string LocalStoragesFile
        {
            get
            {
                return HttpRuntime.AppDomainAppPath + KeywordsHelper.GetKeywordValue("LocalStoragesFile");
            }
        }

        public static string LogoRelativeURL
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("LogoRelativeURL");
            }

            set
            {
                KeywordsHelper.SetKeywordValue("LogoRelativeURL", value);
            }
        }

        public static string MailLogOnId
        {
            set
            {
                KeywordsHelper.SetKeywordValue("MailLogOnId", value);
            }

            get
            {
                return KeywordsHelper.GetKeywordValue("MailLogOnId");
            }
        }

        public static string MailLogOnPassword
        {
            get
            {
                var value = KeywordsHelper.GetKeywordValue("MailLogOnPassword");

                if (string.IsNullOrWhiteSpace(value))
                {
                    return string.Empty;
                }

                try
                {
                    var sm = new SymCryptography();
                    return sm.Decrypt(value);
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                var sm = new SymCryptography();
                KeywordsHelper.SetKeywordValue("MailLogOnPassword", sm.Encrypt(value));
            }
        }

        public static string MailServer
        {
            set
            {
                KeywordsHelper.SetKeywordValue("MailServer", value);
            }

            get
            {
                return KeywordsHelper.GetKeywordValue("MailServer");
            }
        }

        public static int MailServerPort
        {
            set
            {
                KeywordsHelper.SetKeywordValue("MailServerPort", value.ToString());
            }

            get
            {
                return KeywordsHelper.GetKeywordValue("MailServerPort").IntParse(25);
            }
        }

        public static string MainTheme
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("MainTheme");
            }
        }

        public static string MaleAvatar
        {
            get
            {
                return string.Format(KeywordsHelper.GetKeywordValue("MaleAvatar"), ZiceTheme);
            }
        }

        public static CacheItemPriority MemoryCacheItemPriority
        {
            set
            {
                //var cacheItemPriority = string.Empty;

                //switch (value)
                //{
                //    case CacheItemPriority.Default:
                //        cacheItemPriority = "Default";
                //        break;

                //    case CacheItemPriority.NotRemovable:
                //        cacheItemPriority = "NotRemovable";
                //        break;
                //}

                KeywordsHelper.SetKeywordValue("MemoryCacheItemPriority", value.ToString());
            }

            get
            {
                //switch (KeywordsHelper.GetKeywordValue("MemoryCacheItemPriority"))
                //{
                //    case "Default":
                //        return CacheItemPriority.Default;

                //    case "NotRemovable":
                //        return CacheItemPriority.NotRemovable;

                //    default:
                //        return CacheItemPriority.Default;
                //}

                return Utilities.GetEnumByName<CacheItemPriority>(KeywordsHelper.GetKeywordValue("MemoryCacheItemPriority"));
            }
        }

        public static PerformanceMode PerformanceMode
        {
            set
            {
                //var performanceMode = string.Empty;

                //switch (value)
                //{
                //    case PerformanceMode.None:
                //        performanceMode = "None";
                //        break;

                //    case PerformanceMode.ApplicationState:
                //        performanceMode = "ApplicationState";
                //        break;

                //    case PerformanceMode.Cache:
                //        performanceMode = "Cache";
                //        break;

                //    case PerformanceMode.MemoryCache:
                //        performanceMode = "MemoryCache";
                //        break;

                //    case PerformanceMode.Session:
                //        performanceMode = "Session";
                //        break;

                //    case PerformanceMode.Redis:
                //        performanceMode = "Redis";
                //        break;
                //}

                KeywordsHelper.SetKeywordValue("PerformanceMode", value.ToString());
            }

            get
            {
                //switch (KeywordsHelper.GetKeywordValue("PerformanceMode"))
                //{
                //    case "ApplicationState":
                //        return PerformanceMode.ApplicationState;

                //    case "Cache":
                //        return PerformanceMode.Cache;

                //    case "MemoryCache":
                //        return PerformanceMode.MemoryCache;

                //    case "Session":
                //        return PerformanceMode.Session;

                //    case "Redis":
                //        return PerformanceMode.Redis;

                //    default:
                //        return PerformanceMode.None;
                //}

                return Utilities.GetEnumByName<PerformanceMode>(KeywordsHelper.GetKeywordValue("PerformanceMode"));
            }
        }

        public static string NoImage
        {
            get
            {
                return string.Format(KeywordsHelper.GetKeywordValue("NoImage"), MainTheme);
            }
        }

        public static int PerformanceTimeOutMinutes
        {
            set
            {
                KeywordsHelper.SetKeywordValue("PerformanceTimeOutMinutes", value.ToString());
            }

            get
            {
                return KeywordsHelper.GetKeywordValue("PerformanceTimeOutMinutes").IntParse();
            }
        }

        public static string PinterestSiteVerification
        {
            set
            {
                KeywordsHelper.SetKeywordValue("PinterestSiteVerification", value);
            }

            get
            {
                return KeywordsHelper.GetKeywordValue("PinterestSiteVerification");
            }
        }

        public static string PopUpTheme
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("PopUpTheme");
            }
        }

        public static string RemoteServersFile
        {
            get
            {
                return HttpRuntime.AppDomainAppPath + KeywordsHelper.GetKeywordValue("RemoteServersFile");
            }
        }

        public static string SEOFile
        {
            get
            {
                return HttpRuntime.AppDomainAppPath + KeywordsHelper.GetKeywordValue("SEOFile");
            }
        }

        public static string SiteName
        {
            set
            {
                KeywordsHelper.SetKeywordValue("SiteName", value);
            }

            get
            {
                return KeywordsHelper.GetKeywordValue("SiteName");
            }
        }

        public static string TargetTimeZoneID
        {
            set
            {
                KeywordsHelper.SetKeywordValue("TargetTimeZoneID", value);
            }

            get
            {
                return KeywordsHelper.GetKeywordValue("TargetTimeZoneID");
            }
        }

        public static string TwitterWebsite
        {
            set
            {
                KeywordsHelper.SetKeywordValue("TwitterWebsite", value);
            }

            get
            {
                return KeywordsHelper.GetKeywordValue("TwitterWebsite");
            }
        }

        public static string UnspecifiedAvatar
        {
            get
            {
                return string.Format(KeywordsHelper.GetKeywordValue("UnspecifiedAvatar"), ZiceTheme);
            }
        }

        public static string ValidationSettingsFile
        {
            get
            {
                return HttpRuntime.AppDomainAppPath + KeywordsHelper.GetKeywordValue("ValidationSettingsFile");
            }
        }

        public static string WebsiteAdminEmail
        {
            set
            {
                KeywordsHelper.SetKeywordValue("WebsiteAdminEmail", value);
            }

            get
            {
                return KeywordsHelper.GetKeywordValue("WebsiteAdminEmail");
            }
        }

        public static string WebsiteMainEmail
        {
            set
            {
                KeywordsHelper.SetKeywordValue("WebsiteMainEmail", value);
            }

            get
            {
                return KeywordsHelper.GetKeywordValue("WebsiteMainEmail");
            }
        }

        public static string YandexWebmaster
        {
            set
            {
                KeywordsHelper.SetKeywordValue("YandexWebmaster", value);
            }

            get
            {
                return KeywordsHelper.GetKeywordValue("YandexWebmaster");
            }
        }

        public static string ZiceTheme
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("ZiceTheme");
            }
        }

        public static string MicrosoftAPIKey
        {
            get
            {
                var value = KeywordsHelper.GetKeywordValue("MicrosoftAPIKey");

                if (string.IsNullOrWhiteSpace(value))
                {
                    return string.Empty;
                }

                try
                {
                    var sm = new SymCryptography();
                    return sm.Decrypt(value);
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                var sm = new SymCryptography();
                KeywordsHelper.SetKeywordValue("MicrosoftAPIKey", sm.Encrypt(value));
            }
        }

        public static string MicrosoftSecretKey
        {
            get
            {
                var value = KeywordsHelper.GetKeywordValue("MicrosoftSecretKey");

                if (string.IsNullOrWhiteSpace(value))
                {
                    return string.Empty;
                }

                try
                {
                    var sm = new SymCryptography();
                    return sm.Decrypt(value);
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                var sm = new SymCryptography();
                KeywordsHelper.SetKeywordValue("MicrosoftSecretKey", sm.Encrypt(value));
            }
        }

        public static string FacebookAPIKey
        {
            get
            {
                var value = KeywordsHelper.GetKeywordValue("FacebookAPIKey");

                if (string.IsNullOrWhiteSpace(value))
                {
                    return string.Empty;
                }

                try
                {
                    var sm = new SymCryptography();
                    return sm.Decrypt(value);
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                var sm = new SymCryptography();
                KeywordsHelper.SetKeywordValue("FacebookAPIKey", sm.Encrypt(value));
            }
        }

        public static string FacebookSecretKey
        {
            get
            {
                var value = KeywordsHelper.GetKeywordValue("FacebookSecretKey");

                if (string.IsNullOrWhiteSpace(value))
                {
                    return string.Empty;
                }

                try
                {
                    var sm = new SymCryptography();
                    return sm.Decrypt(value);
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                var sm = new SymCryptography();
                KeywordsHelper.SetKeywordValue("FacebookSecretKey", sm.Encrypt(value));
            }
        }

        public static string GoogleAPIKey
        {
            get
            {
                var value = KeywordsHelper.GetKeywordValue("GoogleAPIKey");

                if (string.IsNullOrWhiteSpace(value))
                {
                    return string.Empty;
                }

                try
                {
                    var sm = new SymCryptography();
                    return sm.Decrypt(value);
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                var sm = new SymCryptography();
                KeywordsHelper.SetKeywordValue("GoogleAPIKey", sm.Encrypt(value));
            }
        }

        public static string GoogleSecretKey
        {
            get
            {
                var value = KeywordsHelper.GetKeywordValue("GoogleSecretKey");

                if (string.IsNullOrWhiteSpace(value))
                {
                    return string.Empty;
                }

                try
                {
                    var sm = new SymCryptography();
                    return sm.Decrypt(value);
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                var sm = new SymCryptography();
                KeywordsHelper.SetKeywordValue("GoogleSecretKey", sm.Encrypt(value));
            }
        }

        public static string TwitterAPIKey
        {
            get
            {
                var value = KeywordsHelper.GetKeywordValue("TwitterAPIKey");

                if (string.IsNullOrWhiteSpace(value))
                {
                    return string.Empty;
                }

                try
                {
                    var sm = new SymCryptography();
                    return sm.Decrypt(value);
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                var sm = new SymCryptography();
                KeywordsHelper.SetKeywordValue("TwitterAPIKey", sm.Encrypt(value));
            }
        }

        public static string TwitterSecretKey
        {
            get
            {
                var value = KeywordsHelper.GetKeywordValue("TwitterSecretKey");

                if (string.IsNullOrWhiteSpace(value))
                {
                    return string.Empty;
                }

                try
                {
                    var sm = new SymCryptography();
                    return sm.Decrypt(value);
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                var sm = new SymCryptography();
                KeywordsHelper.SetKeywordValue("TwitterSecretKey", sm.Encrypt(value));
            }
        }

        public static string CCAvenueMerchantID
        {
            get
            {
                var value = KeywordsHelper.GetKeywordValue("CCAvenueMerchantID");

                if (string.IsNullOrWhiteSpace(value))
                {
                    return string.Empty;
                }

                try
                {
                    var sm = new SymCryptography();
                    return sm.Decrypt(value);
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                var sm = new SymCryptography();
                KeywordsHelper.SetKeywordValue("CCAvenueMerchantID", sm.Encrypt(value));
            }
        }

        public static string CCAvenueAccessCode
        {
            get
            {
                var value = KeywordsHelper.GetKeywordValue("CCAvenueAccessCode");

                if (string.IsNullOrWhiteSpace(value))
                {
                    return string.Empty;
                }

                try
                {
                    var sm = new SymCryptography();
                    return sm.Decrypt(value);
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                var sm = new SymCryptography();
                KeywordsHelper.SetKeywordValue("CCAvenueAccessCode", sm.Encrypt(value));
            }
        }

        public static string CCAvenueWorkingKey
        {
            get
            {
                var value = KeywordsHelper.GetKeywordValue("CCAvenueWorkingKey");

                if (string.IsNullOrWhiteSpace(value))
                {
                    return string.Empty;
                }

                try
                {
                    var sm = new SymCryptography();
                    return sm.Decrypt(value);
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                var sm = new SymCryptography();
                KeywordsHelper.SetKeywordValue("CCAvenueWorkingKey", sm.Encrypt(value));
            }
        }

        public static string CCAvenueCurrencyCode
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("CCAvenueCurrencyCode");
            }
            set
            {
                KeywordsHelper.SetKeywordValue("CCAvenueCurrencyCode", value);
            }
        }

        public static string CCAvenueLanguageCode
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("CCAvenueLanguageCode");
            }
            set
            {
                KeywordsHelper.SetKeywordValue("CCAvenueLanguageCode", value);
            }
        }
    }
}