namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Entities;

    public class ConstructiveElementGroupController : B4.Alt.DataController<ConstructiveElementGroup>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("ConstructiveElementGroupDataExport");
            return export != null ? export.ExportData(baseParams) : null;
        }
    }
}