using System;
using System.Collections.Generic;
using System.Linq;

namespace Impress.Collections
{
    internal class SingleSet<T> : ISet<T>
    {

        private T obj;

        public SingleSet(T obj)
        {
            this.obj = obj;
        }

        public bool Add(T item)
        {
            throw new NotSupportedException();
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            throw new NotSupportedException();
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            throw new NotSupportedException();
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return obj != null && other.Count() == 1 && obj.Equals(other);
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return obj != null && other.Contains(obj);
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            return obj != null && other.Count() == 1 && obj.Equals(other);
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return obj != null && other.Contains(obj);
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            return obj != null && other.Count() == 1 && obj.Equals(other);
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            if (other.Count() != 1)
            {
                return false;
            }
            obj = other.Single();
            return true;
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            throw new NotSupportedException();
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
            throw new NotSupportedException();
        }

        public bool Contains(T item)
        {
            return item != null && item.Equals(obj);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (obj != null)
            {
                array[arrayIndex] = obj;
            }
        }

        public int Count
        {
            get { return obj == null ? 0 : 1; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool Remove(T item)
        {
            throw new NotSupportedException();
        }

        private IEnumerable<T> GetEnumerable()
        {
            yield return obj;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return GetEnumerable().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerable().GetEnumerator();
        }
    }
}
