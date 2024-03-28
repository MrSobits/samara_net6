namespace Bars.GkhCr.Controllers.BankStatement
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhCr.Entities;

    public class BasePaymentOrderController : B4.Alt.DataController<BasePaymentOrder>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("CRPaymentOrderDataExport");
            return export != null ? export.ExportData(baseParams) : null;
        }
    }
}