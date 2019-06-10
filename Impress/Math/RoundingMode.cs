namespace Impress.Math
{
    public enum RoundingMode
    {
        /// <summary>
        /// Rounding mode to round towards positive infinity.
        /// 
        /// Rounding mode to round towards positive infinity. If the Decimal is positive, behaves as for RoundUp; if negative, 
        /// behaves as for RoundDown. Note that this rounding mode never decreases the calculated value.
        /// </summary>
        RoundCelling,
        /// <summary>
        /// Rounding mode to round towards negative infinity.
        /// Rounding mode to round towards negative infinity. If the Decimal is positive, behave as for RoundDown; if negative, 
        /// behave as for RoundUp. Note that this rounding mode never increases the calculated value.
        /// </summary>
        RoundFloor,
        /// <summary>
        /// Rounding mode to round towards zero.
        /// Rounding mode to round towards zero. Never increments the digit prior to a discarded fraction (i.e., truncates). 
        /// Note that this rounding mode never increases the magnitude of the calculated value.
        /// </summary>
        RoundDown,
        /// <summary>
        /// Rounding mode to round away from zero.
        /// Rounding mode to round away from zero. Always increments the digit prior to a nonzero discarded fraction. 
        /// Note that this rounding mode never decreases the magnitude of the calculated value.
        /// </summary>
        RoundUp,
        /// <summary>
        /// Rounding mode to round towards "nearest neighbor" unless both neighbors are equidistant, in which case round down.
        /// Behaves as for RoundUp if the discarded fraction is > 0.5; otherwise, behaves as for RoundDown.
        /// </summary>
        RoundHalfDown,
        /// <summary>
        /// Rounding mode to round towards "nearest neighbor" unless both neighbors are equidistant, in which case round up. 
        /// Behaves as for RoundUp if the discarded fraction is ≥ 0.5; otherwise, behaves as for RoundDown. 
        /// Note that this is the rounding mode that most of us were taught in grade school.
        /// </summary>
        RoundHalfUp,
        /// <summary>
        /// Rounding mode to round towards the "nearest neighbor" unless both neighbors are equidistant, in which case, round towards the even neighbor.
        /// Behaves as for RoundHalfUp if the digit to the left of the discarded fraction is odd; behaves as for RoundHalfDown if it's even. 
        /// Note that this is the rounding mode that minimizes cumulative error when applied repeatedly over a sequence of calculations.
        /// </summary>
        RoundHalfEven,
        /// <summary>
        /// Rounding mode to round towards the "nearest neighbor" unless both neighbors are equidistant, in which case, round towards the odd neighbor.
        /// Behaves as for RoundHalfDown if the digit to the left of the discarded fraction is odd; behaves as for RoundHalfUp if it's even. 
        /// </summary>
        RoundHalfOdd
    }
}
