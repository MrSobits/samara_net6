namespace Bars.B4.Modules.Analytics.Reports.Generators
{
    using System.Collections.Generic;
    using System.IO;
    using Bars.B4;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.Analytics.Reports.Enums;

    /// <summary>
    /// Генератор отчётов
    /// </summary>
    public interface IReportGenerator
    {
        /// <summary>
        /// Сгенерировать отчёт
        /// </summary>
        /// <param name="report">Отчёт</param>
        /// <param name="reportTemplate">Шаблон отчёта</param>
        /// <param name="baseParams">Параметры</param>
        /// <param name="printFormat">Формат печати</param>
        /// <param name="customArgs">Дополнительные параметры</param>
        Stream Generate(
            IReport report,
            Stream reportTemplate,
            BaseParams baseParams,
            ReportPrintFormat printFormat,
            IDictionary<string, object> customArgs);
    }
}
