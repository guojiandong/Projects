using log4net;
using log4net.Core;
using System;

namespace JR.LogClient
{
    public class JRLogImpl : LogImpl, IJRLog, ILog, ILoggerWrapper
    {
        // Fields
        private static readonly Type ThisDeclaringType = typeof(JRLogImpl);

        public JRLogImpl(ILogger logger) : base(logger)
        {
        }

        public void Error(object message, object userID)
        {
            this.innerError(message,  userID, string.Empty, null);
        }

        public void Error(object message,  object userID, string parameter)
        {
            this.innerError(message, userID, parameter, null);
        }

        public void Error(object message,  object userID, string parameter, Exception exception)
        {
            this.innerError(message,  userID, parameter, exception);
        }


        public void Info(object message,  object userID)
        {
            this.innerInfo(message, userID, string.Empty, null);
        }

        public void Info(object message, object userID, string parameter)
        {
            this.innerInfo(message,  userID, parameter, null);
        }

        public void Info(object message, object userID, string parameter, Exception exception)
        {
            this.innerInfo(message,  userID, parameter, exception);
        }


        private void innerError(object message,  object userID, string parameter, Exception exception)
        {
            if (this.IsErrorEnabled)
            {
                LoggingEvent logEvent = new LoggingEvent(ThisDeclaringType, this.Logger.Repository, this.Logger.Name, Level.Error, message, exception);
                logEvent.Properties["userID"] = (userID != null) ? userID.ToString() : string.Empty;
                logEvent.Properties["parameter"] = parameter;
                this.Logger.Log(logEvent);
            }
        }

        private void innerInfo(object message,  object userID, string parameter, Exception exception)
        {
            if (this.IsInfoEnabled)
            {
                LoggingEvent logEvent = new LoggingEvent(ThisDeclaringType, this.Logger.Repository, this.Logger.Name, Level.Info, message, exception);
                logEvent.Properties["userID"] = userID;
                logEvent.Properties["parameter"] = parameter;
                this.Logger.Log(logEvent);
            }
        }
    }
}
