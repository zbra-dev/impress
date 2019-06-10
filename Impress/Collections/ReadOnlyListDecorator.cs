using System.Collections.Generic;

namespace Impress.Collections
{
    internal class ReadOnlyListDecorator<T> : IReadOnlyList<T>, IList<T>
    {
        private IList<T> original;

        public ReadOnlyListDecorator(IList<T> original)
        {
            this.original = original;
        }

        public ReadOnlyListDecorator(IEnumerable<T> original)
        {
            this.original = new List<T>(original);
        }

        public int Count
        {
            get { return original.Count; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return original.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return original.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return original.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            throw new System.NotSupportedException();
        }

        public void RemoveAt(int index)
        {
            throw new System.NotSupportedException();
        }

        public T this[int index]
        {
            get
            {
                return original[index];
            }
            set
            {
                throw new System.NotSupportedException();
            }
        }

        public void Add(T item)
        {
            throw new System.NotSupportedException();
        }

        public void Clear()
        {
            throw new System.NotSupportedException();
        }

        public bool Contains(T item)
        {
            return original.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            original.CopyTo(array, arrayIndex);
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool Remove(T item)
        {
            throw new System.NotSupportedException();
        }
    }
}
