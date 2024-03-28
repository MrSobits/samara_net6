using Bars.B4;
using Bars.Gkh.Domain;
using Bars.Gkh.Entities;
using Bars.GkhGji.Regions.Habarovsk.DomainService;
using Microsoft.AspNetCore.Mvc;
using Bars.GkhGji.Regions.Habarovsk.Entities;
using Bars.B4.Modules.DataExport.Domain;

namespace Bars.GkhGji.Regions.Habarovsk.Controllers
{
    class VDGOViolatorsController : B4.Alt.DataController<VDGOViolators>
    {
        public IVDGOViolatorsService service { get; set; }

        public ActionResult Export(BaseParams baseParams)
        {
            IDataExportService export = null;
            try
            {
                export = this.Container.Resolve<IDataExportService>("VDGOViolatorsDataExport");
                return export.ExportData(baseParams);
            }
            finally
            {
                if (export != null)
                {
                    this.Container.Release(export);
                }
            }
        }

        public ActionResult GetListRO(BaseParams baseParams)
        {
            return service.GetListRO(baseParams).ToJsonResult();
        }

        public ActionResult GetListMinOrgContragent(BaseParams baseParams)
        {
            return service.GetListMinOrgContragent(baseParams).ToJsonResult();
        }
    }
}
