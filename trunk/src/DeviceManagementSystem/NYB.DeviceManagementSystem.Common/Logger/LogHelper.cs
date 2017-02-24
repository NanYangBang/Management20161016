using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.IO;
using NYB.DeviceManagementSystem.Common.Helper;

namespace NYB.DeviceManagementSystem.Common.Logger
{
    public static class LogHelper
    {
        private static ILog _log;

        static LogHelper()
        {
            //var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log.config");
            //log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(path));
            log4net.Config.XmlConfigurator.Configure();
            _log = LogManager.GetLogger("Logging");
        }

        public static void Info(string message)
        {
            if (_log.IsInfoEnabled)
            {
                _log.Info(message);
            }
        }

        public static void Info<T>(string name, T entity)
        {
            if (_log.IsInfoEnabled)
            {
                var info = JsonHelper.JsonSerializer(entity);

                _log.Info(string.Format("{0}:{1}", name, info));
            }
        }

        public static void Debug(string message, string userName = "System", int functionID = 0)
        {
            if (_log.IsDebugEnabled)
            {
                _log.Debug(message);
            }
        }

        public static void Error(string message, string userName = "System", int functionID = 0)
        {
            if (_log.IsErrorEnabled)
            {
                _log.Error(message);
            }
        }

        public static void Fatal(string message, string userName = "System", int functionID = 0)
        {
            if (_log.IsFatalEnabled)
            {
                _log.Fatal(message);
            }
        }

        public static void Warn(string message, string userName = "System", int functionID = 0)
        {
            if (_log.IsWarnEnabled)
            {
                _log.Warn(message);
            }
        }
    }
}
