namespace Bars.Gkh.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Методы расширения для <see cref="Lookup{TKey,TElement}"/>
    /// </summary>
    public static class LookupExtensions
    {
        /// <summary>
        /// Получить перечень соответствий Lookup'а по ключу
        /// </summary>
        /// <param name="lookup">Исходный lookup для поиска</param>
        /// <param name="key">Ключ поиска</param>
        /// <param name="defValue">Значение по умолчанию</param>
        /// <param name="createEmptyList">Признак необходимости создания пустого списка при отсутствии значений по ключу</param>
        /// <typeparam name="TKey">Тип ключа lookup'а</typeparam>
        /// <typeparam name="TValue">Результирующий тип lookup'а</typeparam>
        public static IEnumerable<TValue> Get<TKey, TValue>(
            this ILookup<TKey, TValue> lookup,
            TKey key,
            IEnumerable<TValue> defValue = null,
            bool createEmptyList = false)
        {
            return lookup != null && lookup.Contains(key)
                ? lookup[key]
                : createEmptyList
                    ? new List<TValue>()
                    : defValue;
        }
    }
}