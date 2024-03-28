namespace Bars.GkhDi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;

    using Bars.B4.Modules.DataExport.Domain;

    using Entities;

    public class WorkToController : B4.Alt.DataController<WorkTo>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("WorkToDataExport");

            if (export != null)
            {
                return export.ExportData(baseParams);
            }

            return null;
        }
    }
}
