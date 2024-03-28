namespace Bars.Gkh.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class LinqExtensions
    {
        /// <summary>
        /// Возвращает различающиеся элементы последовательности
        /// <para>
        /// Исключает null значения
        /// </para>
        /// </summary>
        public static IEnumerable<TSource> DistinctValues<TSource>(this IEnumerable<TSource> source)
        {
            var seenKeys = new HashSet<TSource>();
            foreach (TSource element in source.Where(x => x != null))
            {
                if (seenKeys.Add(element))
                {
                    yield return element;
                }
            }
        }
    }
}