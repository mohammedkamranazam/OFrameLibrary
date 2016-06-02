using OFrameLibrary.SettingsHelpers;
using OFrameLibrary.Util;
using System.Configuration;
using System.Runtime.Caching;
using System.Web;

namespace OFrameLibrary
{
    public static class AppConfig
    {
        public static string ApplicationRedisAuthenticationKey
        {
            get
            {
                SymCryptography sm = new SymCryptography();
                return sm.Decrypt(KeywordsHelper.GetKeywordValue("ApplicationRedisAuthenticationKey"));
            }
            set
            {
                SymCryptography sm = new SymCryptography();
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
                return DataParser.BoolParse(KeywordsHelper.GetKeywordValue("RedisIsSsl"));
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
                return KeywordsHelper.GetKeywordValue("RedisPassword");
            }
            set
            {
                KeywordsHelper.SetKeywordValue("RedisPassword", value);
            }
        }

        public static bool DisableBcc
        {
            get
            {
                return DataParser.BoolParse(KeywordsHelper.GetKeywordValue("DisableBcc"));
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
                return DataParser.BoolParse(KeywordsHelper.GetKeywordValue("DisableBypassListManagement"));
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
                return DataParser.BoolParse(KeywordsHelper.GetKeywordValue("DisableClickTracking"));
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
                return DataParser.BoolParse(KeywordsHelper.GetKeywordValue("DisableFooter"));
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
                return DataParser.BoolParse(KeywordsHelper.GetKeywordValue("DisableGoogleAnalytics"));
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
                return DataParser.BoolParse(KeywordsHelper.GetKeywordValue("DisableGravatar"));
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
                return DataParser.BoolParse(KeywordsHelper.GetKeywordValue("DisableOpenTracking"));
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
                return DataParser.BoolParse(KeywordsHelper.GetKeywordValue("DisableSpamCheck"));
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
                return DataParser.BoolParse(KeywordsHelper.GetKeywordValue("DisableTemplate"));
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
                return DataParser.BoolParse(KeywordsHelper.GetKeywordValue("DisableUnsubscribe"));
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
                return KeywordsHelper.GetKeywordValue("MAITelemetricKey");
            }

            set
            {
                KeywordsHelper.SetKeywordValue("MAITelemetricKey", value);
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
                return DataParser.BoolParse(KeywordsHelper.GetKeywordValue("AllowGuestBuy"));
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
                return DataParser.BoolParse(KeywordsHelper.GetKeywordValue("AppInstalled"));
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
                return DataParser.BoolParse(KeywordsHelper.GetKeywordValue("IsSiteMultiLingual"));
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
                return DataParser.BoolParse(KeywordsHelper.GetKeywordValue("EnableOAuthRegistration"));
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
                return DataParser.IntParse(KeywordsHelper.GetKeywordValue("CookieTimeOutMinutes"));
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
                return DataParser.BoolParse(KeywordsHelper.GetKeywordValue("EnableQA"));
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
                return DataParser.BoolParse(KeywordsHelper.GetKeywordValue("EnableSsl"));
            }
        }

        public static string ErrorAdminEmail
        {
            set
            {
                KeywordsHelper.SetKeywordValue("ErrorAdminEmail", value.ToString());
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
                KeywordsHelper.SetKeywordValue("MailLogOnId", value.ToString());
            }

            get
            {
                return KeywordsHelper.GetKeywordValue("MailLogOnId");
            }
        }

        public static string MailLogOnPassword
        {
            set
            {
                SymCryptography smc = new SymCryptography();

                KeywordsHelper.SetKeywordValue("MailLogOnPassword", smc.Encrypt(value.ToString()));
            }

            get
            {
                SymCryptography smc = new SymCryptography();

                return smc.Decrypt(KeywordsHelper.GetKeywordValue("MailLogOnPassword"));
            }
        }

        public static string MailServer
        {
            set
            {
                KeywordsHelper.SetKeywordValue("MailServer", value.ToString());
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
                return DataParser.IntParse(KeywordsHelper.GetKeywordValue("MailServerPort"), true);
            }
        }

