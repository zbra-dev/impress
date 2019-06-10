using System;

namespace Impress.Math
{
    public static class Rounding
    {

        private static decimal RoundUp(decimal value, decimal factor)
        {
            return value.Sign() * Decimal.Ceiling(value.Abs() * factor) / factor;
        }

        private static decimal RoundDown(decimal value, decimal factor)
        {
            return value.Sign() * Decimal.Floor(value.Abs() * factor) / factor;
        }

        public static decimal Round(this decimal value, int decimalplaces, RoundingMode mode)
        {
            //http://en.wikipedia.org/wiki/Rounding

            int factor = decimalplaces.PowerOfTen();

            if (mode == RoundingMode.RoundCelling)
            {
                if (value >= 0)
                {
                    return RoundUp(value, factor);
                }
                else
                {
                    return RoundDown(value, factor);
                }
            }
            else if (mode == RoundingMode.RoundFloor)
            {
                if (value >= 0)
                {
                    return RoundDown(value, factor);
                }
                else
                {
                    return RoundUp(value, factor);
                }
            }
            else if (mode == RoundingMode.RoundUp)
            {
                return RoundUp(value, factor);
            }
            else if (mode == RoundingMode.RoundDown)
            {
                return RoundDown(value, factor);
            }

            decimal absolute = value.Abs() * factor;
            decimal discardedFraction = absolute.FractionaryPart();

            if (mode == RoundingMode.RoundHalfUp)
            {
                if (discardedFraction >= 0.5M)
                {
                    return RoundUp(value, factor);
                }
                else
                {
                    return RoundDown(value, factor);
                }
            }
            else if (mode == RoundingMode.RoundHalfDown)
            {
                if (discardedFraction > 0.5M)
                {
                    return RoundUp(value, factor);
                }
                else
                {
                    return RoundDown(value, factor);
                }
            }
            else if (mode == RoundingMode.RoundHalfEven)
            {
                return Round(value, decimalplaces, Decimal.Floor(absolute) % 2 == 0 ? RoundingMode.RoundHalfDown : RoundingMode.RoundHalfUp);
            }
            else if (mode == RoundingMode.RoundHalfOdd)
            {
                return Round(value, decimalplaces, Decimal.Floor(absolute) % 2 != 0 ? RoundingMode.RoundHalfDown : RoundingMode.RoundHalfUp);
            }

            throw new ArgumentException("Rounding type " + mode.ToString() + " is not recognized");
        }
    }
}
