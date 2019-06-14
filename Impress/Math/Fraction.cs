using System;
using System.ComponentModel;
using System.Numerics;
using System.Text;

namespace Impress.Math
{
    /// <summary>
    /// Represents a rational number that is not , necessarily, an integer.  Examples 2/3 , 8/5, 9/1, 0/1
    /// Integers are not closed for division, in fact no division operation exists that taken two integers always produces another integer.
    /// There is an Integer Division operation that taking two integers produces another two integers know as quotient and remainder.
    /// To expand the concept of division, in a closed form, the rational numbers (fractions) are introduced.  All four basic arithmetic operations are closed for rational numbers.
    /// 
    /// Rational numbers allow for long calculations with no loss of precision that is not possible with double , float or even decimal.
    /// 
    /// Using doubles a for loop that suns 0.1 ten times does not result in 1. Using Fraction it does.
    /// 
    /// For creating a fraction prefer the ValueOf overloads that use integers or strings. Use the other overloads only in conversion scenarios.
    /// 
    /// Internally this implementation uses BigInteger to retain the numerator and denominator values. 
    /// After each calculation the fraction is simplified so the numerator and denominator are always coprimes.
    /// </summary>
    public struct Fraction : IComparable<Fraction>
    {
        public static readonly Fraction Zero = new Fraction(0, 1);
        public static readonly Fraction One = new Fraction(1, 1);

        /// <summary>
        /// Approximates PI with accuracy  of 1E10^-38
        /// </summary>
        public static readonly Fraction PI = new Fraction(2646693125139304345, 842468587426513207);


        /// <summary>
        /// Aproximates E with accuracy  of 1E10^-38                               
        /// </summary>
        public static readonly Fraction E = Fraction.ValueOf("10873127313836180941441149885410649991", "4000000000000000000000000000000000000");

        private BigInteger numerator;
        private BigInteger denominator;

        /// <summary>
        /// Creates a Fraction with a value equivalent to the given long number.
        /// The same as Fraction.ValueOf(number, 1).
        /// 
        /// </summary>
        /// <param name="number">The original value</param>
        /// <returns>The rational equivalent to the given value</returns>
        public static Fraction ValueOf(long number)
        {
            return new Fraction(number, 1);
        }

        /// <summary>
        /// Creates a Fraction with a value equivalent to the given decimal number.
        /// 
        /// number is checked to be whole. If it is, then this equivalent to 
        /// Fraction.ValueOf(number.ToString())
        /// 
        /// If number is not a whole number, the system will determine the power of 10 that multiplied be 
        /// the given value turn the value to a hole number. Then return a fraction that is that whole number 
        /// divided by the determined power of ten.
        /// 
        /// Example: for the 0.20 value the power used would be 100 so that 
        /// 
        /// 20 = 100 * 0.20
        /// 
        /// The the fraction would then be  20/ 100 that is equivalent to 2/10 that is equivalent to 1/5. 
        /// So this method would finally return 1/5. 
        /// 
        /// For irrational number like Math.PI the same algorithm applies since double and decimal are only able to represent approximated
        /// values of rational numbers. For the cases of PI and E , if you are interested in better approximations,
        /// please consider using Fraction.PI and Fraction.E constants
        /// 
        /// </summary>
        /// <param name="number">The original value</param>
        /// <returns>The rational equivalent to the given value</returns>
        public static Fraction ValueOf(decimal number)
        {
            if (number.IsInteger())
            {
                // the decimal is really an integer
                return new Fraction(new BigInteger(number), 1);
            }

            return FromDecimalStringLiteral(new DecimalConverter().ConvertToInvariantString(number));
        }

        /// <summary>
        /// Creates a Fraction with a value equivalent to the given decimal number represented in the given string.
        /// </summary>
        /// <param name="number">The original value</param>
        /// <returns>The rational equivalent to the given value</returns>
        public static Fraction ValueOf(string number)
        {
            return FromDecimalStringLiteral(number);
        }


