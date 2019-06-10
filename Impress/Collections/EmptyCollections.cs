using System.Collections.Generic;

namespace Impress.Collections
{
    public static class EmptyCollections
    {
        public static IList<T> EmptyList<T>()
        {
            return new List<T>(0); // the smallest (footprint) editable list
        }

        public static IReadOnlyList<T> EmptyReadOnlyList<T>()
        {
            return new T[0]; // the smallest (footprint) readonly list
        }

        public static ISet<T> EmptySet<T>()
        {
            return new EmptySet<T>();
        }

        public static IDictionary<K, V> EmptyDictionary<K, V>()
        {
            return new Dictionary<K, V>(0);
        }

    }
}
