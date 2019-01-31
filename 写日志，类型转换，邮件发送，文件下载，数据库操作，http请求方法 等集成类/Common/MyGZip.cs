using System;
using System.IO;
using System.Text;

namespace Common
{
    /// <summary>
    /// 加密 解密
    /// </summary>
    public class MyGZip
    {
        /// <summary>
        /// 压缩string
        /// </summary>
        public static string CompressString(string inString)
        {
            var b = Encoding.Default.GetBytes(inString);
            //转成 Base64 形式的 System.String
            return Convert.ToBase64String(CompressByte(b));
        }

        /// <summary>
        /// 解压string
        /// </summary>
        public static string DecompressString(string inString)
        {
            //转回到原来的 System.String。
            var b = Convert.FromBase64String(inString);
            return Encoding.Default.GetString(DecompressByte(b));
        }

        /// <summary>
        /// 压缩byte数组
        /// </summary>
        /// <param name="inBytes">需要压缩的byte数组</param>
        /// <returns>压缩好的byte数组</returns>
        private static byte[] CompressByte(byte[] inBytes)
        {
            var outStream = new MemoryStream();
            Stream zipStream = new GZipOutputStream(outStream);
            zipStream.Write(inBytes, 0, inBytes.Length);
            zipStream.Close();
            var outData = outStream.ToArray();
            outStream.Close();
            return outData;
        }

        /// <summary>
        /// 解压缩byte数组
        /// </summary>
        /// <param name="inBytes">需要解压缩的byte数组</param>
        /// <returns></returns>
        private static byte[] DecompressByte(byte[] inBytes)
        {
            var writeData = new byte[2048];
            var inStream = new MemoryStream(inBytes);
            var zipStream = new GZipInputStream(inStream) as Stream;
            var outStream = new MemoryStream();
            while (true)
            {
                var size = zipStream.Read(writeData, 0, writeData.Length);
                if (size > 0)
                {
                    outStream.Write(writeData, 0, size);
                }
                else
                {
                    break;
                }
            }
            var outData = outStream.ToArray();
            outStream.Close();
            return outData;
        }



        /// <summary>
        /// Base64 FTF8加密
        /// </summary>
        public static string EncodeBase64(string inString)
        {
            var b = Encoding.UTF8.GetBytes(inString);
            //转成 Base64 形式的 System.String
            return Convert.ToBase64String(b);
        }


        /// <summary>
        /// Base64 根据传入的编码格式进行加密
        /// </summary>
        /// <param name="encoding">编码格式</param>
        /// <param name="inString"></param>
        /// <returns></returns>
        public static string EncodeBase64(Encoding encoding, string inString)
        {
            var b = encoding.GetBytes(inString);
            //转成 Base64 形式的 System.String
            return Convert.ToBase64String(b);
        }

        /// <summary>
        /// Base64 FTF8解密
        /// </summary>
        public static string DecodeBase64(string inString)
        {
            //转回到原来的 System.String。
            var b = Convert.FromBase64String(inString);
            return Encoding.UTF8.GetString(b);
        }


        /// <summary>
        /// Base64 根据传入的编码格式进行解密
        /// </summary>
        public static string DecodeBase64(Encoding encoding, string inString)
        {
            //转回到原来的 System.String。
            var b = Convert.FromBase64String(inString);
            return encoding.GetString(b);
        }



    }
}
