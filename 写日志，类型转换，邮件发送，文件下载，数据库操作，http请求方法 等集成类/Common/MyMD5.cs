using System.Web.Security;

namespace Common
{
    /// <summary>
    /// MD5加密
    /// </summary>
    public class MyMD5
    {
        public static string GetMD5(string myString)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(myString, "MD5");
        }
    }
}
