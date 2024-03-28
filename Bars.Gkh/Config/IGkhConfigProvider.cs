namespace Bars.Gkh.Config
{
    using System;
    using System.Collections.Generic;

    using Bars.Gkh.Config.Impl.Internal;

    /// <summary>
    ///     Интерфейс провайдера конфигураций.
    /// </summary>
    public interface IGkhConfigProvider
    {
        /// <summary>
        ///     Возвращает полную карту конфигурации
        /// </summary>
        IDictionary<string, PropertyMetadata> Map { get; }

        /// <summary>
        /// Словарь хранимых значений
        /// </summary>
        IDictionary<string, ValueHolder> ValueHolders { get; }

        /// <summary>
        ///     Выполняет сопоставление загруженной конфигурации с зарегистрированными
        ///     конфигурационными секциями
        /// </summary>
        void CompleteMapping();

        /// <summary>
        ///     Возвращает корневую секцию
        /// </summary>
        /// <typeparam name="T">Тип корневой секции</typeparam>
        /// <returns>Экземпляр класса T</returns>
        T Get<T>() where T : class, IGkhConfigSection;

        /// <summary>
        ///     Возвращает корневую секцию
        /// </summary>
        /// <param name="type">
        ///     Тип секции. Должен реализовывать IGkhConfigSection
        /// </param>
        /// <returns>
        ///     Экземпляр класса type
        /// </returns>
        object Get(Type type);

        /// <summary>
        ///     Возвращает значение конфигурации по указанному полному ключу
        /// </summary>
        /// <param name="key">Полный ключ</param>
        /// <param name="defaultValue">Стандартное значение на случай, если ключ не задан</param>
        /// <typeparam name="T">Тип значения</typeparam>
        /// <returns>Значение конфигурации по указанному полному ключу</returns>
        T GetByKey<T>(string key, T defaultValue = default(T));

        /// <summary>
        ///     Возвращает значение конфигурации по указанному полному ключу
        /// </summary>
        /// <param name="key">
        ///     Полный ключ
        /// </param>
        /// <param name="defaultValue">
        ///     Стандартное значение на случай, если ключ не задан
        /// </param>
        /// <param name="type">
        ///     Тип возвращаемого значения
        /// </param>
        /// <returns>
        ///     Значение конфигурации по указанному полному ключу
        /// </returns>
        object GetByKey(string key, object defaultValue, Type type);

        /// <summary>
        ///     Загружает конфигурацию
        /// </summary>
        void LoadConfiguration();

        /// <summary>
        ///     Сохраняет внесенные изменения
        /// </summary>
        Exception SaveChanges();
    }
}