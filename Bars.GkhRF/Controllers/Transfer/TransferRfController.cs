namespace Bars.GkhRf.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhRf.DomainService;
    using Bars.GkhRf.Entities;

    public class TransferRfController : B4.Alt.DataController<TransferRf>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("TransferRfDataExport");
            return export != null ? export.ExportData(baseParams) : null;
        }

        public ActionResult Copy(BaseParams baseParams)
        {
            var copyService = Container.Resolve<ITransferRfService>();
            return copyService != null ? new JsonNetResult(copyService.Copy(baseParams)) : null;
        }
    }
}