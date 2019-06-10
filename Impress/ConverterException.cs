using System;

namespace Impress
{
    [Serializable]
    public class ConverterException : Exception
    {
        public ConverterException(string message, Exception e)
            : base(message, e) { }

        public ConverterException(string message)
            : base(message) { }
    }
}