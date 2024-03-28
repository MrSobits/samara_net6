namespace Bars.Gkh.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;

    public static class EnumerableExtensions
    {
        /// <summary>
        /// Возвращает пустую коллекцию, если null
        /// </summary>
        public static IEnumerable<T> AllOrEmpty<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                return Enumerable.Empty<T>();
            }
            else
            {
                return source;
            }
        }

        /// <summary>
        /// Получить значение по составному ключу
        /// <para>В качестве составного ключа Tuple</para>
        /// </summary>
        /// <typeparam name="TKey1">Тип первого ключа</typeparam>
        /// <typeparam name="TKey2">Тип второго ключа</typeparam>
        /// <typeparam name="TValue">Тип возвращаемого значения</typeparam>
        /// <param name="dictionary">Словарь</param>
        /// <param name="key1">Первый ключ</param>
        /// <param name="key2">Второй ключ</param>
        /// <returns>Возвращаемое значение</returns>
        public static TValue Get<TKey1, TKey2, TValue>(this IDictionary<Tuple<TKey1, TKey2>, TValue> dictionary, TKey1 key1, TKey2 key2)
        {
            return dictionary.Get(Tuple.Create(key1, key2));
        }

        /// <summary>
        /// Добавить элемент в словарь
        /// <para>В качестве составного ключа Tuple</para>
        /// </summary>
        /// <typeparam name="TKey1">Тип первого ключа</typeparam>
        /// <typeparam name="TKey2">Тип второго ключа</typeparam>
        /// <typeparam name="TValue">Тип добавляемого элемента</typeparam>
        /// <param name="dictionary">Словарь</param>
        /// <param name="key1">Первый ключ</param>
        /// <param name="key2">Второй ключ</param>
        /// <param name="value">Добавляемый элемент</param>
        public static void Add<TKey1, TKey2, TValue>(this IDictionary<Tuple<TKey1, TKey2>, TValue> dictionary, TKey1 key1, TKey2 key2, TValue value)
        {
            dictionary.Add(Tuple.Create(key1, key2), value);
        }

        /// <summary>
        /// Содержит ли словарь указанный ключ
        /// <para>В качестве составного ключа Tuple</para>
        /// </summary>
        /// <typeparam name="TKey1">Тип первого ключа</typeparam>
        /// <typeparam name="TKey2">Тип второго ключа</typeparam>
        /// <typeparam name="TValue">Тип возвращаемого значения</typeparam>
        /// <param name="dictionary">Словарь</param>
        /// <param name="key1">Первый ключ</param>
        /// <param name="key2">Второй ключ</param>
        /// <returns>Возвращаемое значение</returns>
        public static bool ContainsKey<TKey1, TKey2, TValue>(this IDictionary<Tuple<TKey1, TKey2>, TValue> dictionary, TKey1 key1, TKey2 key2)
        {
            return dictionary.ContainsKey(Tuple.Create(key1, key2));
        }
    }
}
