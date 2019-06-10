using System;

namespace Impress
{
    public static class EnumExtensions
    {
        public static E ToEnum<E>(this object value) where E : struct
        {
            return (E)EnumUtils.DoToEnum(value, typeof(E));
        }

        public static object RawValue<E>(this E element) where E : struct
        {
            return Convert.ChangeType(element, Enum.GetUnderlyingType(typeof(E)));
        }

        public static N RawValue<N, E>(this E element)
            where E : struct
            where N : struct
        {
            return DoRawValue<N, E>(element);
        }

        public static int IntRawValue<E>(this E element)
            where E : struct
        {
            return DoRawValue<int, E>(element);
        }

        private static N DoRawValue<N, E>(E element)
            where E : struct
            where N : struct
        {
            var underlyingType = Enum.GetUnderlyingType(typeof(E));
            if (typeof(N) != underlyingType)
            {
                throw new EnumUnderlyingTypeException("Invalid raw type. Expected " + underlyingType + " got " + typeof(N));
            }
            return (N)Convert.ChangeType(element, typeof(N));
        }
    }
}
