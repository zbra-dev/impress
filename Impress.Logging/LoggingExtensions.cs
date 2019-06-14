using System;

namespace Impress.Logging
{
    public static class LoggingExtensions
    {
        public static ILogger Logger(this Type type)
        {
            return LogManagerRegistry.GetInstance().RetriveLoggerByType(type);
        }

        public static ILogger Logger<T>()
        {
            return LogManagerRegistry.GetInstance().RetriveLoggerByType(typeof(T));
        }

        public static ILogger Logger(this object it)
        {
            if (it == null)
            {
                return NullLogger.nullLogger;
            }
            return LogManagerRegistry.GetInstance().RetriveLoggerByType(it.GetType());
        }

        public static ILogger LoggerNamed(this object it, string name)
        {
            return LogManagerRegistry.GetInstance().RetriveLoggerByName(name);
        }

    }


    internal class NullLogger : ILogger
    {
        internal static readonly NullLogger nullLogger = new NullLogger();

        public void Error(System.Exception exception, string message)
        {
            //no-op
        }


        public void Error(System.Exception exception, string message, params object[] parameters)
        {
            //no-op
        }

        public void Error(string message, params object[] parameters)
        {
            //no-op
        }

        public void Info(System.Exception exception, string message)
        {
            //no-op
        }

        public void Info(System.Exception exception, string message, params object[] parameters)
        {
            //no-op
        }

        public void Info(string message, params object[] parameters)
        {
            //no-op
        }

        public void Warn(System.Exception exception, string message)
        {
            //no-op
        }

        public void Warn(System.Exception exception, string message, params object[] parameters)
        {
            //no-op
        }

        public void Warn(string message, params object[] parameters)
        {
            //no-op
        }

        public void Debug(System.Exception exception, string message)
        {
            //no-op
        }

        public void Debug(System.Exception exception, string message, params object[] parameters)
        {
            //no-op
        }

        public void Debug(string message, params object[] parameters)
        {
            //no-op
        }

        public void Fatal(System.Exception exception, string message)
        {
            //no-op
        }

        public void Fatal(System.Exception exception, string message, params object[] parameters)
        {
            //no-op
        }

        public void Fatal(string message, params object[] parameters)
        {
            //no-op
        }
    }
}
