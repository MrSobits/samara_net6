namespace Bars.Gkh.Overhaul.Tat.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Entities;

    public class VersionRecordController : B4.Alt.DataController<VersionRecord>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("VersionRecordDataExport");
            return export != null ? export.ExportData(baseParams) : null;
        }
    }
}
