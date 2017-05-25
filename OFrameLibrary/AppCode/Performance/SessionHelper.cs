using OFrameLibrary.Models;
using System;
using System.Web;

namespace OFrameLibrary.Performance
{
    public static class SessionHelper
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
            var ss = new SerializableSession
            {
                SessionObject = o
            };
            HttpContext.Current.Session.Add(key, ss);
        }

        /// <summary>
        /// Remove item from cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        public static void Clear(string key)
        {
            if (Exists(key))
            {
                HttpContext.Current.Session.Remove(key);
            }
        }

        /// <summary>
        /// Check for item in cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        /// <returns></returns>
        public static bool Exists(string key)
        {
            return HttpContext.Current.Session[key] != null;
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

                var ss = new SerializableSession();
                ss = (SerializableSession)HttpContext.Current.Session[key];

                value = (T)ss.SessionObject;
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
                var ss = new SerializableSession();
                ss = (SerializableSession)HttpContext.Current.Session[key];

                value = (T)ss.SessionObject;
            }

            return value;
        }
    }
}
