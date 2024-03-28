namespace Bars.Gkh.Regions.Tatarstan.Controller
{
    using System.IO;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport;
    using Bars.B4.Modules.Reports;
    using Bars.Gkh.Regions.Tatarstan.DomainService;
    using Bars.Gkh.Regions.Tatarstan.Entities.NormConsumption;

    using Castle.MicroKernel;

    public class NormConsumptionFiringController : B4.Alt.DataController<NormConsumptionFiring>
    {
        public INormConsumptionService Service { get; set; }

        /// <summary>
        /// Получить нормы потребление отопления
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public ActionResult ListNormConsumptionFiring(BaseParams baseParams)
        {
            int totalCount;
            var result = this.Service.GetNormConsumptionFiringQuery(baseParams, out totalCount).ToList();
            return new JsonNetResult(new { success = true, data = result, totalCount });
        }

        public ActionResult Export(BaseParams baseParams)
        {
            baseParams.Params["limit"] = 0;
            baseParams.Params["page"] = 0;

            int totalCount;
            var data = this.Service.GetNormConsumptionFiringQuery(baseParams, out totalCount).ToList();

            var generator = this.Container.Resolve<IReportGenerator>("XlsIoGenerator");

            var report = this.Container.Resolve<IDataExportReport>("NormConsumptionDataExport",
                new Arguments
                {
                    {"Data", data},
                    {"BaseParams", baseParams}
                });

            var rp = new ReportParams();

            report.PrepareReport(rp);
            var template = report.GetTemplate();

            var stream = new MemoryStream();

            generator.Open(template);
            generator.Generate(stream, rp);
            stream.Seek(0, SeekOrigin.Begin);

            var result = new ReportStreamResult(stream, "export.xls");
            return result;
        }
    }
}