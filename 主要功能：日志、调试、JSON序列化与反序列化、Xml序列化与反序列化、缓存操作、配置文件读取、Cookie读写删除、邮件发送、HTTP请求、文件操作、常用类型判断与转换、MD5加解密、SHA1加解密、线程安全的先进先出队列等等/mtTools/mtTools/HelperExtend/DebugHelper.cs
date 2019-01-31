using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using mtTools.Models;

namespace mtTools
{
    /// <summary>
    /// 调试输出至文件或调试面板
    /// </summary>
    public static class DebugHelper
    {

        /// <summary>
        /// 获取自定义toolDebug配置
        /// </summary>
        public static CustomNodeSection DebugConfig = ConfigHelper.GetCustomNodeSetting("toolDebug");


        /// <summary>
        /// 是否输出调试信息
        /// </summary>
        private static bool? isDebug = null;

        /// <summary>
        /// 是否输出调试信息
        /// </summary>
        public static bool IsDebug
        {
            get
            {
                if (isDebug != null)
                    return isDebug.Value;

                isDebug = DebugConfig["IsDebug"] == null ? true : (DebugConfig["IsDebug"] == "true" ? true : false);
                return isDebug.Value;
            }
        }


        /// <summary>
        /// 调试信息文件路径及文件前缀缓存变量
        /// </summary>
        private static string debugPath = null;

        /// <summary>
        /// 调试信息文件路径及文件前缀
        /// </summary>
        public static string DebugPath
        {
            get
            {
                if (debugPath != null)
                    return debugPath;

                if (string.IsNullOrEmpty(DebugConfig["DebugPath"]))
                    debugPath = System.AppDomain.CurrentDomain.BaseDirectory + "Logs\\Debug\\";
                else if (DebugConfig["DebugPath"].IndexOf(":") == -1)
                    debugPath = System.AppDomain.CurrentDomain.BaseDirectory + DebugConfig["DebugPath"];
                else
                    debugPath = DebugConfig["DebugPath"];

                return debugPath;
            }
        }


        /// <summary>
        /// 调试信息输出格式
        /// </summary>
        private static string exDebugMessage = DebugConfig["DebugMessage"] ?? "【@Now】【@CodeLocal】@Message";

        /// <summary>
        /// 日志锁：防止同时多次执行造成丢失数据
        /// </summary>
        private static readonly object lockObj = new object();


        /// <summary>
        /// 输出调试至调试面板
        /// </summary>
        /// <param name="ex">调试信息</param>
        /// <param name="CodeLocal">调试位置</param>
        /// <param name="OtherInfo">其它附加日志信息</param>
        public static void WriteLine(this Exception ex, string CodeLocal = "", string OtherInfo = "")
        {
            WriteLine(ex.Message, CodeLocal, OtherInfo);
        }

        /// <summary>
        /// 输出调试信息至调试面板
        /// </summary>
        /// <param name="Message">调试信息</param>
        /// <param name="CodeLocal">调试位置</param>
        /// <param name="OtherInfo">其它附加日志信息</param>
        public static void WriteLine(this string Message, string CodeLocal = "", string OtherInfo = "")
        {
            if (IsDebug)
            {
                Debug.WriteLine(exDebugMessage.Replace("@Now", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")).Replace("@CodeLocal", CodeLocal).Replace("@Message", Message + OtherInfo));
            }
        }

        /// <summary>
        /// 输出调试信息至文件
        /// </summary>
        /// <param name="ex">调试信息</param>
        /// <param name="CodeLocal">调试位置</param>
        /// <param name="OtherInfo">其它附加日志信息</param>
        public static void WriteFile(this Exception ex, string CodeLocal = "", string OtherInfo = "")
        {
            WriteFile(ex.Message, CodeLocal, OtherInfo);
        }

        /// <summary>
        /// 输出调试信息至文件
        /// </summary>
        /// <param name="Message">调试信息</param>
        /// <param name="CodeLocal">调试位置</param>
        /// <param name="OtherInfo">其它附加日志信息</param>
        public static void WriteFile(this string Message, string CodeLocal = "", string OtherInfo = "")
        {
            if (IsDebug)
            {
                Message.WriteLine(CodeLocal, OtherInfo);

                Message = exDebugMessage.Replace("@Now", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")).Replace("@CodeLocal", CodeLocal).Replace("@Message", Message + OtherInfo);
                Task t = new Task(() =>
                {
                    TaskWriteLine(Message);
                });
                t.Start();
            }
        }


        /// <summary>
        /// 写入一行调试信息
        /// </summary>
        private static void TaskWriteLine(string Message)
        {
            try
            {
                lock (lockObj)
                {
                    string filePath = DebugPath + DateTime.Now.ToString("yyyyMMdd") + ".txt";

                    string direct = System.IO.Path.GetDirectoryName(filePath);
                    if (!Directory.Exists(direct))
                        Directory.CreateDirectory(direct);
                    if (!File.Exists(filePath))
                        File.Create(filePath).Dispose();

                    using (var sw = File.AppendText(filePath))
                    {
                        sw.WriteLine(Message);
                    }
                }
            }
            catch { }
        }

    }
}
