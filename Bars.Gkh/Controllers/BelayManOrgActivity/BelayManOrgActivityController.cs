namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Entities;

    public class BelayManOrgActivityController : B4.Alt.DataController<BelayManOrgActivity>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("BelayManOrgActivityDataExport");
            return export != null ? export.ExportData(baseParams) : null;
        }
    }
}