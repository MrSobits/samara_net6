namespace Bars.GkhCr.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;

    public class EstimateCalculationController : FileStorageDataController<EstimateCalculation>
    {
        public ActionResult ListEstimateRegisterDetail(BaseParams baseParams)
        {
            return new JsonNetResult(Container.Resolve<IEstimateCalculationService>().ListEstimateRegisterDetail(baseParams));
        }

        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("EstimateCalculationDataExport");
            return export != null ? export.ExportData(baseParams) : null;
        }
    }
}