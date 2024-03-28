namespace Bars.B4.Modules.Analytics.Reports.Domain
{
    using System.Collections.Generic;
    using Bars.B4.Modules.Analytics.Reports.Proxies;

    /// <summary>
    /// Интерфейс панели отчетов
    /// </summary>
    public interface IReportPanelService
    {
        /// <summary>
        /// Поиск в панели отчетов
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <returns>Список отчетов, сгруппированных по категориям</returns>
        List<ReportCategory> Search(string query);
    }
}
