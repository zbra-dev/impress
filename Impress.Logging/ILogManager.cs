using System;

namespace Impress.Logging
{
    public interface ILogManager
    {
        ILogger RetriveLoggerByName(string name);
        ILogger RetriveLoggerByType(Type type);
    }
}
