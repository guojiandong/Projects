using System.Collections.Generic;
using System.Linq;
using mtTools.Models;

namespace mtTools
{
    /// <summary>
    /// 日志处理类
    /// </summary>
    public class LogHelper
    {

        /// <summary>
        /// 禁用构造方法
        /// </summary>
        private LogHelper() { }


        /// <summary>
        /// 获取huanrongLog配置
        /// </summary>
        public static CustomNodeSection LogConfig = ConfigHelper.GetCustomNodeSetting("toolLog");


        /// <summary>
        /// 日志文件目录缓存变量
        /// </summary>
        private static string filePath = null;

        /// <summary>
        /// 日志文件目录
        /// </summary>
        public static string appPath
        {
            get
            {
                if (filePath != null)
                    return filePath;

                if (string.IsNullOrEmpty(LogConfig["LogPath"]))
                    filePath = System.AppDomain.CurrentDomain.BaseDirectory + "Logs\\";
                else if (LogConfig["LogPath"].IndexOf(":") == -1)
                    filePath = System.AppDomain.CurrentDomain.BaseDirectory + LogConfig["LogPath"];
                else
                    filePath = LogConfig["LogPath"];

                return filePath;
            }
        }


        /// <summary>
        /// 日志输出格式
        /// </summary>
        public static string exMessage = LogConfig["LogMessage"] ?? "【@Now】@Message";

        /// <summary>
        /// 新增实例化日志对象锁
        /// </summary>
        private static readonly object lockObj = new object();
        /// <summary>
        /// 单例模式
        /// </summary>
        private static readonly LogHelper lh = new LogHelper();


        /// <summary>
        /// 快捷访问
        /// </summary>
        public static LogHelper Logs { get { return lh; } }

        /// <summary>
        /// 已实例化日志对象列表
        /// </summary>
        private static readonly List<LogHelperLog> list = new List<LogHelperLog>();


        /// <summary>
        /// 获取日志对象
        /// </summary>
        /// <param name="logName">目录名或文件名，目录不存在则自动创建</param>
        /// <param name="fileName">日志文件名：为空则以日期命名；否则则为指定的文件名【多级目录请并于此处】(不需.txt后缀)</param>
        /// <returns></returns>
        public LogHelperLog this[string logName, string fileName = ""]
        {
            get
            {
                var log = fileName == "" ? list.FirstOrDefault(p => p.LogName == logName) : list.FirstOrDefault(p => p.LogName == logName && p.FileName == fileName);
                if (log == null)
                {
                    lock (lockObj)
                    {
                        log = fileName == "" ? list.FirstOrDefault(p => p.LogName == logName) : list.FirstOrDefault(p => p.LogName == logName && p.FileName == fileName);
                        if (log == null)
                        {
                            log = new LogHelperLog(logName, fileName);
                            list.Add(log);
                        }
                    }
                }
                return log;
            }
        }

    }
}
