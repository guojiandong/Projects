using log4net;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JR.LogClient
{
    public interface IJRLog : ILog, ILoggerWrapper
    {
        // Methods
        void Error(object message, object userID);
        void Error(object message, object userID, string parameter);
        void Error(object message, object userID, string parameter, Exception exception);
        void Info(object message, object userID);
        void Info(object message, object userID, string parameter);
        void Info(object message, object userID, string parameter, Exception exception);
    }
}
