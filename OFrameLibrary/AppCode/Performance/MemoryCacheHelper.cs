using System;
using System.Runtime.Caching;

namespace OFrameLibrary.Performance
{
    public static class MemoryCacheHelper
    {
        private static readonly ObjectCache memoryCache = MemoryCache.Default;

        private static CacheItemPolicy policy;

        public static void Add<T>(string key, T o)
        {
            policy = new CacheItemPolicy();

            policy.Priority = AppConfig.MemoryCacheItemPriority;

            policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(AppConfig.PerformanceTimeOutMinutes);

            memoryCache.Set(key, o, policy);
        }

        public static void Clear(string key)
        {
            memoryCache.Remove(key);
        }

        public static bool Exists(string key)
        {
            return (memoryCache.Contains(key));
        }

        public static bool Get<T>(string key, out T value)
        {
            try
            {
                if (!Exists(key))
                {
                    value = default(T);
                    return false;
                }

                value = (T)memoryCache[key];
            }
            catch
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
                value = (T)memoryCache[key];
            }

            return value;
        }
    }
}
