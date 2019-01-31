using System;
using System.IO;
using System.Net;
using System.Text;

namespace mtTools
{
    /// <summary>
    /// HttpWebRequest请求工具类
    /// </summary>
    public static class HttpHelper
    {

        /// <summary>
        /// 获取当前网页的内容
        /// </summary>
        /// <param name="httpUrl">Url地址</param>
        /// <param name="timeOut">请求响应超时时间，单位秒，默认30s</param>
        /// <param name="encoding">编码方式</param>
        /// <param name="userAgent">HTTP User-agent值</param>
        /// <param name="cookies">发送Cookie信息,常用于身份验证</param>
        /// <returns></returns>
        public static string GetUrlString(string httpUrl, int timeOut = 30, Encoding encoding = null, string userAgent = "", CookieCollection cookies = null)
        {
            string result = "";

            try
            {
                encoding = encoding ?? Encoding.UTF8;

                HttpWebRequest request = WebRequest.Create(httpUrl) as HttpWebRequest;
                request.UserAgent = userAgent;
                request.Timeout = timeOut * 1000;
                //发送Cookie信息
                if (cookies != null)
                {
                    request.CookieContainer = new CookieContainer();
                    request.CookieContainer.Add(cookies);
                }
                //获取响应内容
                using (WebResponse wr = request.GetResponse())
                {
                    //在这里对接收到的页面内容进行处理
                    using (StreamReader reader = new StreamReader(wr.GetResponseStream(), encoding))
                    {
                        result = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.WriteFile("HttpHelper.GetUrlString", "【Url地址】" + httpUrl);
            }

            return result;
        }

        /// <summary>
        /// 获取Post提交数据后返回的字符串
        /// </summary>
        /// <param name="httpUrl">Url地址</param>
        /// <param name="dataStr">Post数据</param>
        /// <param name="timeOut">请求响应超时时间，单位秒，默认30s</param>
        /// <param name="encoding">编码方式</param>
        /// <param name="userAgent">HTTP User-agent值</param>
        /// <param name="cookies">发送Cookie信息,常用于身份验证</param>
        /// <returns></returns>
        public static string PostUrlString(string httpUrl, string dataStr, int timeOut = 30, Encoding encoding = null, string userAgent = "", CookieCollection cookies = null)
        {
            string result = "";

            try
            {
                encoding = encoding ?? Encoding.UTF8;

                HttpWebRequest request = WebRequest.Create(httpUrl) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.UserAgent = userAgent;
                request.Timeout = timeOut * 1000;
                //发送Cookie信息
                if (cookies != null)
                {
                    request.CookieContainer = new CookieContainer();
                    request.CookieContainer.Add(cookies);
                }
                //发送提交内容
                byte[] data = encoding.GetBytes(dataStr);
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                //获取响应内容
                using (WebResponse wr = request.GetResponse())
                {
                    //在这里对接收到的页面内容进行处理
                    using (StreamReader reader = new StreamReader(wr.GetResponseStream(), encoding))
                    {
                        result = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.WriteFile("HttpHelper.PostUrlString", "【Url地址】" + httpUrl);
            }

            return result;
        }

        /// <summary>
        /// 下载指定文件的字节流
        /// </summary>
        /// <param name="httpUrl">下载文件的地址</param>
        /// <param name="timeOut">下载文件超时时间，单位秒，默认60s</param>
        /// <param name="userAgent">HTTP User-agent值</param>
        /// <param name="cookies">发送Cookie信息,常用于身份验证</param>
        /// <returns></returns>
        public static Byte[] GetUrlByte(string httpUrl, int timeOut = 60, string userAgent = "", CookieCollection cookies = null)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(httpUrl) as HttpWebRequest;
                request.UserAgent = userAgent;
                request.Timeout = timeOut * 1000;
                //发送Cookie信息
                if (cookies != null)
                {
                    request.CookieContainer = new CookieContainer();
                    request.CookieContainer.Add(cookies);
                }
                //获取响应内容流
                using (WebResponse wr = request.GetResponse())
                {
                    using (Stream reader = wr.GetResponseStream())
                    {
                        //int tempByte;
                        //var tempStream = new MemoryStream();
                        //while ((tempByte = reader.ReadByte()) != -1)
                        //{
                        //    tempStream.WriteByte(((byte)tempByte));
                        //}
                        //return tempStream.ToArray();
                        return reader.ToByte();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.WriteFile("HttpHelper.GetUrlByte", "【Url地址】" + httpUrl);
            }

            return null;
        }

    }
}
