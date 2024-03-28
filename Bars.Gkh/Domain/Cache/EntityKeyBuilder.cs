namespace Bars.Gkh.Domain.Cache
{
    using System;
    using B4.DataAccess;

    /// <summary>
    /// Описание построителя выборки/кэша сущностей по ключу
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityKeyBuilder<T>
    {
        /// <summary>
        /// Наименование построителя ключей
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Сравнение ключей
        /// </summary>
        public StringComparer Comparer { get; private set; }

        /// <summary>
        /// Функция построения ключа
        /// </summary>
        public Func<T, string> Builder { get; private set; }

        /// <summary>
        /// Выражение для фильтрации записей
        /// </summary>
        public Predicate<T> Filter { get; private set; }

        internal EntityKeyBuilder(string name, StringComparer keyComparer, Func<T, string> builder, Predicate<T> filter = null)
        {
            Name = name;
            Comparer = keyComparer;
            Builder = builder;
            Filter = filter ?? (x => true);
        }
    }
}