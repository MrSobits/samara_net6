namespace Bars.Gkh.FormatDataExport.ExportableEntities
{
    using System.Collections.Generic;

    using Bars.Gkh.FormatDataExport.Enums;

    /// <summary>
    /// Группа экспортируемых сущностей
    /// </summary>
    public interface IExportableEntityGroup
    {
        /// <summary>
        /// Код группы
        /// </summary>
        string Code { get; }

        /// <summary>
        /// Описание группы
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Маркер доступа к группе секций
        /// </summary>
        FormatDataExportProviderFlags AllowProviderFlags { get; }

        /// <summary>
        /// Блок сведений
        /// </summary>
        FormatDataExportType ExportType { get; }

        /// <summary>
        /// Получить коды зависимых сущностей
        /// </summary>
        IReadOnlyList<string> InheritedEntityCodeList { get; }
    }
}