using System;
using System.Text;
using System.IO;
using System.Net;
using CF;

namespace Common
{
    /// <summary>
    ///HttpMethods 的摘要说明
    /// </summary>
    public class MyHttpMethods
    {

        #region POST


        /// <summary>
        /// HTTP POST方式请求数据
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="param">POST的数据</param>
        /// <returns></returns>
        public static string HttpPost(string url, string param)
        {
            var request = (HttpWebRequest) WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 30000;
            request.AllowAutoRedirect = false;
            try
            {
                string responseStr;
                using (var requestStream = new StreamWriter(request.GetRequestStream()))
                {
                    requestStream.Write(param);
                }
                using (var response = request.GetResponse())
                {
                    var grStream = response.GetResponseStream();
                    if (grStream == null) return null;
                    var reader = new StreamReader(grStream, Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
                return responseStr;
            }
            catch (Exception ex)
            {
                cls_Common.writeLog(
                  System.Web.Hosting.HostingEnvironment.MapPath("/log/" + DateTime.Now.ToString("MMdd") + "/MyHttpMethodPostErro.txt"),
                   DateTime.Now.ToString("HH:mm:ss") + "\t\t" + url + "\t\t" + ex.Message);
                return null;
            }
        }

        #endregion


        
        /// <summary>
        /// HTTP POST方式请求数据
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="param">POST的数据</param>
        /// <returns></returns>
        public static string HttpPost(string url, string param,Encoding encoding)
        {
            var request = (HttpWebRequest) WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;
            try
            {
                string responseStr;
                using (var requestStream = new StreamWriter(request.GetRequestStream()))
                {
                    requestStream.Write(param);
                }
                using (var response = request.GetResponse())
                {
                    var grStream = response.GetResponseStream();
                    if (grStream == null) return null;
                    var reader = new StreamReader(grStream,encoding);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
                return responseStr;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

  

        #region Get

        /// <summary>
        /// HTTP GET方式请求数据.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <returns></returns>
        public static string HttpGet(string url)
        {
            var request = (HttpWebRequest) WebRequest.Create(url);
            request.Method = "GET";
            //request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 50000;
            request.AllowAutoRedirect = false;
            var responseStr = "";
            using (var response = request.GetResponse())
            {
                var stream = response.GetResponseStream();
                if (stream == null) return responseStr;
                var reader = new StreamReader(stream, Encoding.UTF8);
                responseStr = reader.ReadToEnd();
            }

            return responseStr;
        }


        /// <summary>
        /// HTTP GET方式请求数据.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="enc">Encoding.UTF8 Encoding.Gb2312</param>
        /// <returns></returns>
        public static string HttpGet(string url, Encoding enc)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            //request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;
            var responseStr = "";
            using (var response = request.GetResponse())
            {
                var stream = response.GetResponseStream();
                if (stream == null) return responseStr;
                var reader = new StreamReader(stream, enc);
                responseStr = reader.ReadToEnd();
            }

            return responseStr;
        }

        #endregion

    }
}
