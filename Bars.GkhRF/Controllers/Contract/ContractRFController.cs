using System.Collections;

namespace Bars.GkhRf.Controllers
{
    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhRf.DomainService;
    using Bars.GkhRf.Entities;

    using Microsoft.AspNetCore.Mvc;

    public class ContractRfController : FileStorageDataController<ContractRf>
    {
        public ActionResult ActualRealityObjectList(BaseParams baseParams)
        {
            var result = (ListDataResult)Resolve<IContractRfService>().ActualRealityObjectList(baseParams);
            return new JsonNetResult(new { success = true, data = result.Data, totalCount = result.TotalCount });
        }

        public ActionResult CheckAvailableRealObj(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IContractRfService>().CheckAvailableRealObj(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult ListByManOrg(BaseParams baseParams)
        {
            var result = (ListDataResult)Resolve<IContractRfService>().ListByManOrg(baseParams);
            return new JsonNetResult(new { success = true, data = result.Data, totalCount = result.TotalCount });
        }

        public ActionResult ListByManOrgAndContractDate(BaseParams baseParams)
        {
            var result = (ListDataResult)Resolve<IContractRfService>().ListByManOrgAndContractDate(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("ContractRfDataExport");
            return export != null ? export.ExportData(baseParams) : null;
        }
    }
}
