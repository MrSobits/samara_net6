namespace Bars.Gkh.Repair.Controllers
{
    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Repair.DomainService;

    using Microsoft.AspNetCore.Mvc;

    public class RepairObjectController : B4.Alt.DataController<Entities.RepairObject>
    {
        public IRepairObjectService Service { get; set; }

        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("RepairObjectDataExport");
            return export != null ? export.ExportData(baseParams) : null;
        }

        public ActionResult MassStateChange(BaseParams baseParams)
        {
            var result = Service.MassStateChange(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }
    }
}