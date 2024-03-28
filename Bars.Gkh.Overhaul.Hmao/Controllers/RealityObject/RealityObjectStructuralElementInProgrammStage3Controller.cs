namespace Bars.Gkh.Overhaul.Hmao.Controllers
{
    using System.Collections;
    using System.IO;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport;
    using Bars.B4.Modules.Reports;
    using Bars.Gkh.Overhaul.Hmao.DomainService;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    using Castle.MicroKernel;

    public class RealityObjectStructuralElementInProgrammStage3Controller : B4.Alt.DataController<RealityObjectStructuralElementInProgrammStage3>
    {
        public ILongProgramService Service { get; set; }

        public ActionResult Export(BaseParams baseParams)
        {
            var generator = this.Container.Resolve<IReportGenerator>("XlsIoGenerator");

            var report = this.Container.Resolve<IDataExportReport>(
                "RealObjStructElemlnProgStg3DataExport",
                new Arguments
                {
                    {"BaseParams", baseParams}
                });

            var rp = new ReportParams();

            report.PrepareReport(rp);
            var template = report.GetTemplate();

            var result = new MemoryStream();

            generator.Open(template);
            generator.Generate(result, rp);
            result.Seek(0, SeekOrigin.Begin);

            this.Container.Release(report);
            this.Container.Release(generator);

            return new ReportStreamResult(result, "export.xlsx");
        }

        public ActionResult GetParams(BaseParams baseParams)
        {
            return new JsonNetResult(this.Service.GetParams(baseParams));
        }

        /// <summary>
        /// Получение всех параметров очередности ДПКР
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат</returns>
        public ActionResult GetAllParams(BaseParams baseParams)
        {
            return new JsonNetResult(this.Service.GetAllParams(baseParams));
        }

        public ActionResult ListDetails(BaseParams baseParams)
        {
            var result = this.Container.Resolve<IStage3Service>().ListDetails(baseParams);
            return new JsonNetResult(result.Data);
        }

        public ActionResult GetInfo(BaseParams baseParams)
        {
            var result = this.Container.Resolve<IStage3Service>().GetInfo(baseParams);
            return new JsonNetResult(result.Data);
        }

        public ActionResult ListWorkTypes(BaseParams baseParams)
        {
            var result = (ListDataResult) this.Container.Resolve<IStage3Service>().ListWorkTypes(baseParams);
            return new JsonListResult((IEnumerable) result.Data, result.TotalCount);
        }

        public ActionResult ValidationDeleteDpkr(BaseParams baseParams)
        {
            return new JsonNetResult(this.Service.ValidationDeleteDpkr(baseParams));
        }

        public ActionResult DeleteDpkr(BaseParams baseParams)
        {
            return new JsonNetResult(this.Service.DeleteDpkr(baseParams));
        }
    }
}