namespace Bars.B4.Modules.Analytics.Reports.Generators
{
    using System.IO;
    using Bars.B4;
    using Bars.B4.Modules.Analytics.Reports.Enums;

    /// <summary>
    /// Интерфейс менеждера для отчетов
    /// </summary>
    public interface ICodedReportManager: IReportGenerator
    {
        /// <summary>
        /// Получить шаблон для редактирования
        /// </summary>
        /// <param name="codedReport">Отчет</param>
        /// <param name="original">По умолчанию</param>
        /// <returns>Поток</returns>
        Stream ExtractTemplateForEdit(ICodedReport codedReport, bool original = false);

        /// <summary>
        /// Получить пустой шаблон
        /// </summary>
        /// <param name="codedReport">Отчет</param>
        /// <returns>Поток</returns>
        Stream GetEmptyTemplate(ICodedReport codedReport);

        /// <summary>
        /// Сгенерировать отчет
        /// </summary>
        /// <param name="codedReport">Отчет</param>
        /// <param name="baseParams">Параметры</param>
        /// <param name="printFormat">Формат печати</param>
        /// <returns>Поток</returns>
        Stream GenerateReport(ICodedReport codedReport, BaseParams baseParams, ReportPrintFormat printFormat);
    }
}
