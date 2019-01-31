using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace mtTools.Models
{
    /// <summary>
    /// 日志对象并操作
    /// </summary>
    public class LogHelperLog
    {

        /// <summary>
        /// 目录名或多级目录名，目录不存在则自动创建
        /// </summary>
        public string LogName { get; set; }

        /// <summary>
        /// 日志文件名：为空则以日期命名；否则则为指定的文件名【多级目录请并于此处】(不需.txt后缀)
        /// </summary>
        public string FileName { get; set; }


        /// <summary>
        /// 实例化当前类
        /// </summary>
        /// <param name="logName"></param>
        /// <param name="fileName"></param>
        public LogHelperLog(string logName, string fileName)
        {
            this.LogName = logName;
            this.FileName = fileName;
        }


        /// <summary>
        /// 缓存的待写入日志队列
        /// </summary>
        private Fifo<string> fifo = new Fifo<string>();

        /// <summary>
        /// 读写日志锁：防止同时多次执行造成丢失数据
        /// </summary>
        private readonly object lockObj = new object();


        /// <summary>
        /// 日志文件路径
        /// </summary>
        private string FilePath
        {
            get
            {
                string att = "";
                if (FileName == "")
                    att = DateTime.Now.ToString("yyyyMMdd") + ".txt";
                else
                    att = FileName + ".txt";

                return LogHelper.appPath + LogName + "\\" + att;
            }
        }


        /// <summary>
        /// 获取日志内容：FileName为空时为当天日志，FileName存在时为所有日志
        /// </summary>
        /// <returns></returns>
        public string GetAllLog()
        {
            lock (lockObj)
            {
                string text = "";
                try { text = File.ReadAllText(FilePath, Encoding.UTF8); }
                catch { }
                return text;
            }
        }

        /// <summary>
        /// 写入一行日志（异步写入）
        /// </summary>
        /// <param name="Message">日志内容</param>
        public void WriteLine(string Message)
        {
            fifo.Append(LogHelper.exMessage.Replace("@Now", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")).Replace("@Message", Message));

            Task t = new Task(() =>
            {
                TaskWriteLine();
            });
            t.Start();
        }


        /// <summary>
        /// 写入一行日志
        /// </summary>
        private void TaskWriteLine()
        {
            try
            {
                lock (lockObj)
                {
                    var Message = fifo.Pop();

                    string direct = System.IO.Path.GetDirectoryName(FilePath);
                    if (!Directory.Exists(direct))
                        Directory.CreateDirectory(direct);
                    if (!File.Exists(FilePath))
                        File.Create(FilePath).Dispose();

                    using (var sw = File.AppendText(FilePath))
                    {
                        sw.WriteLine(Message);
                    }
                }
            }
            catch { }
        }

    }
}
