using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.IO;

namespace NYB.DeviceManagementSystem.Common.Logger
{
    public static class LogHelper
    {
        private static ILog _log;

        static LogHelper()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log.config");
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(path));
            _log = LogManager.GetLogger("Logging");
        }

        public static void Info(string message, string userName = "System", int functionID = 0)
        {
            if (_log.IsInfoEnabled)
            {
                _log.Info(new LogMessage(userName, functionID, message));
            }
        }

        public static void Debug(string message, string userName = "System", int functionID = 0)
        {
            if (_log.IsDebugEnabled)
            {
                _log.Debug(new LogMessage(userName, functionID, message));
            }
        }

        public static void Error(string message, string userName = "System", int functionID = 0)
        {
            if (_log.IsErrorEnabled)
            {
                _log.Error(new LogMessage(userName, functionID, message));
            }
        }

        public static void Fatal(string message, string userName = "System", int functionID = 0)
        {
            if (_log.IsFatalEnabled)
            {
                _log.Fatal(new LogMessage(userName, functionID, message));
            }
        }

        public static void Warn(string message, string userName = "System", int functionID = 0)
        {
            if (_log.IsWarnEnabled)
            {
                _log.Warn(new LogMessage(userName, functionID, message));
            }
        }
    }
}
