using System;
using System.Collections.Generic;

namespace mtTools
{
    /// <summary>
    /// 文本操作扩展
    /// </summary>
    public static partial class StringHelper
    {

        #region 其它扩展方法

        /// <summary>
        /// 返回字符串汉字长度
        /// </summary>
        /// <param name="str">半角标点及字母数字占半个长度，不足则补齐1</param>
        /// <returns></returns>
        public static int LengthChinese(this string str)
        {
            Byte[] bytes = System.Text.Encoding.GetEncoding("gb2312").GetBytes(str);
            return Convert.ToInt32(Math.Ceiling(bytes.Length / 2.0));
        }

        /// <summary>
        /// 生成随机数
        /// </summary>
        /// <param name="Max"></param>
        /// <returns></returns>
        public static string GetRandomString(this int Max)
        {
            Random rnd = new Random();
            Int32 i = rnd.Next(Max);
            return i.ToString();
        }

        /// <summary>
        /// 将字符串添加至空List集合中形成字符串列表类型：字符串为null则返回null
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<string> ToList(this string str)
        {
            if (str == null)
                return null;

            List<string> list = new List<string>();
            list.Add(str);
            return list;
        }

        #endregion

        #region 字符串处理

        /// <summary>
        /// 从指定字符串开始截取后面所有字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="StartStr">从此字符串后开始截取</param>
        /// <param name="IsFirst">true:前第一个匹配项开始截取;false:最后一个匹配项开始截取</param>
        /// <returns></returns>
        public static string Substring(this string str, string StartStr, bool IsFirst = true)
        {
            int index = str.IndexOf(StartStr);
            if (index != -1)
            {
                if (IsFirst)
                    return str.Substring(index + StartStr.Length);
                else
                    return str.Substring(str.LastIndexOf(StartStr) + StartStr.Length);
            }
            else
                return "";
        }

        /// <summary>
        /// 从指定字符串之后截取指定长度字符串，指定字符串后长度不足则可选全部截取或返回""
        /// </summary>
        /// <param name="str"></param>
        /// <param name="IndexOfStr">从此字符串后开始截取</param>
        /// <param name="length">要截取的长度</param>
        /// <param name="IsSubAll">如果截取的长度不足，默认返回""，为true则截取后面全部字符</param>
        /// <returns></returns>
        public static string Substring(this string str, string IndexOfStr, int length, bool IsSubAll = false)
        {
            int index = str.IndexOf(IndexOfStr);
            if (index != -1)
            {
                var strIndex = index + IndexOfStr.Length;
                if (str.Length - strIndex - length >= 0)
                    return str.Substring(strIndex, length);
                else if (IsSubAll)
                    return str.Substring(strIndex);
                else
                    return "";
            }
            else
                return "";
        }

        /// <summary>
        /// 从指定位置开始截取指定中文长数的字符串（字母和数字算半个长度）
        /// </summary>
        /// <param name="str"></param>
        /// <param name="StrLength">截取中文长度</param>
        /// <param name="Suffix">截取后字符串后缀，默认为"…"，不算在截取长度中</param>
        /// <returns></returns>
        public static string SubstringChinese(this string str, int StrLength, string Suffix = "…")
        {
            if (str.IsNullOrWhiteSpace() || StrLength <= 0)
                return "";

            if (str.LengthChinese() <= StrLength)
                return str;

            int bytesLength = StrLength * 2; //剩余需要截取的字符编码长度
            int currStrLenght = 0; //当前已确认需截取的字符长度
            while (bytesLength > 0 && currStrLenght < str.Length)
            {
                int len = Convert.ToInt32(Math.Ceiling(bytesLength / 2.0));

                bytesLength -= System.Text.Encoding.GetEncoding("gb2312").GetBytes(str.Substring(currStrLenght, len)).Length;
                currStrLenght += len;
            }
            if (bytesLength < 0) //防止只剩余1个编码长度而截取到汉字造成多出1个编码长度
                currStrLenght -= 1;

            string temp = str.Substring(0, currStrLenght);
            return temp != str ? temp + Suffix : temp;
        }

        /// <summary>
        /// 从指定位置开始截取指定最长中文长数的字符串（字母和数字算半个长度）
        /// </summary>
        /// <param name="str"></param>
        /// <param name="StartIndex">起始位置，0开始</param>
        /// <param name="StrLength">截取长度</param>
        /// <param name="Suffix">截取后字符串后缀，默认为"…"，不算在截取长度中</param>
        /// <returns></returns>
        public static string SubstringChinese(this string str, int StartIndex, int StrLength, string Suffix = "…")
        {
            if (str.IsNullOrWhiteSpace() || StrLength <= 0 || StartIndex < 0)
                return "";

            if (str.Length <= StartIndex)
                return "";
            else if (StrLength + StartIndex >= str.Length)
                return str.Substring(StartIndex);

            return SubstringChinese(str.Substring(StartIndex), StrLength, Suffix);
        }

        /// <summary>
        /// 获取字符串分隔的key value集合
        /// </summary>
        /// <param name="str">要取值的字符串</param>
        /// <param name="strSplitChar">字符串分隔符，默认为半角分号</param>
        /// <param name="valueSplitChar">name与value的分隔符，默认为半角冒号</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetSplitValue(this string str, char strSplitChar = ';', char valueSplitChar = ':')
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            var arr = str.Split(strSplitChar);
            foreach (var item in arr)
            {
                var value = item.Split(valueSplitChar);
                if (value[0] != "" && !dic.ContainsKey(value[0]))
                {
                    //防止其值中有分隔符，以第一个分隔符其后皆为其值
                    dic.Add(value[0], value.Length <= 1 ? "" : item.Substring(item.IndexOf(valueSplitChar) + 1));
                }
            }

            return dic;
        }

        #endregion

        #region 时间格式化与Unix时间戳互转格式化

        /// <summary>
        /// 将字符串转换为DateTime类型
        /// </summary>
        /// <param name="str">字符串或Unix时间戳</param>
        /// <param name="IsUnix">是否为Unix时间戳，默认为false</param>
        /// <param name="IsParse">是否在字符串后面加0000000后再转换，默认为true</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string str, bool IsUnix = false, bool IsParse = true)
        {
            if (!IsUnix)
                return Convert.ToDateTime(str);
            else
                return Convert.ToInt64(str).ToDateTime(IsParse);
        }

        /// <summary>
        /// 将时间戳转换为时间格式
        /// </summary>
        /// <param name="timeUnix">时间戳</param>
        /// <param name="IsParse">是否在字符串后面加0000000后再转换，默认为true</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this long timeUnix, bool IsParse = true)
        {
            if (IsParse)
                timeUnix = long.Parse(timeUnix.ToString() + "0000000");

            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan toNow = new TimeSpan(timeUnix);
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// 转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">要转换的时间</param>
        /// <param name="IsParse">是否带有未尾的0000000，默认为false</param>
        /// <returns></returns>
        public static long ToLong(this DateTime time, bool IsParse = false)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            var ts = (long)(time - startTime).TotalSeconds;
            if (IsParse)
                return long.Parse(ts.ToString() + "0000000");
            else
                return ts;
        }

        #endregion

    }
}
