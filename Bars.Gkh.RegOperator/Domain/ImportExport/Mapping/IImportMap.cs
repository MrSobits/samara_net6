namespace Bars.Gkh.RegOperator.Domain.ImportExport.Mapping
{
    using System;
    using System.Collections.Generic;
    using Enums;

    /// <summary>
    /// Мап импорта
    /// </summary>
    public interface IImportMap
    {
        /// <summary>
        /// Тип объекта
        /// </summary>
        Type ObjectType { get; }

        /// <summary>
        /// Получить мап
        /// </summary>
        /// <returns></returns>
        Dictionary<string, ProviderMapper> GetMap();

        /// <summary>
        /// Код поставщика
        /// </summary>
        string ProviderCode { get; }

        /// <summary>
        /// Имя поставщика
        /// </summary>
        string ProviderName { get; }

        /// <summary>
        /// Формат
        /// </summary>
        string Format { get; }

        /// <summary>
        /// Имя
        /// </summary>
        /// <returns></returns>
        string GetName();

        /// <summary>
        /// Ключ прав доступа
        /// </summary>
        string PermissionKey { get; }

        /// <summary>
        /// Направление (импорт/экспорт)
        /// </summary>
        ImportExportType Direction { get; }
    }
}