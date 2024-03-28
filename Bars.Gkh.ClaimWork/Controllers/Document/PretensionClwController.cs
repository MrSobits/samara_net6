namespace Bars.Gkh.Controllers.Document
{
   using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.Modules.DataExport.Domain;
    using B4.Modules.FileStorage;
    using ClaimWork.Entities;
    using Modules.ClaimWork.DomainService;

    public class PretensionClwController : FileStorageDataController<PretensionClw>
    {
        public ActionResult GetDebtPersAccPayments(BaseParams baseParams)
        {
            var claimWorkInfoService = Container.Resolve<IClaimWorkInfoService>();

            try
            {
                return new JsonNetResult(claimWorkInfoService.GetDebtPersAccPayments(baseParams));
            }
            finally
            {
                Container.Release(claimWorkInfoService);
            }
        }

        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("PretensionExport");
            try
            {
                return export.ExportData(baseParams);
            }
            finally
            {
                Container.Release(export);
            }
        }
    }
}