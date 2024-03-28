namespace Bars.B4.Modules.Analytics.Reports.Domain.History
{
    /// <summary>
    /// Интерфейс сервиса работы с историй отчетов
    /// </summary>
    public interface IReportHistoryService
    {
        /// <summary>
        /// Получить список параметров отчета для одной записи из журнала
        /// </summary>
        /// <param name="baseParams">baseParams</param>
        IDataResult ReportHistoryParamList(BaseParams baseParams);
    }
}