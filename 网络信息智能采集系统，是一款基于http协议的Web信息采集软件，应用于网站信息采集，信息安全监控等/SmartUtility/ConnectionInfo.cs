
namespace Smart.Utility
{
    using System;
    using System.Text;
    using System.Configuration;
    using Smart.Security;

    /// <summary>
    /// ���ݿ������ַ������ܽ���
    /// </summary>
    public class ConnectionInfo
    {
        public static string connstring = string.Empty;

        /// <summary>
        /// ���ݿ������ַ���
        /// </summary>
        public static string CONN_STRING
        {
            get
            {
                if (string.Empty == connstring)
                {
                    try
                    {
                        connstring = ConfigurationManager.AppSettings["ConnString"];
                        connstring = ConnectionInfo.DecryptDBConnectionString(connstring);
                    } 
                    catch(Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                }
                return connstring;
            }
        }

        /// <summary>
        /// �����ַ���
        /// </summary>
        /// <param name="InputConnectionString">���ܵ������ַ���</param>
        /// <returns>string</returns>
        public static string DecryptDBConnectionString(string InputConnectionString)
        {
            return Encrypter.Decrypt(InputConnectionString);
        }

        /// <summary>
        /// �������ݿ������ַ���
        /// </summary>
        /// <param name="encryptedString">�����ַ���</param>
        /// <returns></returns>
        public static string EncryptDBConnectionString(string encryptedString)
        {
            return Encrypter.Encrypt(encryptedString);
        }
    }
}
