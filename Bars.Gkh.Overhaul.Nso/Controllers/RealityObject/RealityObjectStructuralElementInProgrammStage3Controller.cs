namespace Bars.Gkh.Overhaul.Nso.Controllers
{
    using System.IO;
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.B4.Modules.DataExport;
    using B4.Modules.Reports;
    using Bars.Gkh.Overhaul.Nso.DomainService;
    using Bars.Gkh.Overhaul.Nso.Entities;

    using Castle.MicroKernel;

    public class RealityObjectStructuralElementInProgrammStage3Controller : B4.Alt.DataController<RealityObjectStructuralElementInProgrammStage3>
    {
        public ILongProgramService service { get; set; }

        public override ActionResult Update(BaseParams baseParams)
        {
            try
            {
                var result = DomainService.Update(baseParams);
                return new JsonNetResult(new { success = result.Success, message = result.Message, data = result.Data, summaryData = new { } });
            }
            catch (ValidationException exc)
            {
                return JsonNetResult.Failure(exc.Message);
            }
        }

        public ActionResult Export(BaseParams baseParams)
        {
            var generator = Container.Resolve<IReportGenerator>("XlsIoGenerator");

            var report = Container.Resolve<IDataExportReport>("RealObjStructElemlnProgStg3DataExport", 
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

            Container.Release(report);
            Container.Release(generator);

            return new ReportStreamResult(result, "export.xlsx");
        }

        public ActionResult GetParams(BaseParams baseParams)
        {
            return new JsonNetResult(service.GetParams(baseParams));
        }

        public ActionResult SetPriority(BaseParams baseParams)
        {
            var result = service.SetPriority(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult ListDetails(BaseParams baseParams)
        {
            return new JsonNetResult(service.ListDetails(baseParams).Data);
        }

        public ActionResult GetInfo(BaseParams baseParams)
        {
            return new JsonNetResult(service.GetInfo(baseParams).Data);
        }

        public ActionResult ListWorkTypes(BaseParams baseParams)
        {
            return new JsonNetResult(service.ListWorkTypes(baseParams));
        }

        public ActionResult MakeNewVersion(BaseParams baseParams)
        {
            return new JsonNetResult(service.MakeNewVersion(baseParams));
        }

        public ActionResult ValidationDeleteDpkr(BaseParams baseParams)
        {
            return new JsonNetResult(service.ValidationDeleteDpkr(baseParams));
        }

        public ActionResult DeleteDpkr(BaseParams baseParams)
        {
            return new JsonNetResult(service.DeleteDpkr(baseParams));
        }
    }
}