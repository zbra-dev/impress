using System;

namespace Impress.Logging
{
    public class ConsoleLogManager : ILogManager
    {
        private static readonly ConsoleLogger logger = new ConsoleLogger();

        public ILogger RetriveLoggerByName(string name)
        {
            return logger;
        }

        public ILogger RetriveLoggerByType(Type type)
        {
            return logger;
        }

    }

    internal enum ConsoleLevels
    {
        Debug,
        Warn,
        Info,
        Error,
        Fatal
    }

    internal class ConsoleLogger : ILogger
    {
        private static object[] NoParameters = new object[0];

        public void Debug(string message, params object[] parameters)
        {
            Log(ConsoleLevels.Debug, message, parameters);
        }

        private void Log(ConsoleLevels level, string message, object[] parameters, Exception ex = null)
        {
            var msg = parameters.Length == 0 ? message :  string.Format(message, parameters);

            Console.WriteLine($"[{level.ToString().ToUpperInvariant()}] {msg}");

            if (ex != null)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void Debug(Exception exception, string message)
        {
            Log(ConsoleLevels.Debug, message, NoParameters);
        }

        public void Debug(Exception exception, string message, params object[] parameters)
        {
            Log(ConsoleLevels.Debug, message, NoParameters, exception);
        }

        public void Error(string message, params object[] parameters)
        {
            Log(ConsoleLevels.Error, message, parameters);
        }

        public void Error(Exception exception, string message)
        {
            Log(ConsoleLevels.Error, message, NoParameters, exception);
        }

        public void Error(Exception exception, string message, params object[] parameters)
        {
            Log(ConsoleLevels.Error, message, NoParameters, exception);
        }

        public void Fatal(string message, params object[] parameters)
        {
            Log(ConsoleLevels.Fatal, message, NoParameters);
        }

        public void Fatal(Exception exception, string message)
        {
            Log(ConsoleLevels.Fatal, message, NoParameters, exception);
        }

        public void Fatal(Exception exception, string message, params object[] parameters)
        {
            Log(ConsoleLevels.Fatal, message, parameters, exception);
        }

        public void Info(string message, params object[] parameters)
        {
            Log(ConsoleLevels.Info, message, parameters);
        }

        public void Info(Exception exception, string message)
        {
            Log(ConsoleLevels.Info, message, NoParameters, exception);
        }

        public void Info(Exception exception, string message, params object[] parameters)
        {
            Log(ConsoleLevels.Info, message, parameters, exception);
        }

        public void Warn(string message, params object[] parameters)
        {
            Log(ConsoleLevels.Warn, message, parameters);
        }

        public void Warn(Exception exception, string message)
        {
            Log(ConsoleLevels.Warn, message, NoParameters, exception);
        }

        public void Warn(Exception exception, string message, params object[] parameters)
        {
            Log(ConsoleLevels.Warn, message, parameters, exception);
        }
    }
}
