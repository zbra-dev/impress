using System;
using System.Collections.Generic;
using System.Linq;

namespace Impress.Collections
{
    internal class EmptySet<T> : ISet<T>
    {
        public bool Add(T item)
        {
            throw new NotSupportedException();
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            //no-op
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            //no-op
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return true;
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return true;
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            return true;
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return !other.Any();
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            return false;
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            return !other.Any();
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            // no-op
        }

        public void UnionWith(IEnumerable<T> other)
        {
            throw new NotSupportedException();
        }

        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            //no-op
        }

        public bool Contains(T item)
        {
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            //no-op
        }

        public int Count
        {
            get { return 0; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool Remove(T item)
        {
            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Enumerable.Empty<T>().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Enumerable.Empty<T>().GetEnumerator();
        }
    }
}
