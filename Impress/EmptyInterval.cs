using System;

namespace Impress
{
    internal sealed class EmptyInterval<T> : Interval<T> where T : IComparable<T>
    {
        internal EmptyInterval()
            : base(
                new IntervalPoint<T>(Maybe<T>.Nothing, false),
                new IntervalPoint<T>(Maybe<T>.Nothing, false)
                )
        { }

        public override bool IsEmpty()
        {
            return true;
        }
    }
}
