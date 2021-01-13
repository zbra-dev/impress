using System;

namespace Impress
{
    public class Interval<T> where T : IComparable<T>
    {
        public virtual IntervalPoint<T> Start { get; private set; }
        public virtual IntervalPoint<T> End { get; private set; }

        public static Interval<V> Empty<V>() where V : IComparable<V>
        {
            return new EmptyInterval<V>();
        }

        public static Interval<V> ClosedBetween<V>(Maybe<V> start, Maybe<V> end) where V : IComparable<V>
        {
            var startIntervalPoint = IntervalPoint<V>.NegativeInfinity<V>();
            var endIntervalPoint = IntervalPoint<V>.PositiveInfinity<V>();
            if (start.HasValue)
            {
                AssertPointValueNotNull<V>(start.Value);
                startIntervalPoint = IntervalPoint<V>.Closed(start.Value);
            }
            if (end.HasValue)
            {
                AssertPointValueNotNull<V>(end.Value);
                endIntervalPoint = IntervalPoint<V>.Closed(end.Value);
            }
            if (start.HasValue && end.HasValue)
            {
                AssertStartBeforeEnd(start.Value, end.Value);
            }
            return new Interval<V>(startIntervalPoint, endIntervalPoint);
        }

        public static Interval<V> OpenBetween<V>(Maybe<V> start, Maybe<V> end) where V : IComparable<V>
        {
            var startIntervalPoint = IntervalPoint<V>.PositiveInfinity<V>();
            var endIntervalPoint = IntervalPoint<V>.NegativeInfinity<V>();
            if (start.HasValue)
            {
                AssertPointValueNotNull<V>(start.Value);
                startIntervalPoint = IntervalPoint<V>.Open(start.Value);
            }
            if (end.HasValue)
            {
                AssertPointValueNotNull<V>(end.Value);
                endIntervalPoint = IntervalPoint<V>.Open(end.Value);
            }
            if (start.HasValue && end.HasValue)
            {
                AssertStartBeforeEnd(start.Value, end.Value);
            }
            return new Interval<V>(startIntervalPoint, endIntervalPoint);
        }

        /// <summary>
        /// Creates a degenerated interval. A degenerated interval is an interval of one single value. It is the interval representation of a point.
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Interval<V> Degenerated<V>(V value) where V : IComparable<V>
        {
            AssertPointValueNotNull<V>(value);
            return new Interval<V>(IntervalPoint<V>.Closed<V>(value), IntervalPoint<V>.Closed<V>(value));
        }

        public static Interval<V> ClosedStartClosedEnd<V>(V start, V end) where V : IComparable<V>
        {
            AssertPointValueNotNull<V>(start);
            AssertPointValueNotNull<V>(end);
            AssertStartBeforeEnd(start, end);
            return new Interval<V>(IntervalPoint<V>.Closed(start), IntervalPoint<V>.Closed(end));
        }

        public static Interval<V> OpenStartOpenEnd<V>(V start, V end) where V : IComparable<V>
        {
            AssertPointValueNotNull<V>(start);
            AssertPointValueNotNull<V>(end);
            AssertStartBeforeEnd(start, end);

            return new Interval<V>(IntervalPoint<V>.Open(start), IntervalPoint<V>.Open(end));
        }

        public static Interval<V> ClosedStartOpenEnd<V>(V start, V end) where V : IComparable<V>
        {
            AssertPointValueNotNull<V>(start);
            AssertPointValueNotNull<V>(end);
            AssertStartBeforeEnd(start, end);

            return new Interval<V>(IntervalPoint<V>.Closed(start), IntervalPoint<V>.Open(end));
        }

        public static Interval<V> OpenStartClosedEnd<V>(V start, V end) where V : IComparable<V>
        {
            AssertPointValueNotNull<V>(start);
            AssertPointValueNotNull<V>(end);
            AssertStartBeforeEnd(start, end);

            return new Interval<V>(IntervalPoint<V>.Open(start), IntervalPoint<V>.Closed(end));
        }

        public static Interval<V> OpenStartInfinityEnd<V>(V start) where V : IComparable<V>
        {
            AssertPointValueNotNull<V>(start);
            return new Interval<V>(IntervalPoint<V>.Open(start), IntervalPoint<V>.PositiveInfinity<V>());
        }

        public static Interval<V> ClosedStartInfinityEnd<V>(V start) where V : IComparable<V>
        {
            AssertPointValueNotNull<V>(start);
            return new Interval<V>(IntervalPoint<V>.Closed(start), IntervalPoint<V>.PositiveInfinity<V>());
        }

        public static Interval<V> InfinityStartOpenEnd<V>(V end) where V : IComparable<V>
        {
            AssertPointValueNotNull<V>(end);
            return new Interval<V>(IntervalPoint<V>.NegativeInfinity<V>(), IntervalPoint<V>.Open(end));
        }

        public static Interval<V> InfinityStartClosedEnd<V>(V end) where V : IComparable<V>
        {
            AssertPointValueNotNull<V>(end);
            return new Interval<V>(IntervalPoint<V>.NegativeInfinity<V>(), IntervalPoint<V>.Closed(end));
        }

        public static Interval<V> InfinityStartInfinityEnd<V>() where V : IComparable<V>
        {
            return new Interval<V>(IntervalPoint<V>.NegativeInfinity<V>(), IntervalPoint<V>.PositiveInfinity<V>());
        }

        private static void AssertPointValueNotNull<V>(V value) where V : IComparable<V>
        {
            if (value == null)
            {
                throw new ArgumentNullException("Value cannot be null, for infinity intervals use InfinityStart and InfinityEnd methods");
            }
        }

        private static void AssertStartBeforeEnd<V>(V start, V end) where V : IComparable<V>
        {
            if (start.CompareTo(end) > 0)
            {
                throw new ArgumentOutOfRangeException("Start must be lower than end");
            }
        }

        protected Interval(IntervalPoint<T> start, IntervalPoint<T> end)
        {
            Start = start;
            End = end;
        }

        public bool Intersects(Interval<T> other)
        {
            if (this.IsEmpty() || other.IsEmpty())
            {
                return false;
            }
            return this.Equals(other) || this.Includes(other.Start) || this.Includes(other.End) || other.Includes(this.Start) || other.Includes(this.End);
        }

        private bool Includes(IntervalPoint<T> point)
        {
            if (this.IsEmpty())
            {
                return false;
            }

            // no infinits
            var compStart = this.Start.CompareClosedValueTo((point));
            var compEnd = this.End.CompareClosedValueTo((point));

            if (compStart == 0)
            {
                return !this.Start.IsOpen && !point.IsOpen;
            }

            if (compEnd == 0)
            {
                return !this.End.IsOpen && !point.IsOpen;
            }

            return compStart < 0 && compEnd > 0;
        }

        public bool Contains(T value)
        {
            if (this.IsEmpty())
            {
                return false;
            }
            var other = IntervalPoint<T>.Closed(value);
            return other >= Start && other <= End;
        }


        public virtual bool IsEmpty()
        {
            return false;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Interval<T>;
            return other != null && this.Start.Equals(other.Start) && this.End.Equals(other.End);
        }

        public override int GetHashCode()
        {
            return this.Start.GetHashCode() ^ this.End.GetHashCode();
        }
    }
}
