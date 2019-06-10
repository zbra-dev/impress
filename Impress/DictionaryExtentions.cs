using System;
using System.Collections.Generic;
using System.Linq;

namespace Impress
{
    public static class DictionaryExtentions
    {
        /// <summary>
        /// Returns an immutable IDictionary with zero elements. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>An immutable IDictionary with zero elements. </returns>
        public static IDictionary<K, V> EmptyDictionary<K, V>()
        {
            return new Dictionary<K, V>(0);
        }

        public static IDictionary<K, V> OrEmpty<K, V>(this Maybe<IDictionary<K, V>> maybe)
        {
            return maybe.Or(EmptyDictionary<K, V>());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>true if the element is added by the first time, false otherwise</returns>
        public static bool ReplaceOrAdd<K, V>(this IDictionary<K, V> dictionary, K key, V value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
                return true;
            }
            else
            {
                dictionary.Add(key, value);
                return false;
            }
        }

        public static Maybe<V> MaybeGet<K, V>(this IEnumerable<KeyValuePair<K, V>> dictionary, K key)
        {
            return DoMaybeGet(dictionary, key);
        }

        private static Maybe<V> DoMaybeGet<K, V>(this IEnumerable<KeyValuePair<K, V>> dictionary, K key)
        {

            V value;
            if (DoGeneralTryGetValue(dictionary, key, out value))
            {
                return value.ToMaybe();
            }
            return Maybe<V>.Nothing;
        }

        /// <summary>
        /// Reads a value from a dictionary. Returns false if the key does not exist on the dictionary.
        /// This method is compatible with IReadOnlyDictionary and IDictionary and tries to cast to them when possible. 
        /// This is due to a limitation on the .NET api design where IDictionary does not implement IReadOnlyDictionary creating
        /// an ambigousity for the extention resolution algorithm to handle. 
        /// To circunvein this problem, this method allows for any IEnumrable of KeyValuePair since is the only 
        /// common interface for both IreadOnlyDictionary and IDictionary. 
        /// The method will try to use the TryGetValue on the IDictionary and IReadOnlyInterfaces, and falls back to a linear search 
        /// if the object does not implement one of those
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static bool DoGeneralTryGetValue<K, V>(this IEnumerable<KeyValuePair<K, V>> dictionary, K key, out V value)
        {
            if (dictionary == null || !dictionary.Any())
            {
                value = default(V);
                return false;
            }


            //  most common since is the mutable version
            IDictionary<K, V> readOther = dictionary as IDictionary<K, V>;

            if (readOther == null)
            {
                // thy this one before falling back to list search
                IReadOnlyDictionary<K, V> dic = dictionary as IReadOnlyDictionary<K, V>;

                if (dic == null)
                {
                    // fall back to direct search
                    // use to List to overcome the problem with SingleOrDefault since if 
                    // V is a struct the default is a , possible and otherwise present, value.
                    var list = dictionary.Where(pair => pair.Key.Equals(key)).ToList();
                    if (list.Count > 0)
                    {
                        value = list[0].Value;
                        return true;
                    }
                }
                else
                {
                    return dic.TryGetValue(key, out value);
                }
            }
            else
            {
                return readOther.TryGetValue(key, out value);
            }


            value = default(V);
            return false;
        }

        public static Maybe<V> MaybeGet<K, V>(this IEnumerable<KeyValuePair<K, V>> dictionary, Maybe<K> key)
        {
            if (key.HasValue)
            {
                return DoMaybeGet(dictionary, key.Value);
            }

            return Maybe<V>.Nothing;
        }

        public static Maybe<V> MaybeGet<K, V>(this IEnumerable<KeyValuePair<K, Maybe<V>>> dictionary, K key)
        {
            return DoMaybeGetOnDictionaryWithMaybeValues(dictionary, key);
        }

        private static Maybe<V> DoMaybeGetOnDictionaryWithMaybeValues<K, V>(this IEnumerable<KeyValuePair<K, Maybe<V>>> dictionary, K key)
        {
            Maybe<V> value;
            if (DoGeneralTryGetValue(dictionary, key, out value))
            {
                return value;
            }
            return Maybe<V>.Nothing;
        }

        public static Maybe<V> MaybeGet<K, V>(this IDictionary<K, Maybe<V>> dictionary, Maybe<K> key)
        {
            if (key.HasValue)
            {
                Maybe<V> value;
                if (dictionary.Count != 0 && dictionary.TryGetValue(key.Value, out value))
                {
                    return value;
                }
            }
            return Maybe<V>.Nothing;
        }

        public static Maybe<V> MaybeGet<K, V>(this IDictionary<K, V> dictionary, Maybe<K> key)
        {
            if (key.HasValue)
            {
                V value;
                if (dictionary.Count != 0 && dictionary.TryGetValue(key.Value, out value))
                {
                    return value.ToMaybe();
                }
            }

            return Maybe<V>.Nothing;
        }


        public static Maybe<V> MaybeGetOrAdd<K, V>(this IDictionary<K, V> dictionary, K key, Func<K, V> constructor)
        {
            if (dictionary == null)
            {
                return Maybe<V>.Nothing;
            }

            V value;
            if (dictionary.TryGetValue(key, out value))
            {
                return value.ToMaybe();
            }
            else
            {
                value = constructor(key);
                dictionary.Add(key, value);
                return value.ToMaybe();
            }

        }

        public static Maybe<V> MaybeGetOrAdd<K, V>(this IDictionary<K, Maybe<V>> dictionary, K key, Func<K, Maybe<V>> constructor)
        {
            if (dictionary == null)
            {
                return Maybe<V>.Nothing;
            }

            Maybe<V> value;
            if (dictionary.TryGetValue(key, out value))
            {
                return value;
            }
            else
            {
                value = constructor(key);
                dictionary.Add(key, value);
                return value;
            }
        }

        public static Maybe<V> MaybeGetOrAdd<K, V>(this IDictionary<K, Maybe<V>> dictionary, Maybe<K> key, Func<K, Maybe<V>> constructor)
        {
            if (dictionary == null)
            {
                return Maybe<V>.Nothing;
            }

            if (key.HasValue)
            {
                Maybe<V> value;
                if (dictionary.TryGetValue(key.Value, out value))
                {
                    return value;
                }
                else
                {
                    value = constructor(key.Value);
                    dictionary.Add(key.Value, value);
                    return value;
                }
            }
            return Maybe<V>.Nothing;
        }

        public static Maybe<V> MaybeGetOrAdd<K, V>(this IDictionary<K, V> dictionary, Maybe<K> key, Func<K, V> constructor)
        {
            if (dictionary == null)
            {
                return Maybe<V>.Nothing;
            }

            if (key.HasValue)
            {
                V value;
                if (dictionary.TryGetValue(key.Value, out value))
                {
                    return value.ToMaybe();
                }
                else
                {
                    value = constructor(key.Value);
                    dictionary.Add(key.Value, value);
                    return value.ToMaybe();
                }
            }

            return Maybe<V>.Nothing;
        }
    }
}
