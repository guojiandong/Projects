using log4net.Appender;
using log4net.Core;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JR.LogClient
{
    public class NetLogBuildAppender : AppenderSkeleton
    {
        #region public properties
        public string RemoteAddress { get; set; }
        public int QueueSize { get; set; }
        #endregion

        #region Public Instance Constructors
        public NetLogBuildAppender()
            : base()
        {
            eventQueue = new ArrayList();
        }
        #endregion

        #region private fields
        private ArrayList eventQueue;
        #endregion

        #region Override implementation of AppenderSkeleton
        protected override void Append(log4net.Core.LoggingEvent loggingEvent)
        {
            lock (eventQueue.SyncRoot)
            {
                eventQueue.Add(loggingEvent);
                if (eventQueue.Count >= QueueSize)
                {
                    this.Clear();
                }
            }
        }
        #endregion

        #region Public Instance Methods

        virtual public void Clear()
        {
            lock (eventQueue.SyncRoot)
            {
                SaveToServer();
                eventQueue.Clear();
            }
        }

        virtual public LoggingEvent[] GetEvents()
        {
            lock (eventQueue.SyncRoot)
            {
                return (LoggingEvent[])eventQueue.ToArray(typeof(LoggingEvent));
            }
        }

        #endregion Public Instance Methods

        #region private methods

        List<LogModel> logModels = null;
        private void SaveToServer()
        {
            lock (eventQueue.SyncRoot)
            {
                logModels = new List<LogModel>();
                foreach (log4net.Core.LoggingEvent evt in eventQueue)
                {
                    try
                    {
                        LogModel logModel = LogModelBuild.BuildLogModel(evt);
                        logModels.Add(logModel);

                        SaveToServer(logModel);
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.ToString());
                        continue;
                    }
                }

            }
        }
        private void SaveToServer(LogModel logModel)
        { 
            try
            {
                string jsonStr = JsonConvert.SerializeObject(logModel);
                PostStrToServer(jsonStr, RemoteAddress);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                return;
            }
        }

        private void PostStrToServer(string content, string url)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                client.Encoding = System.Text.Encoding.UTF8;
                client.UploadStringAsync(new Uri(url), ("logStr=" + content));
            }
        }

        #endregion
    }
}
