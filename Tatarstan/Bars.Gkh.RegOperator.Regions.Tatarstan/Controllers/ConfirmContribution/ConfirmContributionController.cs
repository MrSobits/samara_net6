namespace Bars.Gkh.RegOperator.Regions.Tatarstan.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.RegOperator.Regions.Tatarstan.Entities;
    
    public class ConfirmContributionController : B4.Alt.DataController<ConfirmContribution>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("ConfirmContributionExport");
            return export != null ? export.ExportData(baseParams) : null;
        }
    }
}