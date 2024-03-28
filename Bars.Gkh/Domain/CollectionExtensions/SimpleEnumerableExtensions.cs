namespace Bars.Gkh.Domain.CollectionExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.Utils;

    /// <summary>
    /// Методы расширения <see cref="IEnumerable{T}"/>
    /// </summary>
    public static class SimpleEnumerableExtensions
    {
        /// <summary>
        /// Безопасная сумма.
        /// <remarks>Возвращает 0, если коллекция пустая или равна null</remarks>
        /// </summary>
        /// <typeparam name="T">Тип коллекции</typeparam>
        /// <param name="collection">Коллекция</param>
        /// <param name="func">Функция преобразования, применяемая к каждому элементу.</param>
        /// <returns>Результат</returns>
        public static decimal SafeSum<T>(this IEnumerable<T> collection, Func<T, decimal> func)
        {
            if (collection.IsNull())
                return 0;

            return collection.Any() ? collection.Sum(func) : 0;
        }

        /// <summary>
        /// Безопасная сумма для коллекции типа <see cref="decimal"/>.
        /// <remarks>Возвращает 0, если коллекция пустая или равна null</remarks>
        /// </summary>
        /// <param name="collection">Коллекция</param>
        /// <returns>Результат</returns>
        public static decimal SafeSum(this IEnumerable<decimal> collection)
        {
            if (collection.IsNull())
                return 0;

            return collection.Any() ? collection.Sum() : 0;
        }

        /// <summary>
        /// Безопасный поиск максимального элемента
        /// <remarks>Возвращает default(TResult), если коллекция пустая или равна null</remarks>
        /// </summary>
        /// <typeparam name="T">Тип коллекции</typeparam>
        /// <typeparam name="TResult">Тип выходного результата</typeparam>
        /// <param name="collection">Коллекция</param>
        /// <param name="func">Функция преобразования, применяемая к каждому элементу.</param>
        /// <returns>Результат типа <typeparamref name="TResult"/></returns>
        public static TResult SafeMax<T, TResult>(this IEnumerable<T> collection, Func<T, TResult> func)
        {
            if (collection.IsNull())
                return default(TResult);

            return collection.Any() ? collection.Max(func) : default(TResult);
        }

        /// <summary>
        /// Безопасный поиск минимального элемента
        /// <remarks>Возвращает default(TResult), если коллекция пустая или равна null</remarks>
        /// </summary>
        /// <typeparam name="T">Тип коллекции</typeparam>
        /// <typeparam name="TResult">Тип выходного результата</typeparam>
        /// <param name="collection">Коллекция</param>
        /// <param name="func">Функция преобразования, применяемая к каждому элементу.</param>
        /// <param name="defaultValue">Значение по умолчанию, возвращаемое при пустой или null коллекции</param>
        /// <returns>Результат</returns>
        public static TResult SafeMin<T, TResult>(this IEnumerable<T> collection, Func<T, TResult> func, TResult defaultValue = default(TResult))
        {
            if (collection.IsNull())
                return defaultValue;

            return collection.Any() ? collection.Min(func) : defaultValue;
        }

        #region Avg

        /// <summary>
        /// Безопасное вычисление среднего для последовательности значений <see cref="int"/>.
        /// <remarks>Возвращает 0, если коллекция пустая или равна null</remarks>
        /// </summary>
        /// <typeparam name="T">Тип коллекции</typeparam>
        /// <param name="collection">Коллекция</param>
        /// <param name="func">Функция преобразования, применяемая к каждому элементу.</param>
        /// <returns>Результат</returns>
        public static double SafeAverage<T>(this IEnumerable<T> collection, Func<T, int> func)
        {
            if (collection.IsNull())
                return 0;

            return collection.Any() ? collection.Average(func) : 0;
        }

        /// <summary>
        /// Безопасное вычисление среднего для последовательности значений <see cref="double"/>.
        /// <remarks>Возвращает 0, если коллекция пустая или равна null</remarks>
        /// </summary>
        /// <typeparam name="T">Тип коллекции</typeparam>
        /// <param name="collection">Коллекция</param>
        /// <param name="func">Функция преобразования, применяемая к каждому элементу.</param>
        /// <returns>Результат</returns>
        public static double SafeAverage<T>(this IEnumerable<T> collection, Func<T, double> func)
        {
            if (collection.IsNull())
                return 0;

            return collection.Any() ? collection.Average(func) : 0;
        }

        /// <summary>
        /// Безопасное вычисление среднего для последовательности значений <see cref="long"/>.
        /// <remarks>Возвращает 0, если коллекция пустая или равна null</remarks>
        /// </summary>
        /// <typeparam name="T">Тип коллекции</typeparam>
        /// <param name="collection">Коллекция</param>
        /// <param name="func">Функция преобразования, применяемая к каждому элементу.</param>
        /// <returns>Результат</returns>
        public static double SafeAverage<T>(this IEnumerable<T> collection, Func<T, long> func)
        {
            if (collection.IsNull())
                return 0;

            return collection.Any() ? collection.Average(func) : 0;
        }

        /// <summary>
        /// Безопасное вычисление среднего для последовательности значений <see cref="decimal"/>.
        /// <remarks>Возвращает 0, если коллекция пустая или равна null</remarks>
        /// </summary>
        /// <typeparam name="T">Тип коллекции</typeparam>
        /// <param name="collection">Коллекция</param>
        /// <param name="func">Функция преобразования, применяемая к каждому элементу.</param>
        /// <returns>Результат</returns>
        public static decimal SafeAverage<T>(this IEnumerable<T> collection, Func<T, decimal> func)
        {
            if (collection.IsNull())
                return 0;

            return collection.Any() ? collection.Average(func) : 0;
        }

        #endregion Avg

        /// <summary>
        /// Делить последовательность на подпоследовательности размером <paramref name="length"/>.
        /// <remarks>Последняя подпоследовательность может быть меньше,чем указанное значение <paramref name="length"/></remarks>
        /// </summary>
        /// <typeparam name="T">Тип коллекции</typeparam>
        /// <param name="source">Входная последовательность</param>
        /// <param name="length">Размер подпоследовательности</param>
        /// <exception cref="ArgumentOutOfRangeException">Исключение возникает, если указанный параметр <paramref name="length"/> меньше или равен нулю</exception>
        /// <returns>Последовательность подпоследовательностей</returns>
        public static IEnumerable<IEnumerable<T>> Section<T>(this IEnumerable<T> source, int length)
        {
            if (length <= 0) throw new ArgumentOutOfRangeException("length");

            var section = new List<T>(length);

            foreach (var item in source)
            {
                section.Add(item);

                if (section.Count == length)
                {
                    yield return section.AsReadOnly();
                    section = new List<T>(length);
                }
            }

            if (section.Count > 0) yield return section.AsReadOnly();
        }
    }
}