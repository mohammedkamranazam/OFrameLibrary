using OFrameLibrary.Helpers;
using OFrameLibrary.SettingsHelpers;
using System.Web.Mvc;

namespace OFrameLibrary.Filters
{
    public sealed class LanguageSelectorFilter : ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            var locale = AppConfig.DefaultLocale;
            var now = Util.Utilities.DateTimeNow();

            if (!string.IsNullOrWhiteSpace(filterContext.HttpContext.Request.QueryString["Lang"]))
            {
                locale = filterContext.HttpContext.Request.QueryString["Lang"];
            }
            else
            {
                locale = CookiesHelper.GetCookie(Constants.Keys.CurrentCultureCookieKey) ?? AppConfig.DefaultLocale;
            }

            CookiesHelper.SetCookie(Constants.Keys.CurrentCultureCookieKey, locale, now.AddYears(1));

            CookiesHelper.SetCookie(Constants.Keys.CurrentCultureDirectionCookieKey, LanguageHelper.GetLocaleDirection(locale), now.AddYears(1));

            filterContext.HttpContext.Session[Constants.Keys.CurrentCultureSessionKey] = locale;

            this.OnActionExecuting(filterContext);
        }
    }
}
