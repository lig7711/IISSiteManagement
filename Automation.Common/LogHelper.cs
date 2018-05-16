using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Automation.Common
{

    /// <summary>
    /// Log helper
    /// </summary>
    public class LogHelper
    {
        private static bool _isInitalized = false;
        private static log4net.ILog _log = log4net.LogManager.GetLogger("log");
        private static Queue<LogMessage> _queue = new Queue<LogMessage>();

        /// <summary>
        /// 初始化。
        /// 加载默认的配置信息。
        /// </summary>
        public static void Init()
        {
            log4net.Config.XmlConfigurator.Configure();
            _isInitalized = true;

            Task task = Task.Factory.StartNew(LogTask);
        }

        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="config">配置信息</param>
        public static void Init(FileInfo config)
        {
            log4net.Config.XmlConfigurator.Configure(config);
            _isInitalized = true;
        }

        private static void LogTask()
        {
            while (true)
            {
                if (_queue.Count > 0)
                {
                    try
                    {
                        var log = _queue.Dequeue();
                        if (log != null)
                        {
                            switch (log.Level)
                            {
                                case LogLevel.Debug:
                                    LogDebug(log.Message);
                                    break;
                                case LogLevel.Info:
                                    LogInfo(log.Message);
                                    break;
                                case LogLevel.Warn:
                                    LogWarn(log.Message);
                                    break;
                                case LogLevel.Error:
                                    LogError(log.Message);
                                    break;
                                case LogLevel.Fatal:
                                    LogFatal(log.Message);
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else
                {
                    Thread.Sleep(3000);
                }
            }
        }

        #region Log
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="level">日志级别</param>
        /// <param name="message">日志内容</param>
        public static void Log(LogLevel level, string message)
        {
            if (!_isInitalized)
                throw new Exception("Log4net is not initalized.");

            _queue.Enqueue(new LogMessage(level, message));
        }

        private static void LogDebug(string message)
        {
            if (_log.IsDebugEnabled)
                _log.Debug(message);
        }

        private static void LogInfo(string message)
        {
            if (_log.IsInfoEnabled)
                _log.Info(message);
        }

        private static void LogWarn(string message)
        {
            if (_log.IsWarnEnabled)
                _log.Warn(message);
        }

        private static void LogError(string message)
        {
            if (_log.IsErrorEnabled)
                _log.Error(message);
        }

        private static void LogFatal(string message)
        {
            if (_log.IsFatalEnabled)
                _log.Fatal(message);
        }
        #endregion

        /// <summary>
        /// 日志级别
        /// </summary>
        public enum LogLevel
        {
            /// <summary>
            /// 调试
            /// </summary>
            Debug,
            /// <summary>
            /// 信息
            /// </summary>
            Info,
            /// <summary>
            /// 警告
            /// </summary>
            Warn,
            /// <summary>
            /// 一般错误
            /// </summary>
            Error,
            /// <summary>
            /// 严重异常
            /// </summary>
            Fatal
        }

        internal class LogMessage
        {
            public LogMessage(LogLevel level, string message)
            {
                this.Level = level;
                this.Message = message;
            }

            public LogLevel Level { get; set; }
            public string Message { get; set; }
        }
    }

}
