namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using B4;
    using B4.Modules.DataExport.Domain;
    using Entities;

    public class ActivityTsjController : B4.Alt.DataController<ActivityTsj>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("ActivityTsjDataExport");

            if (export != null)
            {
                return export.ExportData(baseParams);
            }

            return null;
        }
    }
}