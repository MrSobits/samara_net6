namespace Bars.Gkh.FormatDataExport.Domain
{
    using System.Collections.Generic;

    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;

    /// <summary>
    /// Сервис получения инкрементальных данных при экспорте по формату
    /// </summary>
    public interface IFormatDataExportIncrementalService
    {
        /// <summary>
        /// Задать игнорируемые секции
        /// </summary>
        /// <param name="entityCodeList">Список кодов сущностей</param>
        void SetIgnoreEtities(IList<string> entityCodeList);

        /// <summary>
        /// Получить инкрементальные данные
        /// </summary>
        /// <param name="entityCode">Код сущности</param>
        /// <param name="data">Коллекция строк</param>
        IList<ExportableRow> GetIncrementalData(string entityCode, IList<ExportableRow> data);

        /// <summary>
        /// Сохранить информацию о новых данных
        /// </summary>
        void SaveNewDataInfo();
    }
}