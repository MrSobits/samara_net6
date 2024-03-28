namespace Bars.B4.Modules.Analytics.Data
{
    using System;
    using System.Collections.Generic;
    using Bars.B4;
    using Bars.B4.Modules.Analytics.Filters;

    /// <summary>
    /// Интерфейс источника данных
    /// </summary>
    public interface IDataSource
    {
        /// <summary>
        /// Имя источника данных. Используется для показа пользователю.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Описание источника данных.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Фильтр.
        /// </summary>
        DataFilter DataFilter { get; }

        /// <summary>
        /// Системный фильтр.
        /// </summary>
        SystemFilter SystemFilter { get; }

        /// <summary>
        /// Получение данных.
        /// </summary>
        /// <returns></returns>
        object GetData(BaseParams baseParams);

        /// <summary>
        /// Получение метаданных.
        /// </summary>
        /// <returns></returns>
        Type GetMetaData();

        /// <summary>
        /// Список параметров поставщика данных.
        /// </summary>
        IEnumerable<IParam> Params { get; }

        /// <summary>
        /// Возвращает тестовые данные
        /// </summary>
        /// <returns></returns>
        object GetSampleData();
    }
}
