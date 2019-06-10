using System;
using System.Runtime.Serialization;

namespace Impress
{
    [Serializable]
    public class InvalidTypeConverterException : ConverterException
    {
        public InvalidTypeConverterException(string message)
            : base(message) { }
    }
}