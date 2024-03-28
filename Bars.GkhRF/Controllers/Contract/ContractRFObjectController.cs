namespace Bars.GkhRf.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhRf.DomainService;
    using Bars.GkhRf.Entities;

    public class ContractRfObjectController : B4.Alt.DataController<ContractRfObject>
    {
        public ActionResult AddContractObjects(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IContractRfObjectService>().AddContractObjects(baseParams);
            if (result.Success)
            {
                return new JsonNetResult(new { success = true });
            }

            return JsonNetResult.Failure(result.Message);
        }

        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("ContractRfObjectDataExport");
            return export != null ? export.ExportData(baseParams) : null;
        }
    }
}