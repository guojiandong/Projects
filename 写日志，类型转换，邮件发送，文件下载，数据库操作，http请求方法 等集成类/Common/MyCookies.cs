using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;

namespace Common
{
    /// <summary>
    /// cookie操作
    /// </summary>
    public class MyCookies
    {
        public class CookisModel
        {
            #region Model

            /// <summary>
            /// 
            /// </summary>
            public string Key { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string Value { get; set; }

            #endregion Model
        }
        public static void SetCookies(string name, List<CookisModel> modelList, bool isExpires)
        {
            var cookie = new HttpCookie(name);
            if (isExpires)
            {
                cookie.Expires = DateTime.Now.AddDays(20);
            }
            //for (var i = 0; i < modelList.Count; i++)
            //{
            //    cookie.Values.Set(modelList[i].Key, MyGZip.CompressString(modelList[i].Value));
            //}
            foreach (var t in modelList)
            {
                cookie.Values.Set(t.Key, MyGZip.CompressString(t.Value));
            }
            HttpContext.Current.Response.SetCookie(cookie);
        }

        public static void SetNoGZipCookies(string name, string cartstr)
        {
            var cookie = new HttpCookie(name)
                             {
                                 //Expires = DateTime.Now.AddDays(20),
                                 Value = cartstr
                             };
            HttpContext.Current.Response.SetCookie(cookie);
        }


        public static string GetCookies(string name, string key)
        {
            try
            {
                var cookie = HttpContext.Current.Request.Cookies[name] ?? new HttpCookie(name);
                return MyGZip.DecompressString(cookie[key]);
            }
            catch 
            {
                return "";
            }
        }

        public static string GetNoGZipCookies(string name)
        {
            try
            {
                var cookie = HttpContext.Current.Request.Cookies[name] ?? new HttpCookie(name);
                return cookie.Value;
            }
            catch
            {
                return "";
            }
        }

        public static void ClearCookies(string name)
        {
            var cookie = HttpContext.Current.Request.Cookies[name] ?? new HttpCookie(name);
            cookie.Expires = DateTime.Now.AddHours(-2);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}
