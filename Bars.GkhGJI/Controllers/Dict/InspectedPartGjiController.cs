namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.Entities;

    public class InspectedPartGjiController : B4.Alt.DataController<InspectedPartGji>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("InspectedPartGjiDataExport");

            if (export != null)
            {
                return export.ExportData(baseParams);
            }

            return null;
        }
    }
}