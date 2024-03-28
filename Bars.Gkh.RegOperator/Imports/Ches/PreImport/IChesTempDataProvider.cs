namespace Bars.Gkh.RegOperator.Imports.Ches.PreImport
{
    using System.Collections.Generic;
    using System.IO;

    using Bars.B4;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Провайде для работы с временными таблицами импорта ЧЭС
    /// </summary>
    public interface IChesTempDataProvider
    {
        /// <summary>
        /// Файл импорта
        /// </summary>
        IPeriodImportFileInfo FileInfo { get; }

        /// <summary>
        /// Расчетный период
        /// </summary>
        ChargePeriod Period { get; }

        /// <summary>
        /// Наименование таблицы
        /// </summary>
        SchemaQualifiedObjectName TableName { get; }

        /// <summary>
        /// Наименование view с агрегированной информацией
        /// </summary>
        SchemaQualifiedObjectName SummaryViewName { get; }

        /// <summary>
        /// Наименование view с проверочными данными
        /// </summary>
        SchemaQualifiedObjectName CheckViewName { get; }

        /// <summary>
        /// Импортировать секцию
        /// </summary>
        void Import();

        /// <summary>
        /// Получить поток данных из таблицы
        /// </summary>
        /// <returns></returns>
        Stream GetOutputStream();

        /// <summary>
        /// Удалить все загруженные данные
        /// </summary>
        void DropData();

        /// <summary>
        /// Вернуть данные из таблицы
        /// </summary>
        IDictionary<string, object>[] GetData();

        /// <summary>
        /// Вернуть данные из таблицы
        /// </summary>
        IDictionary<string, object> GetSummaryData(BaseParams baseParams);

        /// <summary>
        /// Обновить view с агрегированной информацией
        /// </summary>
        void UpdateSummaryView();

        /// <summary>
        /// Обновить view с проверочными данными
        /// </summary>
        void UpdateCheckView();
    }
}