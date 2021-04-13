using System;

namespace Task_3_3
{
    static class ArrayExtension
    {
        public static void ForEach(this int[] array, Action<int, int> callback)
        {
            if (callback == null) { throw new ArgumentException("Callback must not be null"); }

            for (int i = 0; i < array.Length; i++)
            {
                callback(array[i], i);
            }
        }

        public static int[] Map(this int[] array, Func<int, int, int> callback)
        {
            if (callback == null) { throw new ArgumentException("Callback must not be null"); }

            int[] output = new int[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                output[i] = callback(array[i], i);
            }

            return output;
        }

        public static int Sum(this int[] array)
        {
            int sum = 0;

            for (int i = 0; i < array.Length; i++)
            {
                sum += array[i];
            }

            return sum;
        }

        public static double Average(this int[] array)
        {
            return (double)array.Sum() / array.Length;
        }

        public static int Frequent(this int[] array)
        {
            int item = array[0];
            int maxFreq = 0;

            for (int i = 0; i < array.Length; i++)
            {
                int freq = 0;

                for (int j = 0; j < array.Length; j++)
                {
                    if (array[j] == array[i]) { freq++; }
                }

                if (freq > maxFreq)
                {
                    maxFreq = freq;
                    item = array[i];
                }
            }

            return item;
        }
    }
}