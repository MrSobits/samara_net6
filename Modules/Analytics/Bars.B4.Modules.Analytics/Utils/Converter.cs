namespace Bars.B4.Modules.Analytics.Utils
{
    using Bars.B4.Utils;
    using System;
    using System.Linq;

    public static class Converter
    {
        public static long[] ToLongArray(this string value, string delimiter = ",")
        {
            if (value.IsEmpty())
            {
                return new long[0];
            }

            return value
                .Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.ToLong())
                .ToArray();
        }
    }
}

