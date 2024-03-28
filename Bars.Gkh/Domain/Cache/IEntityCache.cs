namespace Bars.Gkh.Domain.Cache
{
    using System;
    using System.Collections.Generic;
    using NHibernate;

    /// <summary>
    ///     Интерфейс кэша сущностей
    /// </summary>
    public interface IEntityCache
    {
        /// <summary>
        ///     Очистка кэша
        /// </summary>
        void Clear();

        /// <summary>
        ///     Обновление кэша
        /// </summary>
        void Update(bool force = false);

        /// <summary>
        /// Обновление кеша с StatelessSession
        /// </summary>
        /// <param name="ss"></param>
        void UpdateStateless(IStatelessSession ss);
    }

    public interface IEntityCache<T> : IEntityCache
        where T : class
    {
        /// <summary>
        ///     Добавление сущности в кэш.
        ///     Сущность будет добавлена во внутреннюю коллекцию элементов,
        ///     также для нее будут сформирвоаны элементы во всех внутренних словарях
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        IEntityCache<T> AddEntity(params T[] entities);

        /// <summary>
        ///     Удаление сущности из всех словарей
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        IEntityCache<T> DeleteEntity(params T[] entities);

        /// <summary>
        ///     Получение записи из именованного словаря, построенного с помощью метода
        ///     <see>
        ///         <cref>MakeDictionary</cref>
        ///     </see>
        /// </summary>
        /// <param name="name">Наименование словаря</param>
        /// <param name="key">Ключ</param>
        /// <returns></returns>
        T GetByKey(string name, string key);

        /// <summary>
        ///     Получение записи из именованного словаря #Default
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns></returns>
        T GetByKey(string key);

        /// <summary>
        ///     Получение списка записей из именованного словаря по набору ключей
        /// </summary>
        /// <param name="name"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        T[] GetByKeys(string name, IEnumerable<string> keys);

        /// <summary>
        ///     Получение общего списка зарегистрированных сущностей
        /// </summary>
        /// <returns></returns>
        IList<T> GetEntities();

        /// <summary>
        ///     Регистрация словаря элементов для последующего поиска
        /// </summary>
        /// <param name="name">Имя словаря</param>
        /// <param name="keyComparer">Метод сравнения ключей</param>
        /// <param name="builder">Функция построения ключей</param>
        /// <param name="filter">Предикат, определяющий необходимость помещения элемента в словарь</param>
        /// <returns></returns>
        IEntityCache<T> MakeDictionary(
            string name,
            StringComparer keyComparer,
            Func<T, string> builder,
            Predicate<T> filter = null);

        /// <summary>
        ///     Регистрация словаря элементов для последующего поиска
        /// </summary>
        /// <param name="name">Имя словаря</param>
        /// <param name="builder">Функция построения ключей</param>
        /// <param name="filter">Предикат, определяющий необходимость помещения элемента в словарь</param>
        /// <returns></returns>
        IEntityCache<T> MakeDictionary(string name, Func<T, string> builder, Predicate<T> filter = null);

        /// <summary>
        ///     Регистрация словаря элементов для последующего поиска. В качестве имени словаря будет использован ключ "#Default"
        /// </summary>
        /// <param name="builder">Функция построения ключей</param>
        /// <param name="filter">Предикат, определяющий необходимость помещения элемента в словарь</param>
        /// <returns></returns>
        IEntityCache<T> MakeDictionary(Func<T, string> builder, Predicate<T> filter = null);

        /// <summary>
        ///     Установить время жизни кэша.
        ///     Проверяется при попытке обновления кэша в импортерах.
        /// </summary>
        /// <param name="timespan"></param>
        /// <returns></returns>
        IEntityCache<T> SetLivingTime(TimeSpan timespan);

        void UpdateDictionary(string name);

        /// <summary>
        ///     Обновление кэша сущностей.
        ///     Будут обновлены внутренние словари
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        IEntityCache<T> UpdateEntity(params T[] entities);
    }
}