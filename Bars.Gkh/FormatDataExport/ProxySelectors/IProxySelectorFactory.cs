namespace Bars.Gkh.FormatDataExport.ProxySelectors
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    using Bars.B4.DataModels;
    using Bars.B4.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Фабрика создания селектор-сервисов <see cref="IProxySelectorService{T}"/>
    /// </summary>
    public interface IProxySelectorFactory : IDisposable
    {
        /// <summary>
        /// IoC
        /// </summary>
        IWindsorContainer Container { get; set; }

        /// <summary>
        /// Создать или получить селектор из кэша
        /// </summary>
        /// <typeparam name="T">Тип прокси-сущности</typeparam>
        /// <exception cref="TypeLoadException">Не найдена реализация сервиса</exception>
        IProxySelectorService<T> GetSelector<T>() where T : class, IHaveId;

        /// <summary>
        /// Создать или получить селектор из кэша передав параметры
        /// </summary>
        /// <param name="selectorParams">Параметры для получения кэша</param>
        /// <param name="clear">Очистить кэш, если селектор создан</param>
        /// <typeparam name="T">Тип прокси-сущности</typeparam>
        /// <exception cref="TypeLoadException">Не найдена реализация сервиса</exception>
        IProxySelectorService<T> GetSelector<T>(DynamicDictionary selectorParams, bool clear = false) where T : class, IHaveId;

        /// <summary>
        /// Освободить связанные с селектором ресурсы
        /// </summary>
        /// <typeparam name="T">Тип прокси-сущности</typeparam>
        void DisposeSelector<T>() where T : class, IHaveId;

        /// <summary>
        /// Установить параметры по умолчанию
        /// </summary>
        /// <param name="selectorParams"></param>
        void SetDefaultSelectorParams(DynamicDictionary selectorParams);

        /// <summary>
        /// Установить параметры по умолчанию
        /// </summary>
        /// <param name="entityCodes">Коды выбранных сущностей</param>
        void SetSelectedEntityCodes(IEnumerable<string> entityCodes);

        /// <summary>
        /// Коллекция дополнительных идентификаторов типа
        /// </summary>
        ConcurrentDictionary<Type, HashSet<long>> AdditionalProxyIds { get; }

        /// <summary>
        /// Коллекция выбранных пользователем секций
        /// </summary>
        ICollection<string> SelectedEntityCodes { get; }
    }
}