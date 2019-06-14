using System;
using System.Collections.Generic;
using System.Linq;

namespace Impress.Math
{
    public static class NumbersExtensions
    {
        public static decimal FractionaryPart(this decimal value)
        {
            return value - Decimal.Floor(value);
        }

        public static int Sign(this decimal value)
        {
            if (value > 0M)
            {
                return 1;
            }
            else if (value == 0M)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }

        public static decimal Abs(this decimal value)
        {
            if (value >= 0M)
            {
                return value;
            }
            else
            {
                return -value;
            }
        }

        /// <summary>
        /// Faster than 2.Raise(exponent)
        /// </summary>
        /// <param name="exponent"></param>
        /// <returns></returns>
        public static int PowerOfTwo(this int exponent)
        {
            if (exponent < 0)
            {
                throw new ArithmeticException("Cannot raise to negative integers. Consider using Math.Pow(10, double).");
            }

            return 2 << exponent;
        }

        /// <summary>
        /// Faster than 10.Raise(exponent)
        /// </summary>
        /// <param name="exponent"></param>
        /// <returns></returns>
        public static int PowerOfTen(this int exponent)
        {
            if (exponent < 0)
            {
                throw new ArithmeticException("Cannot raise to negative integers. Consider using Math.Pow(10, double).");
            }

            int value = 1;
            for (int i = 0; i < exponent; i++)
            {
                value = (value << 3) + (value << 1);
            }

            return value;
        }

        public static int Raise(this int baseValue, int exponent)
        {
            if (exponent < 0)
            {
                throw new ArithmeticException("Cannot raise to negative integers. Consider using Math.Pow(double, double).");
            }
            if (baseValue == 0 && exponent == 0)
            {
                throw new ArithmeticException("Cannot raise  0 to 0 power");
            }

            int value = 1;
            for (int i = 0; i < exponent; i++)
            {
                value *= baseValue;
            }

            return value;

        }

        public static long BitCount(this long value)
        {
            //Extracted from http://books.google.com.br/books?id=iBNKMspIlqEC&pg=PA66&redir_esc=y#v=onepage&q&f=false

            value = (value & 0x5555555555555555) + ((value >> 1) & 0x5555555555555555);
            value = (value & 0x3333333333333333) + ((value >> 2) & 0x3333333333333333);
            value = (value & 0x0F0F0F0F0F0F0F0F) + ((value >> 4) & 0x0F0F0F0F0F0F0F0F);
            value = (value & 0x00FF00FF00FF00FF) + ((value >> 8) & 0x00FF00FF00FF00FF);
            value = (value & 0x0000FFFF0000FFFF) + ((value >> 16) & 0x0000FFFF0000FFFF);
            value = (value & 0x00000000FFFFFFFF) + ((value >> 32) & 0x00000000FFFFFFFF);

            return value;
        }

        public static int CeilDiv(this int numerator, int denominator)
        {
            //extracted from http://www.cs.nott.ac.uk/~rcb/G51MPC/slides/NumberLogic.pdf

            return (numerator + denominator - 1) / denominator;
        }

        public static long CeilDiv(this long numerator, long denominator)
        {
            //extracted from http://www.cs.nott.ac.uk/~rcb/G51MPC/slides/NumberLogic.pdf

            return (numerator + denominator - 1) / denominator;
        }

        public static bool IsEven(this int number)
        {
            return (number & 1) == 0;
        }

        public static bool IsOdd(this int number)
        {
            return (number & 1) == 1;
        }

        public static bool IsInteger(this decimal number)
        {
            return number % 1 == 0;
        }

        public static bool IsInteger(this double number)
        {
            return number % 1 == 0;
        }

        public static bool IsInteger(this float number)
        {
            return number % 1 == 0;
        }

        public static bool IsIn(this int number, params int[] collection)
        {
            foreach (int candidate in collection)
            {
                if (number == candidate)
                {
                    return true;
                }
            }
            return false;
        }

        public static double ToDouble(this decimal number)
        {
            return Convert.ToDouble(number);
        }

        public static double ToDoubleRoundTo(this decimal number, int decimals)
        {
            return System.Math.Round(ToDouble(number), decimals);
        }

        public static IEnumerable<int> UpTo(this int minValue, int maxValue)
        {
            return Enumerable.Range(minValue, maxValue);
        }
    }
}