        /// <summary>
        /// Creates a Fraction with a value equivalent to the given double number.
        /// 
        /// Please avoid using this method whenever possible. Prefer the overloads that use decimal, ints or string.
        /// </summary>
        /// <param name="number">The original value</param>
        /// <returns>The rational equivalent to the given value</returns>
        public static Fraction ValueOf(double number)
        {
            if (number.IsInteger())
            {
                // the double is really an integer
                return new Fraction(new BigInteger(number), 1);
            }
            else
            {
                return FromDecimalStringLiteral(new DoubleConverter().ConvertToInvariantString(number));
            }
        }

        private static Fraction FromDecimalStringLiteral(string str)
        {
            int pos = str.IndexOf('E');
            if (pos < 0)
            {
                pos = str.IndexOf('.');
                if (pos < 0)
                {
                    return new Fraction(BigInteger.Parse(str), 1);
                }
                else
                {
                    var builder = new StringBuilder(str.Replace(".", ""));
                    while (builder.Length > 0 && builder[0] == '0')
                    {
                        builder.Remove(0, 1);
                    }

                    if (builder.Length == 0)
                    {
                        return Fraction.Zero;
                    }

                    var expo = str.Length - pos - 1;
                    var exponent = BigInteger.Pow(new BigInteger(10), expo);

                    return new Fraction(BigInteger.Parse(builder.ToString()), exponent);
                }
            }
            else
            {
                var sign = str.Substring(pos + 1, 1).Equals("+") ? 1 : -1;
                if (sign != -1)
                {
                    throw new ArgumentException("Only negative exponent representation is supported");
                }
                var exponent = int.Parse(str.Substring(pos + 2));

                var expower = BigInteger.Pow(new BigInteger(10), exponent);
                str = str.Substring(0, pos).Replace(".", "");

                return new Fraction(BigInteger.Parse(str), expower);
            }

        }

        /// <summary>
        /// Creates a Fraction from the values in the string arguments.
        /// The strings must parse to valid whole values.
        /// This method is entended to be use when the values do not fit a long
        /// </summary>
        /// <param name="numerator"></param>
        /// <param name="denominator"></param>
        /// <returns></returns>
        public static Fraction ValueOf(string numerator, string denominator)
        {
            return ValueOf(BigInteger.Parse(numerator), BigInteger.Parse(denominator));
        }

        /// <summary>
        /// Creates a Fraction from the given values in the form numerator/denominator.
        /// 
        /// </summary>
        /// <param name="numerator"></param>
        /// <param name="denominator"></param>
        /// <returns></returns>
        public static Fraction ValueOf(long numerator, long denominator)
        {
            return ValueOf(new BigInteger(numerator), new BigInteger(denominator));
        }

        private static Fraction ValueOf(BigInteger numerator, BigInteger denominator)
        {
            if (denominator == 0)
            {
                throw new ArithmeticException("Cannot create a fraction with denominator zero");
            }
            else if (numerator == 0)
            {
                return Zero;
            }
            else if (numerator == denominator)
            {
                return One;
            }
            return new Fraction(numerator, denominator);
        }

        private Fraction(BigInteger numerator, BigInteger denominator)
        {

            if (numerator == 0)  // all forms of zero are alike.
            {
                denominator = 1;
            }
            else if (numerator == denominator)
            { // all forms of one are alike.
                numerator = 1;
                denominator = 1;
            }
            else if (denominator != 1)
            {
                var gcd = BigInteger.GreatestCommonDivisor(numerator, denominator);
                numerator /= gcd;
                denominator /= gcd;
            }

            // if negative , change sign in denominator
            if (denominator.Sign < 0)
            {
                //move negative sign to numerator
                numerator = -numerator;
                denominator = -denominator;
            }

            this.numerator = numerator;
            this.denominator = denominator;

        }

        /// <summary>
        /// Determines if the Fraction is zero, i.e. if this.Equals(Fraction.Zero) will return true.
        /// </summary>
        /// <returns>true if the Fraction is zero, false otherwise</returns>
        public bool IsZero()
        {
            return this.numerator == 0 || this.denominator == 0;
        }

