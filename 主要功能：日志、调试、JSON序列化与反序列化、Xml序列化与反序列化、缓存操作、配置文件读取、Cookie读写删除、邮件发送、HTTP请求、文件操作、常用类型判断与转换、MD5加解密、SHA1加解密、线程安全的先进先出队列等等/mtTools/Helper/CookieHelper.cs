using System;
using System.Web;

namespace mtTools
{

    /// <summary>
    /// Cookie域名类型
    /// </summary>
    public enum CookieDomain
    {
        /// <summary>
        /// 当前网站域名【Request.Url.Host】（如www.baidu.com）
        /// </summary>
        CurrentDomain = 1,
        /// <summary>
        /// 当前网站顶级域名【Request.Url.Host最后两部分】(如.baidu.com)
        /// </summary>
        TopDomain = 2
    }

    /// <summary>
    /// 客户端浏览器Cookie操作类
    /// </summary>
    public static class CookieHelper
    {

        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="CookieName">Cookie名称</param>
        /// <param name="CookieValue">Cookie值</param>
        /// <param name="DayExpires">过期天数，默认1天</param>
        /// <param name="Domain">设置Cookie的域名类型</param>
        /// <param name="Path">Cookie路径，默认为"/",可指定路径（如/test/,/test/temp/）</param>
        public static void SetCookie(string CookieName, string CookieValue, double DayExpires = 1, CookieDomain Domain = CookieDomain.CurrentDomain, string Path = "/")
        {
            SetCookie(CookieName, CookieValue, DateTime.Now.Date.AddDays(DayExpires), Domain, Path);
        }

        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="CookieName">Cookie名称</param>
        /// <param name="CookieValue">Cookie值</param>
        /// <param name="DateExpires">过期时间</param>
        /// <param name="Domain">设置Cookie的域名类型</param>
        /// <param name="Path">Cookie路径，默认为"/",可指定路径（如/test/,/test/temp/）</param>
        public static void SetCookie(string CookieName, string CookieValue, DateTime DateExpires, CookieDomain Domain = CookieDomain.CurrentDomain, string Path = "/")
        {
            var cookie = new HttpCookie(CookieName);
            cookie.Value = CookieValue;
            cookie.Expires = DateExpires;

            if (Domain == CookieDomain.CurrentDomain)
            {
                cookie.Domain = HttpContext.Current.Request.Url.Host.ToLower();
            }
            else if (Domain == CookieDomain.TopDomain)
            {
                var host = HttpContext.Current.Request.Url.Host.ToLower();
                var arr = host.Split('.');
                if (arr.Length > 2)
                    host = "." + arr[arr.Length - 2] + "." + arr[arr.Length - 1];
                cookie.Domain = host;
            }

            cookie.Path = Path;

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="CookieName">Cookie名称</param>
        /// <returns>Cookie值</returns>
        public static string GetCookie(string CookieName)
        {
            var cookie = HttpContext.Current.Request.Cookies[CookieName];
            return cookie == null ? "" : cookie.Value;
        }

        /// <summary>
        /// 移除Cookie
        /// </summary>
        /// <param name="CookieName">Cookie名称</param>
        /// <param name="Domain">设置Cookie的域名类型</param>
        /// <param name="Path">Cookie路径，默认为"/",可指定路径（如/test/,/test/temp/）</param>
        public static void RemoveCookie(string CookieName, CookieDomain Domain = CookieDomain.CurrentDomain, string Path = "/")
        {
            SetCookie(CookieName, "", -99, Domain, Path);
        }

    }
}
