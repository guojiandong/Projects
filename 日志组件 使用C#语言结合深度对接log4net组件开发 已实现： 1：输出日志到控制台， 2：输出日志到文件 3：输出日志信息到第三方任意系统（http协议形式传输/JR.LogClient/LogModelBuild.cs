using log4net.Core;
using log4net.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JR.LogClient
{
    public class LogModelBuild
    {
        public static LogModel BuildLogModel(LoggingEvent loggingEvent)
        {
            if (loggingEvent == null)
            {
                return null;
            }
            LogModel logModel = new LogModel()
            {
                timestamp = loggingEvent.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss"),
                level = loggingEvent.Level.ToString(),
                thread = loggingEvent.ThreadName,
                userName = loggingEvent.UserName,
                message = loggingEvent.RenderedMessage,
                domain = loggingEvent.Domain,
                machineName = Environment.MachineName
            };

            if (loggingEvent.ExceptionObject != null)
            {
                logModel = BuildExceptionLogModel(logModel, loggingEvent.ExceptionObject);
            }
            PropertiesDictionary properties = loggingEvent.GetProperties();
            if ((properties != null) && (properties.Count > 0))
            {
                logModel.properties = new Dictionary<string, string>();
                foreach (DictionaryEntry entry in (IEnumerable)properties)
                {
                    string name = entry.Key.ToString();
                    logModel.properties.Add(name, entry.Value.ToString());
                }
            }
            return logModel;
        }


        private static LogModel BuildExceptionLogModel(LogModel logModel, Exception ex)
        {
            logModel.exception = new LogException()
            {
                message = ex.Message,
                source = ex.Source,
                stackTrace = ex.StackTrace
            };
            return logModel;
        }
    }
}
