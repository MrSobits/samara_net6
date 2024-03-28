namespace Bars.Gkh1468.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Modules.Gkh1468.Entities;

    public class PublicServiceOrgController : B4.Alt.DataController<PublicServiceOrg>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("PublicServiceOrgDataExport");
            return export != null ? export.ExportData(baseParams) : null;
        }
    }
}