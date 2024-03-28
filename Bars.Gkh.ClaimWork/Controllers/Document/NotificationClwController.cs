namespace Bars.Gkh.ClaimWork.Controllers.Document
{
   using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.Modules.DataExport.Domain;
    using B4.Modules.FileStorage;
    using Modules.ClaimWork.Entities;

    public class NotificationClwController : FileStorageDataController<NotificationClw>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("NotificationExport");
            try
            {
                return export.ExportData(baseParams);
            }
            finally
            {
                Container.Release(export);
            }
        } 
    }
}