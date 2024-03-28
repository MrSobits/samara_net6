namespace Bars.Gkh.Report
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using B4;

    /// <summary>
    /// Сервис для работы с отчетами
    /// </summary>
    public interface IGkhReportService
    {
        /// <summary>
        /// Получить список отчетов
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Список отчетов</returns>
        IList GetReportList(BaseParams baseParams);

        /// <summary>
        /// Получить отчет
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Отчет</returns>
        FileStreamResult GetReport(BaseParams baseParams);
    }
}