        public static bool MailSmtp
        {
            set
            {
                KeywordsHelper.SetKeywordValue("MailSmtp", value.ToString());
            }

            get
            {
                return DataParser.BoolParse(KeywordsHelper.GetKeywordValue("MailSmtp"));
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
                var cacheItemPriority = string.Empty;

                switch (value)
                {
                    case CacheItemPriority.Default:
                        cacheItemPriority = "Default";
                        break;

                    case CacheItemPriority.NotRemovable:
                        cacheItemPriority = "NotRemovable";
                        break;
                }

                KeywordsHelper.SetKeywordValue("MemoryCacheItemPriority", cacheItemPriority);
            }

            get
            {
                switch (KeywordsHelper.GetKeywordValue("MemoryCacheItemPriority"))
                {
                    case "Default":
                        return CacheItemPriority.Default;

                    case "NotRemovable":
                        return CacheItemPriority.NotRemovable;

                    default:
                        return CacheItemPriority.Default;
                }
            }
        }

        public static string NoImage
        {
            get
            {
                return string.Format(KeywordsHelper.GetKeywordValue("NoImage"), MainTheme);
            }
        }

        public static PerformanceMode PerformanceMode
        {
            set
            {
                var performanceMode = string.Empty;

                switch (value)
                {
                    case OFrameLibrary.PerformanceMode.None:
                        performanceMode = "None";
                        break;

                    case OFrameLibrary.PerformanceMode.ApplicationState:
                        performanceMode = "ApplicationState";
                        break;

                    case OFrameLibrary.PerformanceMode.Cache:
                        performanceMode = "Cache";
                        break;

                    case OFrameLibrary.PerformanceMode.MemoryCache:
                        performanceMode = "MemoryCache";
                        break;

                    case OFrameLibrary.PerformanceMode.Session:
                        performanceMode = "Session";
                        break;
                }

                KeywordsHelper.SetKeywordValue("PerformanceMode", performanceMode);
            }

            get
            {
                switch (KeywordsHelper.GetKeywordValue("PerformanceMode"))
                {
                    case "ApplicationState":
                        return PerformanceMode.ApplicationState;

                    case "Cache":
                        return PerformanceMode.Cache;

                    case "MemoryCache":
                        return PerformanceMode.MemoryCache;

                    case "Session":
                        return PerformanceMode.Session;

                    default:
                        return PerformanceMode.None;
                }
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
                return DataParser.IntParse(KeywordsHelper.GetKeywordValue("PerformanceTimeOutMinutes"));
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
                KeywordsHelper.SetKeywordValue("SiteName", value.ToString());
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
                KeywordsHelper.SetKeywordValue("WebsiteAdminEmail", value.ToString());
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
                KeywordsHelper.SetKeywordValue("WebsiteMainEmail", value.ToString());
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

        public static string FacebookAPIKey
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("FacebookAPIKey");
            }

            set
            {
                KeywordsHelper.SetKeywordValue("FacebookAPIKey", value);
            }
        }

        public static string FacebookSecretKey
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("FacebookSecretKey");
            }

            set
            {
                KeywordsHelper.SetKeywordValue("FacebookSecretKey", value);
            }
        }

        public static string GoogleAPIKey
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("GoogleAPIKey");
            }

            set
            {
                KeywordsHelper.SetKeywordValue("GoogleAPIKey", value);
            }
        }

        public static string GoogleSecretKey
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("GoogleSecretKey");
            }

            set
            {
                KeywordsHelper.SetKeywordValue("GoogleSecretKey", value);
            }
        }

        public static string TwitterAPIKey
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("TwitterAPIKey");
            }

            set
            {
                KeywordsHelper.SetKeywordValue("TwitterAPIKey", value);
            }
        }

        public static string TwitterSecretKey
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("TwitterSecretKey");
            }

            set
            {
                KeywordsHelper.SetKeywordValue("TwitterSecretKey", value);
            }
        }

        public static string CCAvenueMerchantID
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("CCAvenueMerchantID");
            }
            set
            {
                KeywordsHelper.SetKeywordValue("CCAvenueMerchantID", value);
            }
        }

        public static string CCAvenueAccessCode
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("CCAvenueAccessCode");
            }
            set
            {
                KeywordsHelper.SetKeywordValue("CCAvenueAccessCode", value);
            }
        }

        public static string CCAvenueWorkingKey
        {
            get
            {
                return KeywordsHelper.GetKeywordValue("CCAvenueWorkingKey");
            }
            set
            {
                KeywordsHelper.SetKeywordValue("CCAvenueWorkingKey", value);
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