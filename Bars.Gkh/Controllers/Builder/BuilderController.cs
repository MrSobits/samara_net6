namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;

    public class BuilderController : FileStorageDataController<Builder>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("BuilderDataExport");
            return export != null ? export.ExportData(baseParams) : null;
        }
    }
}