using System;
using System.Collections;
using System.Collections.Generic;

namespace Impress
{
    internal class ComparableComparer<T> : IComparer<T> where T : IComparable<T>
    {
        public int Compare(T x, T y)
        {
            return x.CompareTo(y);
        }
    }

    internal class WithSelectionComparableComparer<T, R> : IComparer<T> where R : IComparable<R>
    {
        private Func<T, R> selector;

        public WithSelectionComparableComparer(Func<T, R> selector)
        {
            this.selector = selector;
        }

        public int Compare(T x, T y)
        {
            return selector(x).CompareTo(selector(y));
        }
    }

    internal class ComparisonComparer<T> : IComparer<T>, IComparer
    {
        private readonly Comparison<T> comparison;

        public ComparisonComparer(Comparison<T> comparison)
        {
            this.comparison = comparison;
        }

        public int Compare(T x, T y)
        {
            return comparison(x, y);
        }

        public int Compare(object o1, object o2)
        {
            return comparison((T)o1, (T)o2);
        }
    }
}
