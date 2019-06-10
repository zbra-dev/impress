using System;

namespace Impress
{
    public static class EnumUtils
    {
        public static object ToEnum(object value, Type type)
        {
            return DoToEnum(value, type);
        }

        internal static object DoToEnum(object value, Type type)
        {
            var instance = Enum.ToObject(type, value);
            foreach (var obj in Enum.GetValues(type))
            {
                if (obj.Equals(instance))
                {
                    return instance;
                }
            }
            throw new EnumOutOfBoundsException(value + " is not a valid value for " + type);
        }
    }
}
