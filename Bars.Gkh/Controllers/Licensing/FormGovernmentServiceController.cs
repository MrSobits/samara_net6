namespace Bars.Gkh.Controllers.Licensing
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators;
    using Bars.Gkh.Entities.Licensing;
    using Bars.Gkh.Report.Licensing;

    /// <summary>
    /// Контроллер <see cref="FormGovernmentService"/>
    /// </summary>
    public class FormGovernmentServiceController : B4.Alt.DataController<FormGovernmentService>
    {
        /// <summary>
        /// Экспортировать данные
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        public ActionResult Export(BaseParams baseParams)
        {
            var reportGenerator = this.Resolve<ICodedReportManager>();
            var report = new FormGovernmentServiceReport(this.Container);

            using (this.Container.Using(reportGenerator))
            {
                return new ReportStreamResult(reportGenerator.GenerateReport(report, baseParams, ReportPrintFormat.xlsx), $"{report.Name}.xlsx");
            }
        }
    }
}