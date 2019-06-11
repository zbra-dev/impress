using System;

namespace Impress.Logging
{
    public class LogManagerScanException : Exception
    {
        public LogManagerScanException() { }

        protected LogManagerScanException(string message)
            : base(message)
        { }
    }
}
