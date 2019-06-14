using System;

namespace Impress
{
    public class IntervalPoint<T> where T : IComparable<T>
    {
        public Maybe<T> Point { get; private set; }
        public bool IsOpen { get; private set; }

        public static IntervalPoint<V> NegativeInfinity<V>() where V : IComparable<V>
        {
            return new NegativeInfinityIntervalPoint<V>();
        }

        public static IntervalPoint<V> PositiveInfinity<V>() where V : IComparable<V>
        {
            return new PositiveInfinityIntervalPoint<V>();
        }

        public static IntervalPoint<V> Closed<V>(V value) where V : IComparable<V>
        {
            return new IntervalPoint<V>(value.ToMaybe(), false);
        }

        public static IntervalPoint<V> Open<V>(V value) where V : IComparable<V>
        {
            return new IntervalPoint<V>(value.ToMaybe(), true);
        }

        internal IntervalPoint(Maybe<T> point, bool isOpen)
        {
            Point = point;
            IsOpen = isOpen;
        }

        public virtual bool IsPositiveInfinity()
        {
            return false;
        }

        public virtual bool IsNegativeInfinity()
        {
            return false;
        }

        public bool IsInfinity()
        {
            return this.IsPositiveInfinity() || this.IsNegativeInfinity();
        }

        private bool InfinityGreaterThanComparisons(IntervalPoint<T> other)
        {
            if (!this.IsInfinity() && !other.IsInfinity())
            {
                throw new ArgumentOutOfRangeException("At least one value must be infinity");
            }
            return !(this.IsNegativeInfinity() || other.IsPositiveInfinity());
        }

        protected bool GreaterThan(IntervalPoint<T> other)
        {
            if (this.IsInfinity() || other.IsInfinity())
            {
                return InfinityGreaterThanComparisons(other);
            }
            return this.Point.CompareTo(other.Point) > 0;
        }


        protected bool GreaterOrEqualTo(IntervalPoint<T> other)
        {
            if (this.IsInfinity() || other.IsInfinity())
            {
                return InfinityGreaterThanComparisons(other);
            }
            return this.Point.CompareTo(other.Point) > 0 || (!this.IsOpen && !other.IsOpen && this.Point.CompareTo(other.Point) == 0);
        }

        private bool InfinityLessThanComparisons(IntervalPoint<T> other)
        {
            if (!this.IsInfinity() && !other.IsInfinity())
            {
                throw new ArgumentOutOfRangeException("At least one value must be infinity");
            }
            return !(this.IsPositiveInfinity() || other.IsNegativeInfinity());
        }

        public int CompareClosedValueTo(IntervalPoint<T> other)
        {
            if (this.IsNegativeInfinity())
            {
                if (other.IsNegativeInfinity())
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else if (this.IsPositiveInfinity())
            {
                if (other.IsPositiveInfinity())
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else if (other.IsNegativeInfinity())
            {
                if (this.IsNegativeInfinity())
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else if (other.IsPositiveInfinity())
            {
                if (this.IsPositiveInfinity())
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }

            return this.Point.CompareTo(other.Point);

        }

        protected bool LessThan(IntervalPoint<T> other)
        {
            if (this.IsInfinity() || other.IsInfinity())
            {
                return InfinityLessThanComparisons(other);
            }
            return this.Point.CompareTo(other.Point) < 0;
        }

        protected bool LessThanOrEqualTo(IntervalPoint<T> other)
        {
            if (this.IsInfinity() || other.IsInfinity())
            {
                return InfinityLessThanComparisons(other);
            }
            return this.Point.CompareTo(other.Point) < 0 || (!this.IsOpen && !other.IsOpen && this.Point.CompareTo(other.Point) == 0);
        }

        public override bool Equals(object obj)
        {
            var other = obj as IntervalPoint<T>;
            if (this.IsNegativeInfinity())
            {
                return other.IsNegativeInfinity();
            }
            else if (this.IsPositiveInfinity())
            {
                return other.IsPositiveInfinity();
            }
            else if (other.IsNegativeInfinity())
            {
                return this.IsNegativeInfinity();
            }
            else if (other.IsPositiveInfinity())
            {
                return this.IsPositiveInfinity();
            }

            return other != null && this.Point.Equals(other.Point) && this.IsOpen.Equals(other.IsOpen);
        }

        public override int GetHashCode()
        {
            return this.Point.GetHashCode();
        }


        public static bool operator >(IntervalPoint<T> x, IntervalPoint<T> y)
        {
            return x.GreaterThan(y);
        }

        public static bool operator <(IntervalPoint<T> x, IntervalPoint<T> y)
        {
            return x.LessThan(y);
        }

        public static bool operator >=(IntervalPoint<T> x, IntervalPoint<T> y)
        {
            return x.GreaterOrEqualTo(y);
        }

        public static bool operator <=(IntervalPoint<T> x, IntervalPoint<T> y)
        {
            return x.LessThanOrEqualTo(y);
        }
    }

    public class NegativeInfinityIntervalPoint<T> : IntervalPoint<T> where T : IComparable<T>
    {
        public NegativeInfinityIntervalPoint() : base(Maybe<T>.Nothing, true) { }

        public override bool IsNegativeInfinity()
        {
            return true;
        }

    }

    public class PositiveInfinityIntervalPoint<T> : IntervalPoint<T> where T : IComparable<T>
    {
        public PositiveInfinityIntervalPoint() : base(Maybe<T>.Nothing, true) { }

        public override bool IsPositiveInfinity()
        {
            return true;
        }
    }
}
