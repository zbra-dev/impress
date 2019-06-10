using System;

namespace Impress.Math
{
    public static class RandomExtensions
    {
        public static bool NextBoolean(this Random random)
        {
            return random.Next(2) == 1;
        }

        public static decimal NextDecimal(this Random random, decimal min, decimal max)
        {
            return min + (((decimal)random.NextDouble()) * (max - min));
        }

        public static int NextInt(this Random random, int min, int max)
        {
            return min + (int)(random.NextDouble() * (max - min));
        }

        public static int[] IntArray(this Random random, int length, int min, int max)
        {
            var array = new int[length];

            for (int i = 0; i < length; i++)
            {
                array[i] = NextInt(random, min, max);
            }

            return array;
        }
    }
}
