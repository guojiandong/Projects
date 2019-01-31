using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace CF
{
    /// <summary>
    /// 本地数据库缓存类,用于临时存储中间状态的数据
    /// </summary>
    public class cls_Cache_DB
    {
        /// <summary>
        /// PGSql连接串
        /// </summary>
        public static readonly string ConnStr_PGSql_Cache = "server=" + System.Configuration.ConfigurationManager.AppSettings["pgsql_ip"]
            + ";uid=postgres;pwd=cherry27;database=postgres";

        private static Queue Sql_Update = new Queue();

        /// <summary>
        /// 从本地PGSql缓存获取一个对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetCache<T>(string key, string TableName)
        {
            T obj = default(T);

            string strSql = "select * from " + TableName + " where key=@p0";
            object[] args = new object[] { key };
            DataTable dt = CF.cls_PGSql.LoadDataTable(
                ConnStr_PGSql_Cache,
                strSql,
                args);

            if (dt.Rows.Count > 0)
            {
                if (DateTime.Parse(dt.Rows[0]["expiretime"].ToString()) < DateTime.Now)
                {
                    return obj;
                }

                string str = dt.Rows[0]["value"].ToString();

                try
                {
                    return CF.cls_Serialize.Desrialize<T>(str);
                }
                catch (Exception er)
                {
#if(DEBUG)
                    //throw new Exception("反序列化失败,原因:" + er.Message);
#else
#endif
                }
            }

            return default(T);
        }

        /// <summary>
        /// 从本地PGSql缓存删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static void RemoveCacheDB(string key, string TableName)
        {
            string strSql = "delete from " + TableName + " where key=@p0";

            object[] args = new object[] { key };

            CF.cls_PGSql.ExeSqlCommand(ConnStr_PGSql_Cache, strSql, args);
        }

        /// <summary>
        /// 清理本地缓存PGSql库
        /// </summary>
        public static void ClearAllDBCache()
        {
            try
            {
                CF.cls_PGSql.ExeSqlCommand(ConnStr_PGSql_Cache, "delete from cachetable", null);
            }
            catch
            {
            }
        }

        public static void InitCacheTable(string TableName)
        {
            string strSql = "select * from pg_class where relname =@p0;";
            object[] args = new object[] { TableName };
            DataTable dt = CF.cls_PGSql.LoadDataTable(ConnStr_PGSql_Cache, strSql, args);
            if (dt.Rows.Count == 0)
            {
                strSql = "CREATE UNLOGGED TABLE " + TableName + "(  key character varying(250) NOT NULL,  value text,  expiretime timestamp with time zone NOT NULL DEFAULT now(),"
                   + "  CONSTRAINT \"PK_" + TableName + "\" PRIMARY KEY (key) )WITH (OIDS=FALSE);ALTER TABLE " + TableName + "  OWNER TO postgres;";
                CF.cls_PGSql.ExeSqlCommand(ConnStr_PGSql_Cache, strSql, null);
            }
        }

        /// <summary>
        /// 删除过期的缓冲值
        /// </summary>
        public static void RemoveExpireCache(string TableName)
        {
            string strSql = "delete from " + TableName + " where expiretime < @p0";
            object[] args = new object[] { DateTime.Now, DateTime.Now };
            CF.cls_PGSql.ExeSqlCommand(ConnStr_PGSql_Cache, strSql, args);
        }

        /// <summary>
        /// 保存本地PGSql缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiretime"></param>
        /// <param name="invalidtime"></param>
        /// <returns></returns>
        public static int SetCache(string key, object value, DateTime expiretime, string TableName)
        {
            string objValue = CF.cls_Serialize.Serialize(value);
            if (objValue == "")
            {
                return -1;
            }

            string strSql = "update " + TableName + " set value=@p0,expiretime=@p1 where key=@p2";
            object[] args = new object[] { objValue, expiretime, key };
            int IReturn = CF.cls_PGSql.ExeSqlCommand(ConnStr_PGSql_Cache, strSql, args);

            if (IReturn < 1)
            {
                strSql = "insert into " + TableName + " (key, value,expiretime)values(@p0,@p1,@p2)";
                args = new object[] { key, objValue, expiretime };
                IReturn = CF.cls_PGSql.ExeSqlCommand(ConnStr_PGSql_Cache, strSql, args);
            }

            return IReturn;
        }

        /// <summary>
        /// 查看总共缓存对象
        /// </summary>
        /// <returns></returns>
        public static int GetCacheCount()
        {
            return 0;

            string strSql = "select count(*) from cachetable";
            return CF.cls_PGSql.GetInt(ConnStr_PGSql_Cache, strSql, null);
        }

        /// <summary>
        /// 查看本站缓存对象
        /// </summary>
        /// <param name="strPart"></param>
        /// <returns></returns>
        public static int GetCacheCount(string strPart)
        {
            return 0;
            string strSql = "select count(*) from cachetable where key like '" + strPart + "%'";
            return CF.cls_PGSql.GetInt(ConnStr_PGSql_Cache, strSql, null);
        }

        /// <summary>
        /// 缓存对象表名
        /// </summary>
        public static class CommonCacheTableName
        {
            //---------------通用的缓存

            /// <summary>
            /// 通用缓存
            /// </summary>
            public static readonly string CacheTable = "cachetable";

            public static readonly string CacheHeartRecord = "CacheHeartRecord";
        }
    }
}
