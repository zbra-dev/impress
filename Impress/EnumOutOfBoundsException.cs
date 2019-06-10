using System;

namespace Impress
{
    [Serializable]
    public class EnumOutOfBoundsException : EnumConversionException
    {
        public EnumOutOfBoundsException(string message)
            : base(message) { }
    }
}
