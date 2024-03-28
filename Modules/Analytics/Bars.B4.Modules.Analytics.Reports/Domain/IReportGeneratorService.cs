namespace Bars.B4.Modules.Analytics.Reports.Domain
{
    using Bars.B4.Modules.Analytics.Reports.Entities;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Utils;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// »нтерфейс сервиса генерации отчЄтов
    /// </summary>
    public interface IReportGeneratorService
    {
        /// <summary>
        /// —генерировать печать
        /// </summary>
        /// <param name="baseParams">ѕараметры запроса</param>
        /// <returns>–езультат запроса</returns>
        ReportResult Generate(BaseParams baseParams);

        /// <summary>
        /// ќбработать формирование отчета (сгенерировать на месте или отправить на сервер расчетов)
        /// </summary>
        /// <param name="baseParams">ѕараметры запроса</param>
        /// <returns>–езультат запроса</returns>
        IDataResult CreateTaskOrSaveOnServer(BaseParams baseParams);

        /// <summary>
        /// —охранить на сервер
        /// </summary>
        /// <param name="baseParams">ѕараметры запроса</param>
        /// <param name="report">ѕараметры запроса</param>
        /// <param name="format">ѕараметры запроса</param>
        /// <returns>‘айл отчЄта</returns>
        IDataResult<FileInfo> SaveOnServer(BaseParams baseParams, StoredReport report, ReportPrintFormat format);
    }
}