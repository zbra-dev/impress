using System;
using System.Runtime.Serialization;

namespace Impress
{
    [Serializable]
    public class InvalidFormatConverterException : ConverterException
    {
        public InvalidFormatConverterException(string message, Exception e)
            : base(message, e) { }
    }
}