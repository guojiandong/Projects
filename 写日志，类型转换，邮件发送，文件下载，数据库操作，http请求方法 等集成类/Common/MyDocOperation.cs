using System.IO;
using System.Text;

namespace Common
{
    public class MyDocOperation
    {
        /// <summary>
        /// 读取文件内容
        /// </summary>
        ///<param name="url">读取文件路径</param>
        public static string Read(string url)
        {
            string input;
            using (var sr = new StreamReader(url, Encoding.Default))
            {
                input = sr.ReadToEnd();
            }
            return input;
        }

        /// <summary>
        /// 写入文件内容
        /// </summary>
        ///<param name="url">写入文件路径</param>
        ///<param name="append">是否追加 true是底部追加，false是覆盖所有</param>
        ///<param name="content">内容</param>
        public static void Write(string url, bool append, string content)
        {
            try
            {
                using (var sw = new StreamWriter(url, append, Encoding.Default))
                {
                    sw.WriteLine(content);
                }
            }
            catch 
            { 

            }
        }
    }
}
