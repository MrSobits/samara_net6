namespace Bars.GisIntegration.Base.Extensions
{
    using System;
    using System.Linq;

    using Bars.B4.Utils;

    /// <summary>
    /// Статический класс, содержащий методы работы со строкой
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Преобразовать строку к массиву Guid-ов
        /// </summary>
        /// <param name="value">Исходная строка</param>
        /// <param name="delimiter">Разделитель</param>
        /// <returns>Массив Guid-ов</returns>
        public static Guid[] ToGuidArray(this string value, string delimiter = ",")
        {
            if (value.IsEmpty())
            {
                return new Guid[0];
            }

            return value
                .Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => new Guid(x))
                .ToArray();
        }

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
