using System.Collections.Generic;
using System.Data;
using System.IO;

namespace mtTools
{
    /// <summary>
    /// 类型转换扩展
    /// </summary>
    public static partial class ConvertHelper
    {

        /// <summary>
        /// Stream转byte[]：可通用于不支持查找的网络流
        /// </summary>
        /// <param name="stream">流内容</param>
        /// <returns></returns>
        public static byte[] ToByte(this Stream stream)
        {
            int tempByte;
            var tempStream = new MemoryStream();
            while ((tempByte = stream.ReadByte()) != -1)
            {
                tempStream.WriteByte(((byte)tempByte));
            }
            return tempStream.ToArray();
        }

        /// <summary>
        /// byte[]转Stream
        /// </summary>
        /// <param name="bytes">字节流</param>
        /// <returns></returns>
        public static Stream ToStream(this byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

    }

    /// <summary>
    /// 类型转换扩展【JavaScriptSerializer版JsonHelper专用】：用于支持DataTable,DataSet的序列化与反序列化
    /// </summary>
    public static partial class ConvertHelper
    {

        /// <summary>
        /// 数据表DataTable转数据列表List【JavaScriptSerializer版JsonHelper专用】（List对应每行数据，Dictionary对应每行的字段与字段值）
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> ToList(this DataTable dt)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    dic.Add(dc.ColumnName, dr[dc.ColumnName]);
                }
                list.Add(dic);
            }
            return list;
        }

        /// <summary>
        /// 数据列表List转数据表DataTable【JavaScriptSerializer版JsonHelper专用】（List对应每行数据，Dictionary对应每行的字段与字段值）
        /// </summary>
        /// <param name="list">数据列表</param>
        /// <returns></returns>
        public static DataTable ToDataTable(this List<Dictionary<string, object>> list)
        {
            DataTable dt = new DataTable();

            if(list.Count > 0 && list[0].Keys.Count > 0)
            {
                //添加列名称
                foreach(var kv in list[0])
                {
                    dt.Columns.Add(kv.Key);
                }

                //添加行数据
                foreach (var item in list)
                {
                    DataRow dr = dt.NewRow();
                    foreach(var kv in item)
                    {
                        dr[kv.Key] = kv.Value;
                    }
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }

        /// <summary>
        /// 数据集DataSet转数据字典Dictionary【JavaScriptSerializer版JsonHelper专用】（每表对应外Dic,List对应表中每行数据，Dictionary对应每行的字段与字段值）
        /// </summary>
        /// <param name="ds">数据集</param>
        /// <returns></returns>
        public static Dictionary<string, List<Dictionary<string, object>>> ToList(this DataSet ds)
        {
            Dictionary<string, List<Dictionary<string, object>>> result = new Dictionary<string, List<Dictionary<string, object>>>();
            foreach (DataTable dt in ds.Tables)
                result.Add(dt.TableName, dt.ToList());
            return result;
        }

        /// <summary>
        /// 数据字典Dictionary转数据集DataSet【JavaScriptSerializer版JsonHelper专用】（每表对应外Dic,List对应表中每行数据，Dictionary对应每行的字段与字段值）
        /// </summary>
        /// <param name="dic">数据字典</param>
        /// <returns></returns>
        public static DataSet ToDataSet(this Dictionary<string, List<Dictionary<string, object>>> dic)
        {
            DataSet ds = new DataSet();
            foreach(var item in dic)
            {
                DataTable dt = item.Value.ToDataTable();
                dt.TableName = item.Key;
                ds.Tables.Add(dt);
            }
            return ds;
        }

    }

}
