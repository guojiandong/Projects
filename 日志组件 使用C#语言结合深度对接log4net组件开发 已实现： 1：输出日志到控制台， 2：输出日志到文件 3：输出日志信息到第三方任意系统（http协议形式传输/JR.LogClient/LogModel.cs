using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JR.LogClient
{
    public class LogModel
    {
        public string timestamp;
        public string level;
        public string thread;
        public string userName;
        public string message;
        public string domain;
        public string machineName;
        public Dictionary<string, string> properties;
        public LogException exception;
    }

    public class LogException
    {
        public string message;
        public string source;
        public string stackTrace;
    }
}
