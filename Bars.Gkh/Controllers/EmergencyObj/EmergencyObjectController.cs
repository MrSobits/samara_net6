namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;

    public class EmergencyObjectController : FileStorageDataController<EmergencyObject>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("EmergencyObjectDataExport");
            return export != null ? export.ExportData(baseParams) : null;
        }
    }
}