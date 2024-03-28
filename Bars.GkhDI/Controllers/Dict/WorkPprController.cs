namespace Bars.GkhDi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;

    using Bars.B4.Modules.DataExport.Domain;

    using Entities;

    public class WorkPprController : B4.Alt.DataController<WorkPpr>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("WorkPprDataExport");

            if (export != null)
            {
                return export.ExportData(baseParams);
            }

            return null;
        }
    }
}
