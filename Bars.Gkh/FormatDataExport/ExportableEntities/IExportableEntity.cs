namespace Bars.Gkh.FormatDataExport.ExportableEntities
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;

    /// <summary>
    /// Экспортируемая сущность
    /// </summary>
    public interface IExportableEntity
    {
        /// <summary>
        /// Тип поставщика информации
        /// </summary>
        string Code { get; }

        /// <summary>
        /// Получить данные
        /// </summary>
        /// <param name="baseParams">Параметы для фильтрации данных</param>
        IDataResult<IList<List<string>>> GetData(DynamicDictionary baseParams);

        /// <summary>
        /// Получить заголовок
        /// </summary>
        IList<string> GetHeader();

        /// <summary>
        /// Незаполненные обязательные поля с группировкой по идентификатору записи
        /// </summary>
        IDictionary<long, IEnumerable<int>> EmptyMandatoryFields { get; }

        /// <summary>
        /// Версия формата
        /// </summary>
        string FormatVersion { get; }

        /// <summary>
        /// Получить коды зависимых секций
        /// </summary>
        IList<string> GetInheritedEntityCodeList();

        /// <summary>
        /// Флаги провайдеров которым разрешена выгрузка
        /// </summary>
        FormatDataExportProviderFlags AllowProviderFlags { get; }
    }
}