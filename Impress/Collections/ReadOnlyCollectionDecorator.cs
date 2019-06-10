using System.Collections.Generic;

namespace Impress.Collections
{
    internal class ReadOnlyCollectionDecorator<T> : IReadOnlyCollection<T>
    {
        private readonly ICollection<T> original;

        public ReadOnlyCollectionDecorator(ICollection<T> original)
        {
            this.original = original;
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
    }
}
