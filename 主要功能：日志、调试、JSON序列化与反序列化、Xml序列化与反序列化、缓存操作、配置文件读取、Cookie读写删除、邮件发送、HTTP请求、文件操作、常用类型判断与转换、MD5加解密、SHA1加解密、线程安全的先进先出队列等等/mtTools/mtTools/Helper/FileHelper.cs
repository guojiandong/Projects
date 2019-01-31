using System;
using System.IO;
using System.Text;

namespace mtTools
{

    /// <summary>
    /// 文件写入类型
    /// </summary>
    public enum TextWriteType
    {
        /// <summary>
        /// 追加
        /// </summary>
        Append =1,
        /// <summary>
        /// 覆盖
        /// </summary>
        Covered = 2
    }

    /// <summary>
    /// 文本文件操作类
    /// </summary>
    public class FileHelper
    {

        /// <summary>
        /// 读取文本文件内容
        /// </summary>
        /// <param name="FilePath">文件路径</param>
        /// <returns></returns>
        public static string ReadText(string FilePath)
        {
            return ReadText(FilePath, Encoding.UTF8);
        }

        /// <summary>
        /// 读取文本文件内容
        /// </summary>
        /// <param name="FilePath">文件路径</param>
        /// <param name="Enc">编码方式</param>
        /// <returns></returns>
        public static string ReadText(string FilePath, Encoding Enc)
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    return File.ReadAllText(FilePath, Enc);
                }
            }
            catch (Exception ex)
            {
                ex.WriteFile("FileHelper.ReadText", "读取文件路径：" + FilePath);
            }

            return "";
        }

        /// <summary>
        /// 逐行读取大文本文件内容
        /// </summary>
        /// <param name="FilePath">文件路径</param>
        /// <param name="LineContentCallBack">行内容回调方法</param>
        public static void ReadBigText(string FilePath, Action<string> LineContentCallBack)
        {
            ReadBigText(FilePath, LineContentCallBack, Encoding.UTF8);
        }

        /// <summary>
        /// 逐行读取大文本文件内容
        /// </summary>
        /// <param name="FilePath">文件路径</param>
        /// <param name="LineContentCallBack">行内容回调方法</param>
        /// <param name="Enc">编码方式</param>
        public static void ReadBigText(string FilePath, Action<string> LineContentCallBack, Encoding Enc)
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    //读取日志并分捡请求
                    var fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
                    var sr = new StreamReader(fs, Enc);
                    sr.BaseStream.Seek(0, SeekOrigin.Begin); //从开头开始

                    string item = sr.ReadLine();
                    while (item != null)
                    {
                        LineContentCallBack(item);
                        item = sr.ReadLine(); //继续读取下一行
                    }

                    sr.Close();
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                ex.WriteFile("FileHelper.ReadBigText", "读取文件路径：" + FilePath);
            }
        }



        /// <summary>
        /// 写入文本文件内容
        /// </summary>
        /// <param name="FilePath">文件路径</param>
        /// <param name="Content">文件内容</param>
        /// <param name="WriteType">写入方式：1-覆盖;2-追加</param>
        public static void WriteText(string FilePath, string Content, TextWriteType WriteType)
        {
            WriteText(FilePath, Content, WriteType, Encoding.UTF8);
        }

        /// <summary>
        /// 写入文本文件内容
        /// </summary>
        /// <param name="FilePath">文件路径</param>
        /// <param name="Content">文件内容</param>
        /// <param name="WriteType">写入方式：1-覆盖;2-追加</param>
        /// <param name="Enc">编码方式</param>
        public static void WriteText(string FilePath, string Content, TextWriteType WriteType, Encoding Enc)
        {
            try
            {
                string direct = System.IO.Path.GetDirectoryName(FilePath);
                if (!Directory.Exists(direct))
                {
                    Directory.CreateDirectory(direct);
                }

                if (WriteType == TextWriteType.Append)
                    File.AppendAllText(FilePath, Content, Enc);
                else if (WriteType == TextWriteType.Covered)
                    File.WriteAllText(FilePath, Content, Enc);
            }
            catch (Exception ex)
            {
                ex.WriteFile("FileHelper.WriteText", "写入文件路径：" + FilePath);
            }
        }



        /// <summary>
        /// 读取字节流文件内容：读取失败返回null
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static byte[] ReadFile(string FilePath)
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    return File.ReadAllBytes(FilePath);
                }
            }
            catch (Exception ex)
            {
                ex.WriteFile("FileHelper.ReadFile", "读取文件路径：" + FilePath);
            }

            return null;
        }



        /// <summary>
        /// 写入字节流文件内容
        /// </summary>
        /// <param name="FilePath">文件路径</param>
        /// <param name="Content">文件内容</param>
        public static void WriteFile(string FilePath, byte[] Content)
        {
            try
            {
                string direct = System.IO.Path.GetDirectoryName(FilePath);
                if (!Directory.Exists(direct))
                {
                    Directory.CreateDirectory(direct);
                }

                File.WriteAllBytes(FilePath, Content);
            }
            catch (Exception ex)
            {
                ex.WriteFile("FileHelper.WriteFile", "写入文件路径：" + FilePath);
            }
        }

    }
}
