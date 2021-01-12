using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Impress
{
    public static class EnumerableExtensions
    {


        public static Maybe<T> MaybeSingle<T>(this IEnumerable<Nullable<T>> enumerable) where T : struct
        {
            if (enumerable == null)
            {
                return Maybe<T>.Nothing;
            }
            try
            {
                return enumerable.Single().ToMaybe();
            }
            catch (InvalidOperationException)
            {
                return Maybe<T>.Nothing;
            }
        }

        public static Maybe<T> MaybeSingle<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                return Maybe<T>.Nothing;
            }
            try
            {
                return enumerable.Single().ToMaybe();
            }
            catch (InvalidOperationException)
            {
                return Maybe<T>.Nothing;
            }
        }

        #region Maybe First 


        public static Maybe<T> MaybeFirst<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                return Maybe<T>.Nothing;
            }
            try
            {
                return enumerable.First().ToMaybe();
            }
            catch (InvalidOperationException)
            {
                return Maybe<T>.Nothing;
            }
            
        }

        public static Maybe<T> MaybeFirst<T>(this IEnumerable<Nullable<T>> enumerable) where T : struct
        {
            return (enumerable == null || !enumerable.Any()) ? Maybe<T>.Nothing : enumerable.First<Nullable<T>>().ToMaybe<T>();
        }

        #endregion

        #region Maybe Last


        public static Maybe<T> MaybeLast<T>(this IEnumerable<T> enumerable) where T : class
        {
            return (enumerable == null || !enumerable.Any()) ? Maybe<T>.Nothing : enumerable.Last().ToMaybe();
        }

        public static Maybe<T> MaybeLast<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate) where T : class
        {
            return (enumerable == null || !enumerable.Any()) ? Maybe<T>.Nothing : MaybeLast(enumerable.Where(predicate));
        }

        public static Maybe<T> MaybeLast<T>(this IEnumerable<Nullable<T>> enumerable) where T : struct
        {
            return (enumerable == null || !enumerable.Any()) ? Maybe<T>.Nothing : enumerable.Last<Nullable<T>>().ToMaybe<T>();
        }

        #endregion


        public static Maybe<T> ElementAtRandom<T>(this ICollection<T> collection, Random randomNumberGenerator)
        {
            if (collection == null || !collection.Any())
            {
                return Maybe<T>.Nothing;
            }
            else if (collection.Count == 1)
            {
                return collection.First().ToMaybe();
            }

            return collection.ElementAt(randomNumberGenerator.Next(0, collection.Count)).ToMaybe();
        }

        public static Maybe<T> ElementAtRandom<T>(this IList<T> list, Random randomNumberGenerator)
        {
            if (list == null || !list.Any())
            {
                return Maybe<T>.Nothing;
            }
            else if (list.Count == 1)
            {
                return list[0].ToMaybe();
            }

            return list[randomNumberGenerator.Next(0, list.Count)].ToMaybe();
        }

        public static Maybe<T> ElementAtRandom<T>(this IEnumerable<T> enumerable, Random randomNumberGenerator)
        {
            if (enumerable == null)
            {
                return Maybe<T>.Nothing;
            }

            var count = enumerable.Count();
            if (count == 0)
            {
                return Maybe<T>.Nothing;
            }
            else if (count == 1)
            {
                return enumerable.First().ToMaybe();
            }

            return enumerable.ElementAt(randomNumberGenerator.Next(0, count)).ToMaybe();
        }


        public static IEnumerable<T> OrEmpty<T>(this IEnumerable<T> items)
        {
            return items == null ? new T[0] : items;
        }

        public static IEnumerable<T> OrEmpty<T>(this Maybe<IEnumerable<T>> maybe)
        {
            return maybe.Or(Enumerable.Empty<T>());
        }

        public static IEnumerable<T> Peek<T>(this IEnumerable<T> elements, Action<T> action)
        {
            return elements.Select(m => { action(m); return m; });
        }

        public static IEnumerable<Tuple<K, V>> Associate<K, V>(this IEnumerable<K> elements, IDictionary<K, V> dictionary)
        {
            return Associate(elements, dictionary, e => e, (o, m) => Tuple.Create(o, m));
        }

        public static IEnumerable<Tuple<T, V>> Associate<T, K, V>(this IEnumerable<T> elements, IDictionary<K, V> dictionary, Func<T, K> mapping)
        {
            return Associate(elements, dictionary, mapping, (o, m) => Tuple.Create(o, m));
        }

        public static IEnumerable<R> Associate<T, K, V, R>(this IEnumerable<T> elements, IDictionary<K, V> dictionary, Func<T, K> mapping, Func<T, V, R> constructor)
        {
            if (dictionary.Count == 0)
            {
                yield break;
            }

            foreach (var element in elements)
            {
                V value;
                if (dictionary.TryGetValue(mapping(element), out value))
                {
                    yield return constructor(element, value);
                }
            }
        }

        /// <summary>
        /// Removes all Nothing values in the given enumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elements"></param>
        /// <returns></returns>
        public static IEnumerable<T> Compact<T>(this IEnumerable<Maybe<T>> elements)
        {
            return elements.Where(m => m.HasValue).Select(m => m.Value);
        }

        /// <summary>
        /// Removes all null values in the given enumerable.
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="elements"></param>
        /// <returns></returns>
        public static IEnumerable<S> Compact<S>(this IEnumerable<Nullable<S>> elements) where S : struct
        {
            return elements.Where(m => m.HasValue).Select(m => m.Value);
        }

        /// <summary>
        /// Removes all null values in the given enumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elements"></param>
        /// <returns></returns>
        public static IEnumerable<T> Compact<T>(this IEnumerable<T> elements)
        {
            return elements.Where(m => m != null);
        }

        public static IEnumerable<S> Convert<S>(this IEnumerable<string> elements) where S : struct
        {
            foreach (var s in elements)
            {
                yield return GenericTypeConverter.GetConverter().Convert<S>(s);
            }
        }

        public static IEnumerable<IGrouping<int, T>> Split<T>(this IEnumerable<T> elements, int size)
        {
            var list = new List<T>(size);

            var groupCount = 0;
            foreach (T item in elements)
            {
                list.Add(item);
                if (list.Count == size)
                {
                    List<T> chunk = list;
                    list = new List<T>(size);
                    yield return new Group<T>(groupCount++, chunk);
                }
            }

            if (list.Count > 0)
            {
                yield return new Group<T>(groupCount, list);
            }
        }

        private class Group<T> : IGrouping<int, T>
        {
            private int groupCount;
            private IEnumerable<T> enumerable;

            public Group(int groupCount, IEnumerable<T> enumerable)
            {
                this.groupCount = groupCount;
                this.enumerable = enumerable;
            }

            public int Key
            {
                get { return groupCount; }
            }

            public IEnumerator<T> GetEnumerator()
            {
                return enumerable.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return enumerable.GetEnumerator();
            }
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> enumerable, T obj)
        {
            return enumerable.Concat(Enumerable.Repeat(obj, 1));
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> enumerable, Maybe<T> maybe)
        {
            return maybe.HasValue 
                ? enumerable.Concat(Enumerable.Repeat(maybe.Value, 1)) 
                : enumerable;
        }


        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> enumerable, Comparison<T> comparison)
        {
            return enumerable == null 
                ? Enumerable.Empty<T>() 
                : enumerable.OrderBy(t => t, new ComparisonComparer<T>(comparison));
        }

        public static ISet<T> ToSet<T>(this IEnumerable<T> collection)
        {
            return collection == null ? new HashSet<T>() : new HashSet<T>(collection);
        }

        public static C Into<T, C>(this IEnumerable<T> enumerable, C collection) where C : ICollection<T>
        {
            foreach (T t in enumerable)
            {
                collection.Add(t);
            }

            return collection;
        }

        /// <summary>
        /// Simply forces EF to load all objects in the IQueryable 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        public static void LoadImplicitly<T>(this IQueryable<T> query)
        {
            // the use of for each is no good because maintains the connection open given raise to deadloks
            query.ToList();

        }

        /// <summary>
        /// Iterates the enumerable and performs the given action or each element.
        /// The mane of this extension is compatible with List.ForEach
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (enumerable != null)
            {
                foreach (T t in enumerable)
                {
                    action(t);
                }
            }
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> action)
        {
            if (enumerable != null)
            {
                int i = 0;
                foreach (T t in enumerable)
                {
                    action(t, i++);
                }
            }
        }

        /// <summary>
        /// Decouples a LINQ to SQL IEnumerable. When converting objects using functions LINQ to SQL does not properly understands and tries 
        /// to convert the function to SQL witch is not possible. An exception occurs. The programmer is forced to execute an equivalent to ToList() before the select in order to
        /// do the Select as in memory operation.
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static IEnumerable<R> Decouple<T, R>(this IEnumerable<T> enumerable, Expression<Func<IEnumerable<T>, IEnumerable<R>>> expression)
        {
            return expression.Compile()(enumerable.ToList());
        }


        //does not use deferred execution!
        //do not change it!
        public static void ReturnlessZip<T, U>(this IEnumerable<T> firstEnumerable, IEnumerable<U> secondEnumerable, Action<T, U> action)
        {
            using (var firstEnumerator = firstEnumerable.GetEnumerator())
            {
                using (var secondEnumerator = secondEnumerable.GetEnumerator())
                {
                    while (firstEnumerator.MoveNext() && secondEnumerator.MoveNext())
                    {
                        action(firstEnumerator.Current, secondEnumerator.Current);
                    }
                }
            }
        }
    }
}
