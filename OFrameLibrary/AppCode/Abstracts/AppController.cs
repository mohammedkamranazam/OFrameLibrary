using Microsoft.AspNet.Identity;
using OFrameLibrary.Factories;
using OFrameLibrary.Helpers;
using OFrameLibrary.SettingsHelpers;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;

namespace OFrameLibrary.Abstracts
{
    public abstract class AppController : Controller
    {
        public AppController()
        {
        }

        public AppUserPrincipal CurrentUser
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

        protected void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
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

        protected ActionResult RedirectToLocal(string url = "", string action = "Index", string controller = "Home")
        {
            return Redirect(GetRedirectUrl(url, action, controller));
        }

        string GetRedirectUrl(string url = "", string action = "Index", string controller = "Home")
        {
            if (string.IsNullOrWhiteSpace(url) || !Url.IsLocalUrl(url))
            {
                return Url.Action(action, controller);
            }

            return url;
        }
    }
}
