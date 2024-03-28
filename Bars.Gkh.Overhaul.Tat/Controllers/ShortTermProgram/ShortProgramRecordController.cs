namespace Bars.Gkh.Overhaul.Tat.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.Modules.DataExport.Domain;

    using DomainService;
    using Entities;

    public class ShortProgramRecordController : B4.Alt.DataController<ShortProgramRecord>
    {
        public IShortProgramRecordService Service { get; set; }

        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("ShortProgramRecordExport");
            return export != null ? export.ExportData(baseParams) : null;
        }

        public ActionResult CreateShortProgram(BaseParams baseParams)
        {
            var result = Service.CreateShortProgram(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult ListWork(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.ListWork(baseParams);
            return new JsonListResult((IList) result.Data, result.TotalCount);
        }

        public ActionResult AddWorks(BaseParams baseParams)
        {
            var result = Service.AddWorks(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }

        public ActionResult ActualizeVersion(BaseParams baseParams)
        {
            var result = Service.ActualizeVersion(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult ListForMassStateChange(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.ListForMassStateChange(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        public ActionResult MassStateChange(BaseParams baseParams)
        {
            var result = Service.MassStateChange(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetYears(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.GetYears(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }
    }
}