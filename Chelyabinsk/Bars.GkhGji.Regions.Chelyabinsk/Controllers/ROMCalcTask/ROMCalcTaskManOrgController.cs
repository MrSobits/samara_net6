namespace Bars.GkhGji.Regions.Chelyabinsk.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Entities;

    using Bars.B4;
    using DomainService;
    using B4.Modules.DataExport.Domain;

    public class ROMCalcTaskManOrgController : B4.Alt.DataController<ROMCalcTaskManOrg>
    {
        public IROMCalcTaskManOrgService service { get; set; }
        public ActionResult AddManOrg(BaseParams baseParams)
        {
            var result = service.AddManOrg(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult Export(BaseParams baseParams)
        {
            IDataExportService export = null;
            try
            {
                export = this.Container.Resolve<IDataExportService>("RiskOrientedMethodDataExport");
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
    }
}