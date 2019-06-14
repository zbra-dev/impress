using System;
using System.Collections.Generic;
using System.Linq;

namespace Impress
{
    public static class CollectionExtensions
    {
        public static Maybe<T> SingleRandom<T>(this ICollection<T> collection, Random randomNumberGenerator)
        {
            if (collection == null || collection.Count == 0)
            {
                return Maybe<T>.Nothing;
            }

            return collection.ElementAt(randomNumberGenerator.Next(0, collection.Count)).ToMaybe();
        }

        /// <summary>
        /// Disjoins the given lists. The common elements are captured in a new list that is returned.
        /// The common elements are taken from the list that is the target of the extension invocation.
        /// The elements are only taken from the target lists if they're not read-only
        /// the disjoin operation uses the Equals definition of the object type.
        /// </summary>
        /// <typeparam name="T">The type of object in the lists</typeparam>
        /// <param name="a">the target list</param>
        /// <param name="b">the list to disjoin with</param>
        /// <returns>a list of the elements in common taken from the target list</returns>
        public static ICollection<T> Disjoin<T>(this ICollection<T> a, ICollection<T> b)
        {
            var intersection = a.Intersect(b).ToSet();

            Action<ICollection<T>, T> removeAction = (list, item) => list.Remove(item);
            Action<ICollection<T>, T> emptyAction = (list, Item) => { };

            var aListAction = a.IsReadOnly ? emptyAction : removeAction;
            var bListAction = b.IsReadOnly ? emptyAction : removeAction;

            if (!a.IsReadOnly || !b.IsReadOnly)
            {
                foreach (T t in intersection)
                {
                    aListAction(a, t);
                    bListAction(b, t);
                }
            }

            return intersection;
        }

        public static IReadOnlyCollection<T> OrEmpty<T>(this Maybe<IReadOnlyCollection<T>> maybe)
        {
            return maybe.Or(SetExtensions.EmptySet<T>().AsReadOnly());
        }

        public static ICollection<T> OrEmpty<T>(this Maybe<ICollection<T>> maybe)
        {
            return maybe.Or(SetExtensions.EmptySet<T>());
        }

    }
}
