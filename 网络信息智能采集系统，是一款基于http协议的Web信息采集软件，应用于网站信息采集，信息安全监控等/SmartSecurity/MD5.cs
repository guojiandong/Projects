
namespace Smart.Security
{
    using System;
    using System.Security.Cryptography;

	/// <summary>
    /// MD5������
	/// </summary>
	public class MD5
	{
		/// <summary>		
        /// ��һ���ַ���ʹ��MD5�㷨���ܣ������ؼ��ܺ���ַ���
		/// </summary>
		/// <param name="clearText">ԭʼ�����ַ���</param>
		/// <param name="encryptedText">����MD5�㷨���ܺ�������ַ���</param>
		public static void MD5Encrypt(string clearText, ref string encryptedText)
		{
            encryptedText = "";
			try
			{
                if (clearText == null)
                {
                    clearText = "";
                }
				MD5CryptoServiceProvider o_MD5 = new MD5CryptoServiceProvider();
				byte[] bytesEncrypt = System.Text.Encoding.Default.GetBytes(clearText);
				byte[] bytesEncrypted = o_MD5.ComputeHash(bytesEncrypt, 0, bytesEncrypt.Length);				
				for (int i = 0; i < bytesEncrypted.Length; i++)
				{
                    encryptedText += bytesEncrypted[i].ToString("x").PadLeft(2, '0');
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
        /// ��һ���ַ���ʹ��MD5�㷨���ܣ������ؼ��ܺ���ַ���
		/// </summary>
        /// <param name="clearText">ԭʼ�����ַ���</param>
        /// <returns>����MD5�㷨���ܺ�������ַ���</returns>
		public static string MD5Encrypt(string clearText)
		{
            string encryptedText = "";
            MD5Encrypt(clearText, ref encryptedText);
            return encryptedText;
		}
	}
}