        /// <summary>
        /// Determines if the Fraction is one, i.e. if this.Equals(Fraction.One) will return true.
        /// </summary>
        /// <returns>true if the Fraction is one, false otherwise</returns>
        public bool IsOne()
        {
            return this.numerator.CompareTo(this.denominator) == 0;
        }

        /// <summary>
        /// Multiplies this fraction by another.
        /// </summary>
        /// <param name="other">other fraction to multiply</param>
        /// <returns>the product of the fractions</returns>
        public Fraction Times(Fraction other)
        {
            return Multiply(this, other);
        }

        /// <summary>
        /// Multiplies this fraction by a decimal value.
        /// </summary>
        /// <param name="other">value to multiply</param>
        /// <returns>the product of the fractions</returns>
        public Fraction Times(decimal other)
        {
            return Multiply(this, Fraction.ValueOf(other));
        }

        /// <summary>
        /// Returns this fraction with the signal changed, i.e. return -this
        /// 
        /// </summary>
        /// <returns>-this</returns>
        public Fraction Negate()
        {
            return new Fraction(-this.numerator, this.denominator);
        }

        /// <summary>
        ///  Inverts the fraction. Fraction a/b will be b/a. 
        ///  An ArithmeticException will be thrown if this.IsZero is true.
        /// </summary>
        /// <returns></returns>
        public Fraction Invert()
        {
            if (this.IsZero())
            {
                throw new ArithmeticException("Cannot invert zero");
            }
            return new Fraction(this.denominator, this.numerator);
        }

        private static BigInteger LCM(BigInteger left, BigInteger right)
        {
            // LCM = |a.b| / GCD(a,b) = (|a| / GCD(a,b)) . |b|
            // https://en.wikipedia.org/wiki/Least_common_multiple

            if (left.Sign < 0)
            {
                left = -left;
            }
            if (right.Sign < 0)
            {
                right = -right;
            }
            return (left / BigInteger.GreatestCommonDivisor(left, right)) * right;

        }

        #region Operators

        public static bool operator ==(Fraction left, Fraction right)
        {
            return left.CompareTo(right) == 0;
        }

        public static bool operator !=(Fraction left, Fraction right)
        {
            return left.CompareTo(right) != 0;
        }

        public static Fraction operator *(Fraction left, Fraction right)
        {
            return Multiply(left, right);
        }

        public static Fraction operator *(Fraction left, int right)
        {
            return MultiplyInt(left, right);
        }

        private static Fraction MultiplyInt(Fraction left, int right)
        {
            // r = a/b * n = (a*n)/b
            var dleft = left.Define();

            checked
            {
                return new Fraction(dleft.numerator * right, dleft.denominator);
            }
        }

        private static Fraction DivideInt(Fraction left, int right)
        {
            // r = (a/b) / n = a/(b*n)
            var dleft = left.Define();

            checked
            {
                return new Fraction(dleft.numerator, dleft.denominator * right);
            }
        }

        private static Fraction DivideIntInverse(int right, Fraction left)
        {
            // r = n / (a/b ) = (n * b) /a
            var dleft = left.Define();

            checked
            {
                return new Fraction(dleft.denominator * right, dleft.numerator);
            }
        }

        public static Fraction operator *(int left, Fraction right)
        {
            return MultiplyInt(right, left);
        }

        public static decimal operator *(Fraction left, decimal right)
        {
            return left.ToDecimal() * right;
        }

        /// <summary>
        /// Performs division. If the right side is zero an ArithmeticException will be thrown
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Fraction operator /(Fraction left, Fraction right)
        {
            return Divide(left, right);
        }

        /// <summary>
        /// Performs division. If the right side is zero an ArithmeticException will be throw
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Fraction operator /(Fraction left, int right)
        {
            return DivideInt(left, right);
        }

        /// <summary>
        /// Performs division. If the right side is zero an ArithmeticException will be thrown
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Fraction operator /(int left, Fraction right)
        {
            if (left == 1)
            {
                return right.Invert();
            }
            return DivideIntInverse(left, right);
        }

