namespace Bars.Gkh.Overhaul.Tat.Controllers
{
    using System.IO;
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.B4.Modules.DataExport;
    using Bars.B4.Modules.Reports;
    using Bars.Gkh.Overhaul.Tat.DomainService;
    using Bars.Gkh.Overhaul.Tat.Entities;

    using Castle.MicroKernel;

    public class RealityObjectStructuralElementInProgrammStage3Controller : B4.Alt.DataController<RealityObjectStructuralElementInProgrammStage3>
    {
        public ILongProgramService Service { get; set; }

        public ActionResult Export(BaseParams baseParams)
        {
            var generator = Container.Resolve<IReportGenerator>("XlsIoGenerator");

            var report = Container.Resolve<IDataExportReport>("RealObjStructElemlnProgStg3DataExport", new Arguments
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

            Container.Release(report);
            Container.Release(generator);

            return new ReportStreamResult(result, "export.xlsx");
        }

        public ActionResult GetParams(BaseParams baseParams)
        {
            return new JsonNetResult(this.Service.GetParams(baseParams));
        }

        public ActionResult SetPriority(BaseParams baseParams)
        {
            var result = this.Service.SetPriority(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult ListDetails(BaseParams baseParams)
        {
            return new JsonNetResult(this.Service.ListDetails(baseParams).Data);
        }

        public ActionResult GetInfo(BaseParams baseParams)
        {
            return new JsonNetResult(this.Service.GetInfo(baseParams).Data);
        }

        public ActionResult ListWorkTypes(BaseParams baseParams)
        {
            return new JsonNetResult(this.Service.ListWorkTypes(baseParams));
        }

        public ActionResult MakeNewVersion(BaseParams baseParams)
        {
            var result = this.Service.MakeNewVersion(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult CopyFromVersion(BaseParams baseParams)
        {
            var result = Container.Resolve<IStage2Service>().CopyFromVersion(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult UpdateWorkSum(BaseParams baseParams)
        {
            return new JsonNetResult(this.Service.UpdateWorkSum(baseParams));
        }
    }
}