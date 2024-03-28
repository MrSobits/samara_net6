namespace Bars.B4.Modules.Analytics.Data
{
    using System;
    using System.Collections.Generic;
    using Bars.B4;
    using Bars.B4.Modules.Analytics.Executions;
    using Bars.B4.Modules.Analytics.Filters;

    /// <summary>
    /// Интерфейс поставщика данных. Из регистрированного поставщика данных автоматически 
    /// создается источник данных - см. <see cref="MigrateDataProvidersHandler"/>.
    /// </summary>
    public interface IDataProvider
    {
        /// <summary>
        /// Описание поставщика данных. Используется при создании источника данных.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Наименование поставщика данных.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Ключ поставщика данных, уникальный в рамках приложения.
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Предоставляет данные.
        /// </summary>
        object ProvideData(SystemFilter systemFilter, DataFilter dataFilter, BaseParams baseParams);

        /// <summary>
        /// Предоставляет тип данных, возвращаемых поставщиком.
        /// </summary>
        /// <returns></returns>
        Type ProvideMetaData();

        /// <summary>
        /// Список параметров источника данных.
        /// </summary>
        IEnumerable<DataProviderParam> Params { get; }

        /// <summary>
        /// Является ли данный поставщик скрытым.
        /// Если поставщик скрытый, то его нельзя использовать
        /// для создания хранимых источников данных.
        /// </summary>
        bool IsHidden { get; }

        /// <summary>
        /// Возвращает пример данных
        /// </summary>
        /// <returns>Пример данных</returns>
        object GetSampleData();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dpParam"></param>
        void AddParam(DataProviderParam dpParam);

    }

}
