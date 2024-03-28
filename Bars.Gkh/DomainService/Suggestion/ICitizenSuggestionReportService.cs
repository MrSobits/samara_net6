namespace Bars.Gkh.DomainService
{
    using System.IO;

    using Bars.B4;

    /// <summary>
    /// Сервис для печати отчетов обращений граждан
    /// </summary>
    public interface ICitizenSuggestionReportService
    {
        /// <summary>
        /// Напечатать Обращение граждан с (портала)
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        Stream PrintSuggestionPortalReport(BaseParams baseParams);
    }
}