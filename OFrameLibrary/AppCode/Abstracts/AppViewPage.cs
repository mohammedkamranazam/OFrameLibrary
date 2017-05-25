using OFrameLibrary.Factories;
using OFrameLibrary.Helpers;
using OFrameLibrary.SettingsHelpers;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace OFrameLibrary.Abstracts
{
    public abstract class AppViewPage<TModel> : WebViewPage<TModel>
    {
        protected AppUserPrincipal CurrentUser
        {
            get
            {
                return new AppUserPrincipal(this.User as ClaimsPrincipal);
            }
        }

        protected string Direction
        {
            get
            {
                return CookiesHelper.GetCookie(Constants.Keys.CurrentCultureDirectionCookieKey);
            }
        }

        protected string Locale
        {
            get
            {
                return CookiesHelper.GetCookie(Constants.Keys.CurrentCultureCookieKey);
            }
        }

        protected string LocaleName
        {
            get
            {
                return LanguageHelper.GetLanguages().FirstOrDefault(c => c.Locale == Locale)?.Name;
            }
        }

        protected string Theme
        {
            set
            {
                Layout = ThemeHelper.GetTheme(value);
            }
        }

        protected static string Language(string key)
        {
            return LanguageHelper.GetKey(key);
        }

        protected static string Language(string key, string locale)
        {
            return LanguageHelper.GetKey(key, locale);
        }

        protected static IHtmlString ScriptRender(string theme)
        {
            return Scripts.Render(string.Format("~/Script_{0}", theme));
        }

        protected static IHtmlString StyleRender(string theme)
        {
            return Styles.Render(string.Format("~/Theme_{0}", theme));
        }
    }

    public abstract class AppViewPage : AppViewPage<dynamic>
    {
    }
}
