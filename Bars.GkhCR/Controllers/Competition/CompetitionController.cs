using Bars.B4.Modules.DataExport.Domain;

namespace Bars.GkhCr.Controllers
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;

    using Microsoft.AspNetCore.Mvc;

    public class CompetitionController : FileStorageDataController<Competition>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("CompetitionDataExport");
            return export != null ? export.ExportData(baseParams) : null;
        }
    }
}
