namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Entities;

    public class ConstructiveElementController : B4.Alt.DataController<ConstructiveElement>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("ConstructiveElementDataExport");
            return export != null ? export.ExportData(baseParams) : null;
        }
    }
}