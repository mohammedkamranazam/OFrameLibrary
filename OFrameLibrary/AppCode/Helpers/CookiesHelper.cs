using System;
using System.Web;

namespace OFrameLibrary.Helpers
{
    public static class CookiesHelper
    {
        public static HttpCookie GetCookieResp(string key)
        {
            var context = HttpContext.Current;

            var cookieInResponse = new HttpCookie(key, string.Empty);

            if (context != null)
            {
                cookieInResponse = context.Response.Cookies[key];
            }

            return (cookieInResponse == null || string.IsNullOrWhiteSpace(cookieInResponse.Value)) ? null : cookieInResponse;
        }

        public static string GetCookie(string key)
        {
            var cookieInResponse = GetCookieResp(key);
            var cookieInRequest = GetCookieReq(key);

            if (cookieInResponse != null)
            {
                return cookieInResponse.Value;
            }
            else if (cookieInRequest != null)
            {
                return cookieInRequest.Value;
            }

            return null;
        }

        public static HttpCookie GetCookieReq(string key)
        {
            var context = HttpContext.Current;

            var cookieInRequest = new HttpCookie(key, string.Empty);

            if (context != null)
            {
                cookieInRequest = context.Request.Cookies[key];
            }


            return (cookieInRequest == null || string.IsNullOrWhiteSpace(cookieInRequest.Value)) ? null : cookieInRequest;
        }

        public static void SetCookie(string key, string value, DateTime expires)
        {
            var cookie = new HttpCookie(key, value);
            cookie.Expires = expires;
            cookie.HttpOnly = true;

            if (HttpContext.Current.Response.Cookies[key] != null)
            {
                HttpContext.Current.Response.Cookies.Set(cookie);
            }
            else
            {
                HttpContext.Current.Response.AppendCookie(cookie);
            }

            //HttpContext.Current.Request.Cookies.Add(cookie);
        }
    }
}