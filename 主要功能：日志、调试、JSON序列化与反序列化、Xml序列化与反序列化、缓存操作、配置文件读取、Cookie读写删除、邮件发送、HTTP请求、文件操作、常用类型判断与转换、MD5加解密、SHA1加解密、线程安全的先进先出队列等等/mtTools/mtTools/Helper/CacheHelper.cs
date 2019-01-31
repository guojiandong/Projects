using System;
using System.Web;
using System.Web.Caching;

namespace mtTools
{
    /// <summary>
    /// 缓存操作类
    /// </summary>
    public static class CacheHelper
    {

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <returns>返回缓存数据：不存在则为null</returns>
        public static object GetCache(string key)
        {
            return HttpRuntime.Cache[key];
        }

        /// <summary>
        /// 移除缓存数据
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <returns></returns>
        public static object RemoveCache(string key)
        {
            return HttpRuntime.Cache.Remove(key);
        }

        /// <summary>
        /// 移除缓存key中带某关键字的缓存
        /// </summary>
        /// <param name="keyInclude">缓存Key中包含的关键字</param>
        public static void RemoveMultiCache(string keyInclude)
        {
            var CacheEnum = HttpRuntime.Cache.GetEnumerator();
            while (CacheEnum.MoveNext())
            {
                if (string.IsNullOrEmpty(keyInclude) || CacheEnum.Key.ToString().IndexOf(keyInclude) >= 0)
                    HttpRuntime.Cache.Remove(CacheEnum.Key.ToString());
            }
        }

        /// <summary>
        /// 移除所有缓存
        /// </summary>
        public static void RemoveAllCache()
        {
            RemoveMultiCache(null);
        }

        /// <summary>
        /// 插入缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存数据</param>
        /// <param name="absoluteMinutes">缓存过期时间（分钟）【该时长一到则强制过期】</param>
        /// <param name="dependencies">缓存文件依赖项或缓存key依赖项，无则null【任何依赖项更改时过期并移除】</param>
        /// <param name="priority">缓存移除优先级，默认Default（即Normal）</param>
        /// <param name="onRemovedCallback">缓存被移除时调用的委托，无则null</param>
        /// <returns></returns>
        public static void InsertCache(string key, object value, int absoluteMinutes, CacheDependency dependencies = null, CacheItemPriority priority = CacheItemPriority.Default, CacheItemRemovedCallback onRemovedCallback = null)
        {
            HttpRuntime.Cache.Insert(key, value, dependencies, System.DateTime.UtcNow.AddMinutes(absoluteMinutes), Cache.NoSlidingExpiration, priority, onRemovedCallback);
        }

        /// <summary>
        /// 插入缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存数据</param>
        /// <param name="slidingExpiration">缓存超时时间【超过该时长内没有访问此数据则强制过期，一经访问则重新开始计时】</param>
        /// <param name="dependencies">缓存文件依赖项或缓存key依赖项，无则null【任何依赖项更改时过期并移除】</param>
        /// <param name="priority">缓存移除优先级，默认Default（即Normal）</param>
        /// <param name="onRemovedCallback">缓存被移除时调用的委托，无则null</param>
        /// <returns></returns>
        public static void InsertCache(string key, object value, TimeSpan slidingExpiration, CacheDependency dependencies = null, CacheItemPriority priority = CacheItemPriority.Default, CacheItemRemovedCallback onRemovedCallback = null)
        {
            HttpRuntime.Cache.Insert(key, value, dependencies, Cache.NoAbsoluteExpiration, slidingExpiration, priority, onRemovedCallback);
        }

    }
}
