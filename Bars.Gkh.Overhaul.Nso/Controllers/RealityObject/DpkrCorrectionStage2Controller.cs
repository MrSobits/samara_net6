namespace Bars.Gkh.Overhaul.Nso.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using DomainService;

    public class DpkrCorrectionStage2Controller : B4.Alt.DataController<DpkrCorrectionStage2>
    {
        public IDpkrCorrectionService Service { get; set; }

        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("DpkrCorrectionStage2Export");
            return export != null ? export.ExportData(baseParams) : null;
        }

        public ActionResult ChangeNumber(BaseParams baseParams)
        {
            var result = Service.ChangeNumber(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }
    }
}