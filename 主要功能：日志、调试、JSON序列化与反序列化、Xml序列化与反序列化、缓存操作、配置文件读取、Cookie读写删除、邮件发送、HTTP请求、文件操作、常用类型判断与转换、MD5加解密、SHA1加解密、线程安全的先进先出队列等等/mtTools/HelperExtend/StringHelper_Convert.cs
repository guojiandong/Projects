using System;
using System.Text.RegularExpressions;

namespace mtTools
{
    /// <summary>
    /// 常用字符串格式转换
    /// </summary>
    public static partial class StringHelper
    {

        #region 自定义类型转换与类型判断
        
        /// <summary>
        /// 32位收敛字符串或36位发散字符串转Guid
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Guid ToGuid(this string str)
        {
            if (str.Length == 32)
                return new Guid(str.Substring(0, 8) + "-" + str.Substring(8, 4) + "-" + str.Substring(12, 4) + "-" + str.Substring(16, 4) + "-" + str.Substring(20, 12));
            else
                return new Guid(str);
        }

        /// <summary>
        /// 将string转换为bool：支持数字(数字会转为int后ToBoolean)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool ToBool(this string str)
        {
            if (str.IsInt())
                return Convert.ToBoolean(Convert.ToInt32(str));
            else
                return Convert.ToBoolean(str);
        }


        
        /// <summary>
        /// 判断数字是否为null或0
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this int? obj)
        {
            if (obj == null || obj == 0)
                return true;

            return false;
        }

        /// <summary>
        /// 判断字符串是否为纯中文或含有中文
        /// </summary>
        /// <param name="str"></param>
        /// <param name="AllMatch">是否完全匹配：true-纯中文；false-含有中文。</param>
        /// <returns></returns>
        public static bool IsChinese(this string str, bool AllMatch = false)
        {
            Byte[] bytes = System.Text.Encoding.GetEncoding("gb2312").GetBytes(str);
            return AllMatch ? bytes.Length == str.Length * 2 : bytes.Length > str.Length;

            //return AllMatch ? Regex.IsMatch(str, @"^[\u4e00-\u9fa5]+$") : Regex.IsMatch(str, @"[\u4e00-\u9fa5]");
        }

        /// <summary>
        /// 判断字符串是否为Guid，32位与36位皆可：Guid.Empty亦为true
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsGuid(this string str)
        {
            if (str.IsNullOrWhiteSpace())
                return false;

            return str.Length == 32 || str.Length == 36;
        }

        #endregion

        #region 字符串格式转换：系统自带方法改静态扩展

        /// <summary>
        /// 将字符串转换为Int类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt(this string str)
        {
            return Convert.ToInt32(str);
        }

        /// <summary>
        /// 将字符串转换为Long类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long ToLong(this string str)
        {
            return Convert.ToInt64(str);
        }

        /// <summary>
        /// 将字符串转换为decimal类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string str)
        {
            return decimal.Parse(str);
        }

        /// <summary>
        /// 将字符串转换为double类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double ToDouble(this string str)
        {
            return double.Parse(str);
        }

        /// <summary>
        /// 将字符串转换为float类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float ToFloat(this string str)
        {
            return float.Parse(str);
        }

        /// <summary>
        /// 将字符串转换为Byte类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Byte ToByte(this string str)
        {
            return Convert.ToByte(str);
        }

        #endregion

        #region 字符串类型及格式判断：系统自带方法改静态扩展
        
        /// <summary>
        /// 判断字符串是否为null,空
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// 判断字符串是否为null,空,空白字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// 判断字符串是否为DateTime类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDateTime(this string str)
        {
            if (str.IsNullOrWhiteSpace())
                return false;

            DateTime t = new DateTime();
            return DateTime.TryParse(str, out t);
        }
        
        /// <summary>
        /// 判断字符串是否能转换为Bool类型：int类型亦为true
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsBool(this string str)
        {
            if (str.IsNullOrWhiteSpace())
                return false;

            if (str.IsInt())
                return true;

            bool t = false;
            return bool.TryParse(str, out t);
        }
        


        /// <summary>
        /// 判断字符串是否能转换为Int32
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsInt(this string str)
        {
            if (str.IsNullOrWhiteSpace())
                return false;

            int t = 0;
            return int.TryParse(str, out t);
        }

        /// <summary>
        /// 判断字符串是否能转换为Long(Int64)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsLong(this string str)
        {
            if (str.IsNullOrWhiteSpace())
                return false;

            long t = 0;
            return long.TryParse(str, out t);
        }

        /// <summary>
        /// 判断字符串是否能转换为decimal
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDecimal(this string str)
        {
            if (str.IsNullOrWhiteSpace())
                return false;

            decimal t = 0;
            return decimal.TryParse(str, out t);
        }

        /// <summary>
        /// 判断字符串是否能转换为Double
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDouble(this string str)
        {
            if (str.IsNullOrWhiteSpace())
                return false;

            double t = 0;
            return double.TryParse(str, out t);
        }

        /// <summary>
        /// 判断字符串是否能转换为Float
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsFloat(this string str)
        {
            if (str.IsNullOrWhiteSpace())
                return false;

            float t = 0;
            return float.TryParse(str, out t);
        }

        /// <summary>
        /// 判断字符串是否为byte类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsByte(this string str)
        {
            if (str.IsNullOrWhiteSpace())
                return false;

            byte t = new byte();
            return byte.TryParse(str, out t);
        }

        #endregion

    }
}
