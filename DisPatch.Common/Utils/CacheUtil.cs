using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace DisPatch.Common.Helpers
{
    public static class CacheUtil
    {
        /// <summary>  
        /// 获取数据缓存  
        /// </summary>  
        /// <param name="cacheKey">键</param>  
        public static object GetCache(string cacheKey)
        {
            ObjectCache cache = MemoryCache.Default;
            return cache[cacheKey];
        }

        /// <summary>  
        /// 设置数据缓存
        /// </summary>  
        public static void SetCache(string cacheKey, object objObject)
        {
            SetCache(cacheKey, objObject, 0);
        }

        /// <summary>  
        /// 设置数据缓存  
        /// </summary>  
        public static void SetCache(string cacheKey, object objObject, int timeout = 7200)
        {
            try
            {
                ObjectCache cache = MemoryCache.Default;
                CacheItemPolicy policy = new CacheItemPolicy();
                if (timeout > 0)
                {
                    policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(timeout);
                }
                cache.Set(cacheKey, objObject, policy);
            }
            catch (Exception)
            {
                //throw;  
            }
        }

        /// <summary>  
        /// 移除指定数据缓存  
        /// </summary>  
        public static void RemoveCache(string cacheKey)
        {
            ObjectCache cache = MemoryCache.Default;
            cache.Remove(cacheKey);
        }
    }
}
