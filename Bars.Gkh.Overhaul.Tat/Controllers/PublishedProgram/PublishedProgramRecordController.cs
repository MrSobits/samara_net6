namespace Bars.Gkh.Overhaul.Tat.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Overhaul.Tat.DomainService;
    using Bars.Gkh.Overhaul.Tat.Entities;

    public class PublishedProgramRecordController : B4.Alt.DataController<PublishedProgramRecord>
    {
        public IPublishProgramService PublishedProgramService { get; set; }

        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("PublishedProgramRecordExport");
            return export != null ? export.ExportData(baseParams) : null;
        }

        public ActionResult PublishedProgramMunicipalityList(BaseParams baseParams)
        {
            var result = PublishedProgramService.PublishedProgramMunicipalityList(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}