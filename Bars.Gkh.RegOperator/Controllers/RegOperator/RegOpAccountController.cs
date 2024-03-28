namespace Bars.Gkh.RegOperator.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.RegOperator.DomainService;

    public class RegOpAccountController : BaseController
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("RegOpAccountExport");
            return export != null ? export.ExportData(baseParams) : null;
        }
    }
}