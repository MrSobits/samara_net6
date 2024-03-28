namespace Bars.Gkh.Domain
{
    using System;
    using System.Linq;
    using B4.Utils;

    public static class Converter
    {
        public static long[] ToLongArray(this string value, string delimiter = ",")
        {
            if (value.IsEmpty())
            {
                return new long[0];
            }

            return value
                .Split(new[] {delimiter}, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.ToLong())
                .ToArray();
        }

        public static string[] ToStringArray(this string value, string delimiter = ",")
        {
            if (value.IsEmpty())
            {
                return new string[0];
            }

            return value.Split(new[] {delimiter}, StringSplitOptions.RemoveEmptyEntries).ToArray();
        }
    }
}