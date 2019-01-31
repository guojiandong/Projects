using System;
using System.Text;
using System.Security.Cryptography;

namespace mtTools
{
    /// <summary>
    /// 常用加解密工具类
    /// </summary>
    public static partial class EncodeHelper
    {

        #region MD5加密方法

        /// <summary>
        /// MD5加密算法
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns>MD5值</returns>
        public static string MD5(this string str)
        {
            return MD5(str, Encoding.UTF8);
        }

        /// <summary>
        /// MD5加密算法
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>MD5值</returns>
        public static string MD5(this string str, Encoding encoding)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(encoding.GetBytes(str));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }

        #endregion

        #region SHA1加密方法

        /// <summary>
        /// SHA1加密算法
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns>SHA1值</returns>
        public static string SHA1(this string str)
        {
            return SHA1(str, Encoding.UTF8);
        }

        /// <summary>
        /// SHA1加密算法
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>SHA1值</returns>
        public static string SHA1(this string str, Encoding encoding)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] result = sha1.ComputeHash(encoding.GetBytes(str));
            return BitConverter.ToString(result).Replace("-", ""); //encoding.GetString(result);
        }

        #endregion

    }
}
