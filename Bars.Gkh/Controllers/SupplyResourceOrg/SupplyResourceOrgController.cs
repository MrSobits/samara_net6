namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Entities;

    public class SupplyResourceOrgController : B4.Alt.DataController<SupplyResourceOrg>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("SupplyResourceOrgDataExport");
            return export != null ? export.ExportData(baseParams) : null;
        }
    }
}