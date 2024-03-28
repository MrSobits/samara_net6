namespace Bars.Gkh.Overhaul.Hmao.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Overhaul.Hmao.DomainService;

    public class OverhaulHmaoScriptsController : BaseController
    {
        public IOverhaulHmaoScriptsService Service { get; set; }

        public ActionResult UpdateStructElements()
        {
            var result = Service.UpdateStructElements();
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult Export(BaseParams baseParams)
        {
            IDataExportService export = null;
            try
            {
                export = this.Container.Resolve<IDataExportService>("EconFeasibilityCalcResultExport");
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
