namespace Bars.Gkh.FormatDataExport.ProxySelectors
{
    using System;
    using System.Collections.Generic;

    using Bars.B4.DataModels;
    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Domain;

    /// <summary>
    /// Сервис получения прокси-объекта
    /// </summary>
    public interface IProxySelectorService<T> : IDisposable
        where T : IHaveId
    {
        /// <summary>
        /// Параметры для получения кэша
        /// </summary>
        DynamicDictionary SelectParams { get; }

        /// <summary>
        /// Получить кэшированный список прокси-объектов
        /// </summary>
        [Obsolete("Use ExtProxyListCache")]
        IDictionary<long, T> ProxyListCache { get; }

        /// <summary>
        /// Получить расширенный список прокси-объектов состоящий из кэша и дополнительных сущностей
        /// </summary>
        ICollection<T> ExtProxyListCache { get; }

        /// <summary>
        /// Получить кэшированный список идентификаторов прокси-объектов
        /// </summary>
        ICollection<long> ProxyListIdCache { get; }

        /// <summary>
        /// Получить список прокси-объектов
        /// </summary>
        IDictionary<long, T> GetProxyList();

        /// <summary>
        /// Очистить кэш
        /// </summary>
        void Clear();

        /// <summary>
        /// Фабрика создания селектор-сервисов
        /// </summary>
        IProxySelectorFactory ProxySelectorFactory { get; }

        /// <summary>
        /// Сервис фильтрации
        /// </summary>
        IFormatDataExportFilterService FilterService { get; }
    }
}