namespace Impress.Logging
{
    public interface ILogger
    {
        void Fatal(System.Exception exception, string message);
        void Fatal(System.Exception exception, string message, params object[] parameters);
        void Fatal(string message, params object[] parameters);

        void Error(System.Exception exception, string message);
        void Error(System.Exception exception, string message, params object[] parameters);
        void Error(string message, params object[] parameters);

        void Info(System.Exception exception, string message);
        void Info(System.Exception exception, string message, params object[] parameters);
        void Info(string message, params object[] parameters);

        void Warn(System.Exception exception, string message);
        void Warn(System.Exception exception, string message, params object[] parameters);
        void Warn(string message, params object[] parameters);

        void Debug(System.Exception exception, string message);
        void Debug(System.Exception exception, string message, params object[] parameters);
        void Debug(string message, params object[] parameters);
    }
}
