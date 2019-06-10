using System.Collections.Generic;
using System.Linq;

namespace Impress.Collections
{
    internal class ReadOnlyListEnumeratorDecorator<T> : IReadOnlyList<T>
    {
        private IEnumerable<T> original;
        private IList<T> list;

        public ReadOnlyListEnumeratorDecorator(IEnumerable<T> original)
        {
            this.original = original;
        }

        private IList<T> GetList()
        {
            if (list == null)
            {
                list = original.ToArray();
            }
            return list;
        }

        public T this[int index]
        {
            get { return GetList()[index]; }
        }

        public int Count
        {
            get { return GetList().Count; }
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