        private static Fraction Divide(Fraction left, Fraction right)
        {
            var dleft = left.Define();
            var dright = right.Define();

            if (dright.IsZero())
            {
                throw new ArithmeticException("Cannot divide by zero");
            }

            checked
            {
                var numerator = dleft.numerator * dright.denominator; // crossed multiplication
                var denominator = dleft.denominator * dright.numerator; // crossed multiplication

                return new Fraction(numerator, denominator);
            }
        }

        private static Fraction Multiply(Fraction left, Fraction right)
        {
            var dleft = left.Define();
            var dright = right.Define();

            checked
            {
                var numerator = dleft.numerator * dright.numerator;
                var denominator = dleft.denominator * dright.denominator;

                return new Fraction(numerator, denominator);
            }
        }

        /// <summary>
        /// Increment the fraction by one
        /// </summary>
        /// <param name="value"> fraction to increment</param>
        /// <returns>this + 1</returns>
        public static Fraction operator ++(Fraction value)
        {
            return value.Increment();
        }

        /// <summary>
        /// Decrements the fraction by one
        /// </summary>
        /// <param name="value"> fraction to increment</param>
        /// <returns>this - 1</returns>
        public static Fraction operator --(Fraction value)
        {
            return value.Decrement();
        }

        /// <summary>
        /// Increment the fraction by one
        /// </summary>
        /// <param name="value"> fraction to increment</param>
        /// <returns>this + 1</returns>
        public Fraction Increment()
        {
            // (a / b) + 1 = (a+b) / b
            return new Fraction(this.numerator + this.denominator, this.denominator);
        }

        /// <summary>
        /// Increment the fraction by n
        /// </summary>
        /// <param name="n">value to increment</param>
        /// <returns>this + n</returns>
        public Fraction Increment(int n)
        {
            // (a / b) + n = (a+b*n) / b
            return new Fraction(this.numerator + (this.denominator * new BigInteger(n)), this.denominator);
        }

        /// <summary>
        /// Decrements the fraction by one
        /// </summary>
        /// <param name="value"> fraction to increment</param>
        /// <returns>this - 1</returns>
        public Fraction Decrement()
        {
            // a / b - 1 = (a-b) / b
            return new Fraction(this.numerator - this.denominator, this.denominator);
        }

        /// <summary>
        /// Decrement the fraction by n
        /// </summary>
        /// <param name="n">value to decrement</param>
        /// <returns>this - n</returns>
        public Fraction Decrement(int n)
        {
            // a / b - n = (a-b*n) / b
            return new Fraction(this.numerator - (this.denominator * new BigInteger(n)), this.denominator);
        }

        public static Fraction operator +(Fraction left, Fraction right)
        {
            return Plus(left, right);
        }

        public static Fraction operator +(Fraction left, int right)
        {
            return left.Increment(right);
        }

        public static Fraction operator +(int left, Fraction right)
        {
            return right.Increment(left);
        }

        public static Fraction operator -(Fraction left, Fraction right)
        {
            return Plus(left, right.Negate());
        }

        public static Fraction operator -(Fraction right, int left)
        {
            return right.Decrement(left);
        }

        public static Fraction operator -(int right, Fraction left)
        {
            return new Fraction(left.denominator * new BigInteger(right) - left.numerator, left.denominator);
        }

        private static Fraction Plus(Fraction left, Fraction right)
        {
            var dleft = left.Define();
            var dright = right.Define();

            if (dleft.IsZero())
            {
                return right;
            }
            else if (right.IsZero())
            {
                return left;
            }

            var lcm = LCM(dleft.denominator, dright.denominator); // cannot return less than 1
            var leftFactor = lcm / dleft.denominator;
            var righFactor = lcm / dright.denominator;

            checked
            {
                var numerator = dleft.numerator * leftFactor + dright.numerator * righFactor;

                return new Fraction(numerator, lcm);
            }

        }

