namespace Bars.Gkh.FormatDataExport.Domain
{
    using Bars.B4;

    /// <summary>
    /// Интерфейс сервиса экспорта в формате 4.0.X
    /// </summary>
    public interface IFormatDataExportService
    {
        /// <summary>
        /// Метод возвращает список доступных выгружаемых секций
        /// </summary>
        IDataResult ListAvailableSection(BaseParams baseParams);

        /// <summary>
        /// Получить результат загрузки
        /// </summary>
        IDataResult GetRemoteStatus(BaseParams baseParams);

        /// <summary>
        /// Запустить удаленный импорт
        /// </summary>
        IDataResult StartRemoteImport(BaseParams baseParams);

        /// <summary>
        /// Получить удаленный файл
        /// </summary>
        IDataResult GetRemoteFile(BaseParams baseParams);

        /// <summary>
        /// Обновить статус удаленного импорта
        /// </summary>
        IDataResult UpdateRemoteStatus(BaseParams baseParams);
    }
}