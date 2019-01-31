using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace mtTools
{
    /// <summary>
    /// Xml序列化与反序列化
    /// </summary>
    public static class XmlHelper
    {

        /// <summary>
        /// 序列化Xml对象为字符串
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="Source">要序列化的实体类</param>
        /// <param name="encoding">xml编码格式</param>
        /// <param name="OmitXmlDeclaration">true:忽略xml头信息</param>
        /// <param name="removeXmlns">true,不生成xmlns</param>
        /// <param name="IsZHDateString">是否格式化日期为yyyy-MM-dd HH:mm:ss格式【Xml中日期存在空格则反序列化时会报错需将时间格式化回去，此方法中反序列化字符串时已实现，反序列化Stream未加此处理】</param>
        /// <returns></returns>
        public static string GetXmlString<T>(this T Source, Encoding encoding = null, bool OmitXmlDeclaration = false, bool removeXmlns = true, bool IsZHDateString = false) where T : class
        {
            string XmlStr = "";
            if (encoding == null)
                encoding = Encoding.UTF8;

            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializer xml = new XmlSerializer(typeof(T));

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.NewLineChars = "\r\n";
                settings.Encoding = encoding;
                settings.IndentChars = "    ";
                settings.OmitXmlDeclaration = OmitXmlDeclaration;//true:忽略 <?xml version='1.0' encoding='utf-8'?>
                using (XmlWriter writer = XmlWriter.Create(stream, settings))
                {
                    if (removeXmlns)//true,不生成xmlns
                    {
                        XmlSerializerNamespaces _namespaces = new XmlSerializerNamespaces(new XmlQualifiedName[] { new XmlQualifiedName(string.Empty, string.Empty) });
                        xml.Serialize(writer, Source, _namespaces);
                    }
                    else
                    {
                        xml.Serialize(writer, Source);
                    }
                    writer.Close();
                }

                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream, encoding))
                {
                    XmlStr = reader.ReadToEnd();
                }
            }

            //Xml中日期存在空格则反序列化时会报错需将时间格式化回去
            if (IsZHDateString)
            {
                XmlStr = Regex.Replace(XmlStr, @"(\d{4}-\d{1,2}-\d{1,2}T\d{1,2}:\d{1,2}:\d{1,2}\.\d+\+08:00)", match =>
                {
                    return match.Groups[1].Value.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                });
            }

            return XmlStr;
        }

        /// <summary>
        /// 序列化Xml对象为字符串(带GBK格式头信息)
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="Source">要序列化的实体类</param>
        /// <param name="encoding">xml编码格式（默认为GBK）</param>
        /// <param name="removeXmlns">true,不生成xmlns</param>
        /// <param name="IsZHDateString">是否格式化日期为yyyy-MM-dd HH:mm:ss格式【Xml中日期存在空格则反序列化时会报错需将时间格式化回去，此方法中反序列化字符串时已实现，反序列化Stream未加此处理】</param>
        /// <returns></returns>
        public static string GetGBKXmlString<T>(this T Source, Encoding encoding = null, bool removeXmlns = true, bool IsZHDateString = false) where T : class
        {
            if (encoding == null)
                encoding = Encoding.GetEncoding("GBK");

            string XmlStr = GetXmlString<T>(Source, encoding, true, removeXmlns, IsZHDateString);

            XmlStr = "<?xml version=\"1.0\" encoding=\"GBK\"?>" + Environment.NewLine + XmlStr;

            return XmlStr;
        }



        /// <summary>
        /// 将字符串反序列化为Xml对象：实现yyyy-MM-dd HH:mm:ss格式判断处理
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="XmlStr">Xml字符串</param>
        /// <returns></returns>
        public static T GetXmlObj<T>(this string XmlStr) where T : class
        {
            T obj = null;

            //Xml中日期存在空格则反序列化时会报错需将时间格式化回去
            XmlStr = Regex.Replace(XmlStr, @"(\d{4}-\d{1,2}-\d{1,2}\s{1}\d{1,2}:\d{1,2}:\d{1,2})", match =>
            {
                return match.Groups[1].Value.ToDateTime().ToString("s");
            });

            using (StringReader sr = new StringReader(XmlStr))
            {
                XmlSerializer xmldes = new XmlSerializer(typeof(T));
                obj = (T)xmldes.Deserialize(sr);
            }

            return obj;
        }

        /// <summary>
        /// 将字符串反序列化为Xml对象：未加yyyy-MM-dd HH:mm:ss格式判断处理
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="XmlStream">Xml字节流</param>
        /// <returns></returns>
        public static T GetXmlObj<T>(this Stream XmlStream) where T : class
        {
            T obj = null;

            XmlSerializer xmldes = new XmlSerializer(typeof(T));
            obj = (T)xmldes.Deserialize(XmlStream);

            return obj;
        }

    }
}