        public static bool operator <(Fraction left, Fraction right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(Fraction left, Fraction right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(Fraction left, Fraction right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >=(Fraction left, Fraction right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static bool operator <(Fraction left, long right)
        {
            return left.CompareTo(new Fraction(right, 1)) < 0;
        }

        public static bool operator >(Fraction left, long right)
        {
            return left.CompareTo(new Fraction(right, 1)) > 0;
        }

        public static bool operator <=(Fraction left, long right)
        {
            return left.CompareTo(new Fraction(right, 1)) <= 0;
        }

        public static bool operator >=(Fraction left, long right)
        {
            return left.CompareTo(new Fraction(right, 1)) >= 0;
        }

        #endregion

        /// <summary>
        /// Determines if the fraction is undefined. 
        /// A fraction is undefined if the denominator is zero.
        /// </summary>
        /// <returns>true if the fraction is undefined, false otherwise</returns>
        public bool IsUndefined()
        {
            return denominator.IsZero;
        }

        /// <summary>
        /// ensures the fraction is well defined, i.e. no zero denominators, by returning Fraction.Zero
        /// if the fraction has a zero denominator. Meaning that 0/0 => 0
        /// </summary>
        /// <returns></returns>
        private Fraction Define()
        {
            return this.IsZero() ? Fraction.Zero : this;
        }

        /// <summary>
        /// Compares to fractions for order.
        /// The comparison does not convert the fraction to decimal.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Fraction other)
        {
            var dthis = this.Define();
            var dother = other.Define();

            checked
            {
                var leftScale = dthis.numerator * dother.denominator;
                var rightScale = dthis.denominator * dother.numerator;

                if (leftScale < rightScale)
                    return -1;
                else if (leftScale > rightScale)
                    return 1;
                else
                    return 0;
            }

        }

        /// <summary>
        /// Converts the fraction to a decimal.
        /// The conversion my remove the precision present in the fraction.
        /// </summary>
        /// <returns></returns>
        public decimal ToDecimal()
        {
            if (this.denominator.IsOne)
            {
                return (decimal)this.numerator;
            }
            else if (this.IsZero())
            {
                return 0M;
            }
            else
            {
                try
                {
                    var nn = (decimal)this.numerator;
                    var dn = (decimal)this.denominator;

                    return nn / dn;
                }
                catch (OverflowException)
                {
                    // big integer it too large to fit a decimal 
                    // use logarithm reduction

                    int sgn = this.numerator.Sign * this.denominator.Sign;

                    var ln = BigInteger.Log(BigInteger.Abs(this.numerator));
                    var ld = BigInteger.Log(BigInteger.Abs(this.denominator));
                    return (decimal)System.Math.Exp(ln - ld) * sgn;
                }
            }
        }

        /// <summary>
        /// Converts the fraction to a double.
        /// The conversion my remove the precision present in the fraction.
        /// </summary>
        /// <returns></returns>
        public double ToDouble()
        {
            if (this.denominator.IsOne)
            {
                return (double)this.numerator;
            }
            else if (this.IsZero())
            {
                return 0d;
            }
            else
            {

                try
                {
                    var nn = (double)this.numerator;
                    var dn = (double)this.denominator;

                    return nn / dn;
                }
                catch (OverflowException)
                {
                    // big integer it too large to fit a double 
                    // use logrithm reduction

                    int sgn = this.numerator.Sign * this.denominator.Sign;

                    var ln = BigInteger.Log(BigInteger.Abs(this.numerator));
                    var ld = BigInteger.Log(BigInteger.Abs(this.denominator));
                    return System.Math.Exp(ln - ld) * sgn;
                }

            }
        }

        /// <summary>
        /// Converts the fraction to a double with the given decimals
        /// </summary>
        /// <returns></returns>
        public double ToDouble(int decimals)
        {
            return Convert.ToDouble(ToDecimal().Round(decimals, RoundingMode.RoundHalfUp));
        }

        /// <summary>
        /// Does not compare value, only representation. 4/2 and 2/1 are different fractions.
        /// However because constructions reduces the fraction, equals is compatible with value comparision.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var other = (Fraction)obj;
            return this.numerator == other.numerator && this.denominator == other.denominator;
        }

        public override int GetHashCode()
        {
            return numerator.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}/{1}", numerator, denominator);
        }

        /// <summary>
        /// Calculates the exponent power of this fraction.
        /// This is equivalent to raising he numerator and denominator to the same power.
        /// 
        /// </summary>
        /// <param name="exponent"></param>
        /// <returns></returns>
        public Fraction Pow(int exponent)
        {
            if (exponent == 0)
            {
                if (this.IsZero())
                {
                    throw new ArgumentException("Cannot calculate 0^0");
                }
                return Fraction.One;
            }
            else if (this.IsZero() && exponent < 0)
            {
                throw new ArgumentException("Cannot invert 0");
            }
            else if (this.IsZero() || this.IsOne() || exponent == 1)
            {
                return this;
            }


            if (exponent < 0)
            {
                return this.Invert().Pow(-exponent);
            }
            else
            {
                return new Fraction(BigInteger.Pow(this.numerator, exponent), BigInteger.Pow(this.denominator, exponent));
            }
        }

        /// <summary>
        /// Return the fraction with the value equivalent to the closest whole number that is greater the the value of the fraction.
        /// </summary>
        public Fraction Ceilling
        {
            get
            {
                if (numerator.Sign == -1)
                {
                    return this.Negate().Floor.Negate();
                }
                else
                {
                    //extracted from http://www.cs.nott.ac.uk/~rcb/G51MPC/slides/NumberLogic.pdf
                    return new Fraction((numerator + denominator - 1) / denominator, BigInteger.One);
                }
            }

        }

        /// <summary>
        /// Return the fraction with the value equivalent to the closest whole number that is less than the value of the fraction.
        /// </summary>
        public Fraction Floor
        {
            get
            {
                if (numerator.Sign == -1)
                {
                    return this.Negate().Ceilling.Negate();
                }
                else
                {
                    return new Fraction(numerator / denominator, BigInteger.One);
                }
            }
        }

        /// <summary>
        /// Return the int with the value equivalent to the closest whole number that is less than the value of the fraction.
        /// </summary>
        public int IntFloor
        {
            get
            {
                var n = numerator / denominator;
                if (n >= int.MinValue && n <= int.MaxValue)
                {
                    return (int)n;
                }
                else
                {
                    throw new ArithmeticException("Result does not fit into a int.");
                }
            }
        }


        /// <summary>
        /// Return the fraction with the value equivalent to the closest whole number that is greater the the value of the fraction.
        /// </summary>
        public int IntCeiling
        {
            get
            {
                var n = (numerator + denominator - 1) / denominator;
                if (n >= int.MinValue && n <= int.MaxValue)
                {
                    return (int)n;
                }
                else
                {
                    throw new ArithmeticException("Result does not fit into a int.");
                }
            }
        }

        /// <summary>
        /// Returns an array of fraction that corresponds to integer division between the numerator and denominator
        /// The position 0 in the array is the quotient of the division and position 1 in the array is the remainder, so that 
        /// 
        /// if 
        /// 
        /// f = a / b = q + r
        /// 
        /// then
        /// 
        /// f = f.QuotientAndRemainder[0] + f.QuotientAndRemainder[1]
        /// 
        /// 
        /// 
        /// </summary>
        public Fraction[] QuotientAndRemainder
        {
            get
            {
                var remainder = BigInteger.Zero;
                var quocient = BigInteger.DivRem(numerator, denominator, out remainder);
                return new[] { new Fraction(quocient, BigInteger.One), new Fraction(remainder, BigInteger.One) };
            }
        }


        /// <summary>
        /// Indicates if this fraction really represent a whole number
        /// The whole number represented may be greater than an int or a long.
        /// </summary>
        /// <returns></returns>
        public bool IsWhole()
        {
            return this.denominator.IsOne;
        }

        /// <summary>
        /// Returns the absolute value of this.
        /// </summary>
        /// <returns>The absolute value</returns>
        public Fraction Abs()
        {
            // only the numerator has signal
            return new Fraction(BigInteger.Abs(numerator), denominator);
        }

        public int Sign
        {
            get
            {
                // only the numerator has signal
                return numerator.Sign;
            }
        }
    }
}
