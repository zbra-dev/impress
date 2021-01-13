using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Impress.Tests
{
    internal class SingleEnumerableMock : IEnumerable<int>
    {

        public int CallCount { get; set; } = 0;

        public IEnumerator<int> GetEnumerator()
        {
            CallCount++;
            return Enumerable.Empty<int>().Concat(1).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
