using System;
using System.Collections.Generic;

namespace Impress.Collections
{
    public class EnumCollections
    {

        public static ISet<E> AsSet<E>() where E : struct
        {
            return new HashSet<E>((E[])Enum.GetValues(ChecIsEnum<E>()));
        }

        public static IList<E> AsList<E>() where E : struct
        {
            return new List<E>((E[])Enum.GetValues(ChecIsEnum<E>()));
        }

        public static IReadOnlyList<E> AsReadOnlyList<E>() where E : struct
        {
            return (E[])Enum.GetValues(ChecIsEnum<E>());
        }

        public static IEnumerable<E> AsEnumerable<E>() where E : struct
        {
            return (E[])Enum.GetValues(ChecIsEnum<E>());
        }

        private static Type ChecIsEnum<E>() where E : struct
        {
            var type = typeof(E);
            if (!type.IsEnum)
            {
                throw new ArgumentException("Type is not an enum");
            }

            return type;
        }
    }
}
