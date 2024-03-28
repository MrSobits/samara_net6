namespace Bars.GisIntegration.Base.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Bars.B4.Utils.Annotations;

    public static class EnumerableExtensions
    {
        /// <summary>
        /// Метод преобразует коллекцию в HashSet
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="source">Источник</param>
        /// <returns>HashSet.T</returns>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }

        /// <summary>
        /// Метод объединения строк через разделитель
        /// </summary>
        public static string AggregateWithSeparator(this IEnumerable<string> source, string separator)
        {
            var result = new StringBuilder();

            foreach (var current in source)
            {
                if (!string.IsNullOrWhiteSpace(current))
                {
                    if (result.Length > 0)
                    {
                        result.Append(separator);
                    }

                    result.Append(current);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Метод объединения строк через разделитель
        /// </summary>
        public static string AggregateWithSeparator<T>(this IEnumerable<T> source, Func<T, string> selector, string separator)
        {
            return EnumerableExtensions.AggregateWithSeparator(source.Select(selector), separator);
        }

        /// <summary>
        /// Метод объединения строк через разделитель
        /// </summary>
        public static string AggregateWithSeparator<T>(this IEnumerable<T> source, Func<T, object> selector, string separator)
        {
            var result = new StringBuilder();

            foreach (var current in source)
            {
                var value = selector(current);

                if (value != null)
                {
                    if (result.Length > 0)
                    {
                        result.Append(separator);
                    }

                    result.Append(value);
                }
            }

            return result.ToString();
        }

        public static IEnumerable<T[]> SplitArray<T>(this T[] source, int portionCount = 1000)
        {
            if (source == null)
            {
                throw new ArgumentException("Source cannot be null");
            }

            var length = source.Length;

            for (int i = 0; i < source.Length; i += portionCount)
            {
                yield return source.Skip(i).Take(length - i <= portionCount ? length - i : portionCount).ToArray();
            }
        }

        /// <summary>
        /// Разделяет список на порции определенного в PortionSize размера
        /// </summary>
        /// <typeparam name="T">Тип элемента списка</typeparam>
        /// <param name="sourceList">Исходный список</param>
        /// <param name="portionSize">Размер порции</param>
        /// <returns>Список порций</returns>
        public static List<IEnumerable<T>> GetPortions<T>(this List<T> sourceList, int portionSize)
        {
            if (sourceList == null)
            {
                throw new ArgumentException("Source list cannot be null");
            }

            var startIndex = 0;
            var result = new List<IEnumerable<T>>();

            do
            {
                result.Add(sourceList.Skip(startIndex).Take(portionSize));
                startIndex += portionSize;
            }
            while (startIndex < sourceList.Count);

            return result;
        }

        /// <summary>
        /// Вернуть первое непустое значение
        /// </summary>
        /// <param name="source"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string Coalesce(this IEnumerable<string> source, string defaultValue = null)
        {
            ArgumentChecker.NotNull(source, "source");

            foreach (var item in source)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    return item;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Вернуть первое непустое значение
        /// </summary>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string Coalesce<T>(this IEnumerable<T> source, Func<T, string> selector, string defaultValue = null)
        {
            ArgumentChecker.NotNull(source, "source");
            ArgumentChecker.NotNull(selector, "selector");

            return source.Select(selector).Coalesce(defaultValue);
        }

        /// <summary>
        /// Вернуть первое непустое значение
        /// </summary>
        /// <param name="source"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T Coalesce<T>(this IEnumerable<T> source, T defaultValue = null) where T : class
        {
            ArgumentChecker.NotNull(source, "source");

            foreach (var item in source)
            {
                if (item != null)
                {
                    return item;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Вернуть первое непустое значение
        /// </summary>
        /// <param name="source"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T? Coalesce<T>(this IEnumerable<T?> source, T? defaultValue = null) where T : struct
        {
            ArgumentChecker.NotNull(source, "source");

            foreach (var item in source)
            {
                if (item != null)
                {
                    return item;
                }
            }

            return defaultValue;
        }
    }
}