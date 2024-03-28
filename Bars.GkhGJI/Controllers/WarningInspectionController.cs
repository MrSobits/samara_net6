namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.Entities;

    public class WarningInspectionController<T> : FileStorageDataController<T>
        where T: WarningInspection
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("WarningInspectionDataExport");
            using (this.Container.Using(export))
            {
                return export?.ExportData(baseParams);
            }
        }
    }
}