namespace Bars.GkhRf.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using B4;

    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhRf.Entities;

    public class RequestTransferRfController : FileStorageDataController<RequestTransferRf>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("RequestTransferRfDataExport");
            return export != null ? export.ExportData(baseParams) : null;
        }
    }
}