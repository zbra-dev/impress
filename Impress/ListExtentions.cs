using Impress.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Impress
{
    public static class ListExtentions
    {
        public static Maybe<T> SingleRandom<T>(this IList<T> list, Random randomNumberGenerator)
        {
            if (list == null || list.Count == 0)
            {
                return Maybe<T>.Nothing;
            }

            return list[randomNumberGenerator.Next(0, list.Count)].ToMaybe();
        }

        public static Maybe<T> MaybeGet<T>(this IList<T> list, int position)
        {
            if (list == null || list.Count == 0)
            {
                return Maybe<T>.Nothing;
            }

            if (position >= 0 && list.Count > position)
            {
                return list[position].ToMaybe();
            }
            return Maybe<T>.Nothing;
        }

        /// <summary>
        /// Returns an immutable IList with zero elements. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>An immutable IList with zero elements. </returns>
        public static IList<T> EmptyList<T>()
        {
            return new T[0]; // the smallest (footprint) readonly list
        }


        public static IList<T> OrEmpty<T>(this Maybe<IList<T>> maybe)
        {
            return maybe.Or(EmptyList<T>());
        }

        public static IReadOnlyCollection<T> AsReadOnly<T>(this ICollection<T> other)
        {
            return new ReadOnlyCollectionDecorator<T>(other);
        }

        public static IReadOnlyList<T> AsReadOnly<T>(this IList<T> other)
        {
            return new ReadOnlyListDecorator<T>(other);
        }

        public static IReadOnlyList<T> ToReadOnlyList<T>(this IEnumerable<T> other)
        {
            return new ReadOnlyListEnumeratorDecorator<T>(other);
        }

        public static IList<T> ToList<T>(this List<T> other) // necessary to disanbiguate when calling ToList(IReadOnlyList) with a List as argument (that is also a IreadOnlyList)
        {
            return other;
        }

        public static IList<T> ToList<T>(this IReadOnlyList<T> other)
        {
            var list = other as ReadOnlyListDecorator<T>;
            return list != null ? list : new ReadOnlyListDecorator<T>(other);
        }


        public static IList<T> SortBy<T, R>(this IList<T> elements, Func<T, R> selector) where R : IComparable<R>
        {
            Sort(elements, new WithSelectionComparableComparer<T, R>(selector));
            return elements;
        }

        public static void Sort<T, R>(this IList<T> elements, Func<T, R> selector) where R : IComparable<R>
        {
            Sort(elements, new WithSelectionComparableComparer<T, R>(selector));
        }

        public static void Sort<T>(this IList<T> elements) where T : IComparable<T>
        {
            Sort(elements, new ComparableComparer<T>());
        }

        public static void Sort<T>(this IList<T> elements, Comparison<T> comparison)
        {
            Sort(elements, new ComparisonComparer<T>(comparison));
        }

        public static void Sort<T>(this IList<T> elements, IComparer<T> comparer)
        {

            if (elements.Count <= 1)
            {
                return; // empty on single list is already sorted.
            }

            if (elements.IsReadOnly)
            {
                // list must be editable
                throw new Exception("Cannot sort a read only IList in place. Please, copy the read only list to a editable list and sort that new list.");
            }

            // apply different strategies for performance

            if (elements is List<T>)
            {
                // if it is a List, use List.Sort(). This is the majority of cases
                List<T> list = elements as List<T>;
                list.Sort(comparer);
            }
            else
            {
                // fall back for any type of IList

                T[] array = new T[elements.Count];

                elements.CopyTo(array, 0);

                Array.Sort(array, comparer);

                elements.Clear();

                foreach (T item in array)
                {
                    elements.Add(item);
                }
            }

        }

        public static IList<T> IntoNewList<T>(this T element)
        {
            var list = new List<T>();
            list.Add(element);
            return list;
        }

        public static IList<T> IntoList<T>(this T element, IList<T> list)
        {
            list.Add(element);
            return list;
        }

        public static IList<T> IntoList<T>(this IEnumerable<T> enumerable, IList<T> list)
        {
            foreach (var t in enumerable)
            {
                list.Add(t);
            }
            return list;
        }


        public static IList<T> IntoSingleList<T>(this T element)
        { // TODO create SingleList
            var list = new List<T>();
            list.Add(element);
            return list;
        }

        public static ISet<T> IntoNewSet<T>(this T element)
        {
            var set = new HashSet<T>();
            set.Add(element);
            return set;
        }

        public static ISet<T> IntoSingleSet<T>(this T element)
        {
            return new SingleSet<T>(element);
        }

        public static ISet<T> IntoSet<T>(this T element, ISet<T> set)
        {
            set.Add(element);
            return set;
        }

        public static ISet<T> IntoSet<T>(this IEnumerable<T> enumerable, ISet<T> set)
        {
            foreach (var t in enumerable)
            {
                set.Add(t);
            }
            return set;
        }

        public static IList<T> Duplicate<T>(this IList<T> list)
        {
            return new List<T>(list);
        }

        public static IList<T> Swap<T>(this List<T> list, T firstElement, T secondElement)
        {
            if (list == null || firstElement == null || secondElement == null)
            {
                throw new ArgumentException("Arguments cannot be null");
            }

            var firstElementIndex = list.IndexOf(firstElement);
            var secondElementIndex = list.IndexOf(secondElement);

            if (firstElementIndex >= 0 && secondElementIndex >= 0)
            {
                var temp = list[firstElementIndex];
                list[firstElementIndex] = list[secondElementIndex];
                list[secondElementIndex] = temp;
            }
            else
            {
                throw new ArgumentException("Elements are not in the list");
            }
            return list;
        }




        /// <summary>
        /// Disjoins the given lists. The common elements are captured in a new list that is returned.
        /// The commom elements are taken from the list that is the target of the extention invocation.
        /// The elements are only taken from the target lists if they're not read-only
        /// The disjoin operation uses the Equals definition of the given IEqualityComparer
        /// </summary>
        /// <typeparam name="T">The type of object in the lists</typeparam>
        /// <param name="a">the target list</param>
        /// <param name="b">the list to disjoin with</param>
        /// <param name="equalityComparer"> a specified IEqualityComparer of T that will be used in the comparison of objects during the disjoin operation</param>
        /// <returns>a list of the elements in common taken fron the target list</returns>
        public static ICollection<T> Disjoin<T>(this IList<T> a, IList<T> b, IEqualityComparer<T> equalityComparer)
        {
            var intersection = a.Intersect(b, equalityComparer).ToSet();

            Action<IList<T>, T> removeAction = (list, item) =>
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (equalityComparer.Equals(list[i], item))
                    {
                        list.RemoveAt(i);
                        break;
                    }
                }
            };
            Action<IList<T>, T> emptyAction = (list, item) => { };

            var aListAction = a.IsReadOnly ? emptyAction : removeAction;
            var bListAction = b.IsReadOnly ? emptyAction : removeAction;

            if (!a.IsReadOnly || !b.IsReadOnly)
            {
                foreach (var obj in intersection)
                {
                    aListAction(a, obj);
                    bListAction(b, obj);
                }
            }

            return intersection;
        }

        public static ICollection<T> DisjoinByProperty<T>(this IList<T> a, IList<T> b, Func<T, object> propertySelector)
        {
            return Disjoin<T>(a, b, new IEqualityComparerAdapter<T>(propertySelector));
        }

        public static void AddRange<T>(this IList<T> list, IEnumerable<T> other)
        {
            var hardList = list as List<T>;
            if (hardList == null)
            {
                foreach (var t in other)
                {
                    list.Add(t);
                }
            }
            else
            {
                hardList.AddRange(other);
            }
        }



        internal class IEqualityComparerAdapter<T> : IEqualityComparer<T>
        {
            private Func<T, object> propertySelector;

            public IEqualityComparerAdapter(Func<T, object> propertySelector)
            {
                this.propertySelector = propertySelector;
            }

            public bool Equals(T x, T y)
            {
                return propertySelector(x).Equals(propertySelector(y));
            }

            public int GetHashCode(T obj)
            {
                return propertySelector(obj).GetHashCode();
            }
        }
    }
}
