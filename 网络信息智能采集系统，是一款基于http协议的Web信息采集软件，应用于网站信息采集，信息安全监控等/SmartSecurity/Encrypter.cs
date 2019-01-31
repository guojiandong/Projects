
namespace Smart.Security
{
    using System;
    using System.Text;
    using System.Globalization;
    using System.Security.Cryptography;
    using System.IO;

	/// <summary>
	/// ��ȫ�ࣨ�ַ����ӡ����ܣ�
	/// </summary>
	public class Encrypter 
	{ 
		/// <summary> 
		/// ʹ��ȱʡ��Կ�ַ������� 
		/// </summary> 
		/// <param name="original">����</param> 
		/// <returns>����</returns> 
		public static string Encrypt(string original) 
		{ 
			string key = "SMARTCAM"; // ��Կ��ֻ��Ϊ8λ

			DESCryptoServiceProvider  des  =  new  DESCryptoServiceProvider();  
			//���ַ����ŵ�byte������  
			byte[]  inputByteArray  =  Encoding.Default.GetBytes(original);  
			
			//�������ܶ������Կ��ƫ����  
			//ԭ��ʹ��ASCIIEncoding.ASCII������GetBytes����  
			//ʹ�����������������Ӣ���ı�  
			des.Key  =  ASCIIEncoding.ASCII.GetBytes(key);  
			des.IV  =  ASCIIEncoding.ASCII.GetBytes(key);  
			MemoryStream  ms  =  new  MemoryStream();  
			CryptoStream  cs  =  new  CryptoStream(ms,  des.CreateEncryptor(),CryptoStreamMode.Write); 			
			cs.Write(inputByteArray,  0,  inputByteArray.Length);  
			cs.FlushFinalBlock(); 			
			StringBuilder  ret  =  new  StringBuilder();  
			foreach(byte  b  in  ms.ToArray())  
			{  				
				ret.AppendFormat("{0:X2}",  b);  
			}  
			ret.ToString();  
			return  ret.ToString(); 
		} 

		/// <summary> 
		/// ʹ��ȱʡ��Կ���� 
		/// </summary> 
		/// <param name="original">����</param> 
		/// <returns>����</returns> 
		public static string Decrypt(string original) 
		{ 
			// --�·�����ʼ
			string key = "SMARTCAM"; // ��Կ��ֻ��Ϊ8λ

			DESCryptoServiceProvider  des  =  new  DESCryptoServiceProvider();  
 
			byte[]  inputByteArray  =  new  byte[original.Length  /  2];  
			for(int  x  =  0;  x  <  original.Length  /  2;  x++)  
			{  
				int  i  =  (Convert.ToInt32(original.Substring(x  *  2,  2),  16));  
				inputByteArray[x]  =  (byte)i;  
			}  

			//�������ܶ������Կ��ƫ��������ֵ��Ҫ�������޸�  
			des.Key  =  ASCIIEncoding.ASCII.GetBytes(key);  
			des.IV  =  ASCIIEncoding.ASCII.GetBytes(key);  
			MemoryStream  ms  =  new  MemoryStream();  
			CryptoStream  cs  =  new  CryptoStream(ms,  des.CreateDecryptor(),CryptoStreamMode.Write);  
			
			cs.Write(inputByteArray,  0,  inputByteArray.Length);  
			cs.FlushFinalBlock();  

			//����StringBuild����CreateDecryptʹ�õ��������󣬱���ѽ��ܺ���ı����������  
			StringBuilder  ret  =  new  StringBuilder();  
             
			return  System.Text.Encoding.Default.GetString(ms.ToArray());
		} 

		/// <summary> 
		/// ʹ�ø�����Կ���� 
		/// </summary> 
		/// <param name="original">����</param> 
		/// <param name="key">��Կ</param> 
		/// <returns>����</returns> 
		public static string Decrypt(string original, string key) 
		{ 
			return Decrypt(original,key,System.Text.Encoding.Default); 
		} 

		/// <summary> 
		/// ʹ��ȱʡ��Կ����,����ָ�����뷽ʽ���� 
		/// </summary> 
		/// <param name="original">����</param> 
		/// <param name="encoding">���뷽ʽ</param> 
		/// <returns>����</returns> 
		public static string Decrypt(string original,Encoding encoding) 
		{ 
			return Decrypt(original,"SMARTCAM",encoding); 
		} 

		/// <summary> 
		/// ʹ�ø�����Կ���� 
		/// </summary> 
		/// <param name="original">ԭʼ����</param> 
		/// <param name="key">��Կ</param> 
		/// <returns>����</returns> 
		public static string Encrypt(string original, string key) 
		{ 
			byte[] buff = System.Text.Encoding.Default.GetBytes(original); 
			byte[] kb = System.Text.Encoding.Default.GetBytes(key); 
			return Convert.ToBase64String(Encrypt(buff,kb)); 
		} 

		/// <summary> 
		/// ʹ�ø�����Կ���� 
		/// </summary> 
		/// <param name="encrypted">����</param> 
		/// <param name="key">��Կ</param> 
		/// <param name="encoding">�ַ����뷽��</param> 
		/// <returns>����</returns> 
		public static string Decrypt(string encrypted, string key,Encoding encoding) 
		{ 
			byte[] buff = Convert.FromBase64String(encrypted); 
			byte[] kb = System.Text.Encoding.Default.GetBytes(key); 
			return encoding.GetString(Decrypt(buff,kb)); 
		} 

		/// <summary> 
		/// ����MD5ժҪ 
		/// </summary> 
		/// <param name="original">����Դ</param> 
		/// <returns>ժҪ</returns> 
		public static byte[] MakeMD5(byte[] original) 
		{ 
			MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider(); 
			byte[] keyhash = hashmd5.ComputeHash(original); 
			hashmd5 = null; 
			return keyhash; 
		} 

		/// <summary> 
		/// ʹ�ø�����Կ���� 
		/// </summary> 
		/// <param name="original">����</param> 
		/// <param name="key">��Կ</param> 
		/// <returns>����</returns> 
		public static byte[] Encrypt(byte[] original, byte[] key) 
		{ 
			TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider(); 
			des.Key = MakeMD5(key); 
			des.Mode = CipherMode.ECB; 

			return des.CreateEncryptor().TransformFinalBlock(original, 0, original.Length); 
		} 

		/// <summary> 
		/// ʹ�ø�����Կ�������� 
		/// </summary> 
		/// <param name="encrypted">����</param> 
		/// <param name="key">��Կ</param> 
		/// <returns>����</returns> 
		public static byte[] Decrypt(byte[] encrypted, byte[] key) 
		{ 
			TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider(); 
			des.Key = MakeMD5(key); 
			des.Mode = CipherMode.ECB; 

			return des.CreateDecryptor().TransformFinalBlock(encrypted, 0, encrypted.Length); 
		} 

		/// <summary> 
		/// ʹ��ȱʡ��Կ���� 
		/// </summary> 
		/// <param name="original">ԭʼ����</param> 
		/// <returns>����</returns> 
		public static byte[] Encrypt(byte[] original) 
		{ 
			byte[] key = System.Text.Encoding.Default.GetBytes("SMARTCAM"); 
			return Encrypt(original,key); 
		} 

		/// <summary> 
		/// ʹ��ȱʡ��Կ�������� 
		/// </summary> 
		/// <param name="encrypted">����</param> 
		/// <returns>����</returns> 
		public static byte[] Decrypt(byte[] encrypted) 
		{ 
			byte[] key = System.Text.Encoding.Default.GetBytes("SMARTCAM"); 
			return Decrypt(encrypted,key); 
		} 
	} 
}