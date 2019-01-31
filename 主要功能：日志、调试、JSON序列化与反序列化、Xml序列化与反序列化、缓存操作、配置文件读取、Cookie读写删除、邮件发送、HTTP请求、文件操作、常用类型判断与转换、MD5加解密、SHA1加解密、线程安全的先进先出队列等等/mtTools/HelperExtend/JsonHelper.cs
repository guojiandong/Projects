using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Data;
using System.Text.RegularExpressions;

namespace mtTools
{

    /// <summary>
    /// Json序列化与反序列化
    /// </summary>
    public static class JsonHelper
    {

        /// <summary>
        /// JSON反序列化为动态类
        /// </summary>
        /// <param name="JSONString">要反序列化的此对象字符串</param>
        /// <returns>反序列化生成的动态对象</returns>
        public static dynamic JSONDeserializeDynamic(this string JSONString)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.DeserializeObject(JSONString);
        }

        /// <summary>
        /// JSON反序列化
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="JSONString">要反序列化的此对象字符串</param>
        /// <returns>反序列化生成的此对象</returns>
        public static T JSONDeserialize<T>(this string JSONString) where T : class
        {
            T result = null;

            try
            {
                if (!string.IsNullOrEmpty(JSONString))
                {
                    JavaScriptSerializer jss = new JavaScriptSerializer();

                    if (typeof(T).Name == "DataTable")
                        result = jss.Deserialize<List<Dictionary<string, object>>>(JSONString).ToDataTable() as T;
                    else if (typeof(T).Name == "DataSet")
                        result = jss.Deserialize<Dictionary<string, List<Dictionary<string, object>>>>(JSONString).ToDataSet() as T;
                    else
                        result = jss.Deserialize<T>(JSONString);
                }
            }
            catch (Exception ex)
            {
                ex.WriteFile("JsonHelper.JSONDeserialize");
            }

            return result;
        }



        /// <summary>
        /// JSON序列化：null为""
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="Object">要序列化的对象</param>
        /// <param name="IsZHDateString">是否格式化日期为yyyy-MM-dd HH:mm:ss格式</param>
        /// <returns>序列化此对象生成的字符串</returns>
        public static string JSONSerialize<T>(this T Object, bool IsZHDateString = true) where T : class
        {
            string JSONString = "";

            try
            {
                if (Object != null)
                {
                    JavaScriptSerializer jss = new JavaScriptSerializer();

                    if (typeof(T).Name == "DataTable")
                        JSONString = jss.Serialize((Object as DataTable).ToList());
                    else if (typeof(T).Name == "DataSet")
                        JSONString = jss.Serialize((Object as DataSet).ToList());
                    else
                        JSONString = jss.Serialize(Object);

                    if (IsZHDateString)
                    {
                        JSONString = Regex.Replace(JSONString, @"\\/Date\((\d+)\)\\/", match =>
                        {
                            return (new DateTime(1970, 1, 1)).AddMilliseconds(long.Parse(match.Groups[1].Value)).ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ex.WriteFile("JsonHelper.JSONSerialize");
            }

            return JSONString;
        }

    }

}
