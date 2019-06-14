using System;

namespace Impress.Logging
{
    public interface ILogManager
    {
        ILogger RetrieveLoggerByName(string name);
        ILogger RetrieveLoggerByType(Type type);
    }
}
