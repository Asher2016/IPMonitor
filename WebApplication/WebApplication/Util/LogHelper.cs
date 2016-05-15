using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Util
{
    public class LogHelper
    {
        private static LogHelper logHelper = new LogHelper();

        public const string WebSite = "WebSite";

        public static LogHelper Instance
        {
            get
            {
                return logHelper;
            }
        }

        private LogHelper()
        {

        }

        public void Info(string logName, string message)
        {
            LogManager.GetLogger(logName).Info(message);
        }

        public void Debug(string logName, string message)
        {
            LogManager.GetLogger(logName).Debug(message);
        }

        public void Error(string logName, string message)
        {
            LogManager.GetLogger(logName).Error(message);
        }
    }
}