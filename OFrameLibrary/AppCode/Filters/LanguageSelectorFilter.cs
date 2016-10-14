using OFrameLibrary.Helpers;
using OFrameLibrary.SettingsHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace OFrameLibrary.Filters
{
    public class LanguageSelectorFilter : ActionFilterAttribute, IActionFilter
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
                var storedLocale = CookiesHelper.GetCookie(Constants.Keys.CurrentCultureCookieKey);

                locale = (storedLocale == null) ? AppConfig.DefaultLocale : storedLocale;
            }

            CookiesHelper.SetCookie(Constants.Keys.CurrentCultureCookieKey, locale, now.AddYears(1));

            CookiesHelper.SetCookie(Constants.Keys.CurrentCultureDirectionCookieKey, LanguageHelper.GetLocaleDirection(locale), now.AddYears(1));

            filterContext.HttpContext.Session[Constants.Keys.CurrentCultureSessionKey] = locale;

            this.OnActionExecuting(filterContext);
        }

    }
}
