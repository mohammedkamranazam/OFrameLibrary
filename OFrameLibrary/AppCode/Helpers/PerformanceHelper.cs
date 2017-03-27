using OFrameLibrary.Performance;
using System;

namespace OFrameLibrary.Helpers
{
    public static class PerformanceHelper
    {
        public static void ClearPerformance(string performanceKey)
        {
            ClearPerformance(performanceKey, AppConfig.PerformanceMode);
        }

        public static void ClearPerformance(string performanceKey, PerformanceMode performanceMode)
        {
            switch (performanceMode)
            {
                case PerformanceMode.ApplicationState:
                    if (ApplicationStateHelper.Exists(performanceKey))
                    {
                        ApplicationStateHelper.Clear(performanceKey);
                    }
                    break;

                case PerformanceMode.Cache:
                    if (CacheHelper.Exists(performanceKey))
                    {
                        CacheHelper.Clear(performanceKey);
                    }
                    break;

                case PerformanceMode.MemoryCache:
                    if (MemoryCacheHelper.Exists(performanceKey))
                    {
                        MemoryCacheHelper.Clear(performanceKey);
                    }
                    break;

                case PerformanceMode.Session:
                    if (SessionHelper.Exists(performanceKey))
                    {
                        SessionHelper.Clear(performanceKey);
                    }
                    break;

                case PerformanceMode.Redis:
                    if (RedisCacheHelper.Exists(performanceKey))
                    {
                        RedisCacheHelper.Clear(performanceKey);
                    }
                    break;

                case PerformanceMode.None:
                    break;
            }
        }

        public static void GetPerformance<T>(PerformanceMode performanceMode, string performanceKey, out T keyValue, Delegate func, params object[] args)
        {
            keyValue = default(T);

            switch (performanceMode)
            {
                case PerformanceMode.ApplicationState:
                    if (ApplicationStateHelper.Exists(performanceKey))
                    {
                        ApplicationStateHelper.Get<T>(performanceKey, out keyValue);
                    }
                    else
                    {
                        keyValue = (T)func.DynamicInvoke(args);
                        ApplicationStateHelper.Add<T>(performanceKey, keyValue);
                    }
                    break;

                case PerformanceMode.Cache:
                    if (CacheHelper.Exists(performanceKey))
                    {
                        CacheHelper.Get<T>(performanceKey, out keyValue);
                    }
                    else
                    {
                        keyValue = (T)func.DynamicInvoke(args);
                        CacheHelper.Add<T>(performanceKey, keyValue);
                    }
                    break;

                case PerformanceMode.MemoryCache:
                    if (MemoryCacheHelper.Exists(performanceKey))
                    {
                        MemoryCacheHelper.Get<T>(performanceKey, out keyValue);
                    }
                    else
                    {
                        keyValue = (T)func.DynamicInvoke(args);
                        MemoryCacheHelper.Add<T>(performanceKey, keyValue);
                    }
                    break;

                case PerformanceMode.Session:
                    if (SessionHelper.Exists(performanceKey))
                    {
                        SessionHelper.Get<T>(performanceKey, out keyValue);
                    }
                    else
                    {
                        keyValue = (T)func.DynamicInvoke(args);
                        SessionHelper.Add<T>(performanceKey, keyValue);
                    }
                    break;

                case PerformanceMode.Redis:
                    if (RedisCacheHelper.Exists(performanceKey))
                    {
                        RedisCacheHelper.Get<T>(performanceKey, out keyValue);
                    }
                    else
                    {
                        keyValue = (T)func.DynamicInvoke(args);
                        RedisCacheHelper.Add<T>(performanceKey, keyValue);
                    }
                    break;

                case PerformanceMode.None:
                    keyValue = (T)func.DynamicInvoke(args);
                    break;
            }
        }

        public static T SetOrGet<T>(string performanceKey, T value)
        {
            switch (AppConfig.PerformanceMode)
            {
                case PerformanceMode.ApplicationState:
                    return ApplicationStateHelper.SetOrGet<T>(performanceKey, value);

                case PerformanceMode.Cache:
                    return CacheHelper.SetOrGet<T>(performanceKey, value);

                case PerformanceMode.MemoryCache:
                    return MemoryCacheHelper.SetOrGet<T>(performanceKey, value);

                case PerformanceMode.Session:
                    return SessionHelper.SetOrGet<T>(performanceKey, value);

                case PerformanceMode.Redis:
                    return RedisCacheHelper.SetOrGet<T>(performanceKey, value);
            }

            return value;
        }
    }
}
