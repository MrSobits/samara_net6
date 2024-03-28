namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class BusinessActivityController : FileStorageDataController<BusinessActivity>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("BusinessActivityDataExport");

            if (export != null)
            {
                return export.ExportData(baseParams);
            }

            return null;
        }

        public ActionResult CheckDateNotification(BaseParams baseParams)
        {
            var result = Container.Resolve<IBusinessActivityService>().CheckDateNotification(baseParams);
            return new JsonNetResult(result.Data);
        }
    }
}