using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Threading;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace CF
{
    /// <summary>
    /// 内存高速缓存
    /// </summary>
    public class cls_Cache
    {
        /// <summary>
        /// 默认对象缓存时间,单位:分钟
        /// </summary>
        private int cacheTime = 20;

        /// <summary>
        /// 最大缓存对象
        /// </summary>
        private int maxCacheObjLength = 10000;


        /// <summary>
        /// 总缓存对象数目
        /// </summary>
        private int TotalCacheObjCount = 0;

        /// <summary>
        /// 总命中数目
        /// </summary>
        private int TotalHitObjCount = 0;



        /// <summary>
        /// 创建并初始化缓存
        /// </summary>
        public void CreatCache()
        {
            CreatCache(cacheTime, 10000);
        }

        /// <summary>
        /// 创建并初始化缓存
        /// </summary>
        /// <param name="CacheTime">缓存时间,单位分钟,默认20</param>
        /// <param name="MaxCacheObjLength">最大缓存对象数目,默认10000</param>
        public void CreatCache(int CacheTime, int MaxCacheObjLength)
        {
            cacheTime = CacheTime;
            maxCacheObjLength = MaxCacheObjLength;
        }


        /// <summary>
        /// 新增内存对象
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Obj"></param>
        public void SetMemCacheObj(string Key, object obj)
        {
            if (System.Web.HttpRuntime.Cache.Count > maxCacheObjLength)
            {
                return;
            }

            RemoveMemCacheObj(Key);

            TotalCacheObjCount++;

            byte[] objValue = CF.cls_Serialize.Serialize2Byte(obj);

            System.Web.HttpRuntime.Cache.Insert(Key, objValue, null, DateTime.Now.AddSeconds(cacheTime * 60), TimeSpan.Zero);
        }

        /// <summary>
        /// 新增内存对象
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="obj"></param>
        /// <param name="InvalidTime">绝对过期时间</param>
        public void SetMemCacheObj(string Key, object obj, DateTime InvalidTime)
        {
            SetMemCacheObj(Key, obj, InvalidTime, true);
        }

        /// <summary>
        /// 新增内存对象
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="obj"></param>
        /// <param name="InvalidTime">绝对过期时间</param>
        public void SetMemCacheObj(string Key, object obj, DateTime InvalidTime, bool IsZip)
        {
            if (System.Web.HttpRuntime.Cache.Count > maxCacheObjLength)
            {
                return;
            }

            RemoveMemCacheObj(Key);

            TotalCacheObjCount++;

            byte[] objValue = CF.cls_Serialize.Serialize2Byte(obj, IsZip, false);

            System.Web.HttpRuntime.Cache.Insert(Key, objValue, null, InvalidTime, TimeSpan.Zero);
        }

        /// <summary>
        /// 获取内存对象
        /// </summary>
        /// <returns>未找到则返回null</returns>
        public void RemoveMemCacheObj(string Key)
        {
            if (System.Web.HttpRuntime.Cache != null && System.Web.HttpRuntime.Cache[Key] != null)
            {
                System.Web.HttpRuntime.Cache.Remove(Key);
            }
        }

        /// <summary>
        /// 获取内存对象
        /// </summary>
        /// <returns>未找到则返回null</returns>
        public T GetMemCacheObj<T>(string Key)
        {
            return GetMemCacheObj<T>(Key, true);
        }

        /// <summary>
        /// 获取内存对象
        /// </summary>
        /// <returns>未找到则返回null</returns>
        public T GetMemCacheObj<T>(string Key, bool IsZip)
        {
            T obj = default(T);

            if (System.Web.HttpRuntime.Cache[Key] == null)
            {
                return obj;
            }
            else
            {
                TotalHitObjCount++;

                byte[] b = (byte[])System.Web.HttpRuntime.Cache.Get(Key);

                try
                {
                    if (IsZip)
                    {
                        obj = CF.cls_Serialize.Desrialize<T>(b, IsZip, false);
                    }
                    else
                    {
                        MemoryStream outMS = new MemoryStream(b);
                        IFormatter formatter = new BinaryFormatter();
                        obj = (T)formatter.Deserialize(outMS);
                        outMS.Dispose();
                    }
                }
                catch (Exception er)
                {
#if(DEBUG)
                    throw new Exception("反序列化失败,原因:" + er.Message);
#else
#endif
                }

                return obj;
            }
        }

        /// <summary>
        /// 简易监控
        /// </summary>
        /// <returns></returns>
        public string GetMonitorHtml()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div>");

            if (System.Web.HttpRuntime.Cache != null)
            {
                sb.Append("已经缓存对象数目: " + System.Web.HttpRuntime.Cache.Count + "<br/>");
                sb.Append("总缓存对象数目: " + TotalCacheObjCount + "<br/>");
                sb.Append("命中次数: " + TotalHitObjCount + "<br/>");
            }
            sb.Append("</div>");
            return sb.ToString();
        }

        /// <summary>
        /// 删除内存缓存
        /// </summary>
        public void ClearCache()
        {

        }
    }
}
