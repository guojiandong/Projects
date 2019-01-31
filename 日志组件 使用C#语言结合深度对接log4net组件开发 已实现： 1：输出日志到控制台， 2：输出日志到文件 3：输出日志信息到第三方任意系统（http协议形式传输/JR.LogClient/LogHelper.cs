using log4net;
using log4net.Config;
using System;
using System.IO;
using log4net.Core;

namespace JR.LogClient
{
    public class LogHelper
    {
        public static ILog Logger = null;

        public static  IJRLog Log = null;

        static LogHelper()

        {
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            XmlConfigurator.ConfigureAndWatch(logCfg);

            Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


            Log = JRLogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        }
    }
}
