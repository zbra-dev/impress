using System;

namespace Impress
{
    [Serializable]
    public class EnumUnderlyingTypeException : EnumConversionException
    {
        public EnumUnderlyingTypeException(string message)
            : base(message) { }
    }
}
