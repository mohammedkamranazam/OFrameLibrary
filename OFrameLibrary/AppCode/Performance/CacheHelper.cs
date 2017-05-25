using OFrameLibrary.Util;
using System;
using System.Web;

namespace OFrameLibrary.Performance
{
    public static class CacheHelper
    {
        /// <summary>
        /// Insert value into the cache using
        /// appropriate name/value pairs
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="o">Item to be cached</param>
        /// <param name="key">Name of item</param>
        public static void Add<T>(string key, T o)
        {
            HttpContext.Current.Cache.Insert(
                key,
                o,
                null,
                Utilities.DateTimeNow().AddMinutes(AppConfig.CookieTimeOutMinutes),
                System.Web.Caching.Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// Remove item from cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        public static void Clear(string key)
        {
            HttpContext.Current.Cache.Remove(key);
        }

        /// <summary>
        /// Check for item in cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        /// <returns></returns>
        public static bool Exists(string key)
        {
            return HttpContext.Current.Cache[key] != null;
        }

        /// <summary>
        /// Retrieve cached item
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Name of cached item</param>
        /// <param name="value">Cached value. Default(T) if item doesn't exist.</param>
        /// <returns>Cached item as type</returns>
        public static bool Get<T>(string key, out T value)
        {
            try
            {
                if (!Exists(key))
                {
                    value = default(T);
                    return false;
                }

                value = (T)HttpContext.Current.Cache[key];
            }
            catch (Exception)
            {
                value = default(T);
                return false;
            }

            return true;
        }

        public static T SetOrGet<T>(string key, T value)
        {
            if (!Exists(key))
            {
                Add(key, value);
            }
            else
            {
                value = (T)HttpContext.Current.Cache[key];
            }

            return value;
        }
    }
}
