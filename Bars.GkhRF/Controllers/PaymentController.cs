namespace Bars.GkhRf.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhRf.Entities;
    
    public class PaymentController : B4.Alt.DataController<Payment>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("PaymentDataExport");
            return export != null ? export.ExportData(baseParams) : null;
        }
    }
}
