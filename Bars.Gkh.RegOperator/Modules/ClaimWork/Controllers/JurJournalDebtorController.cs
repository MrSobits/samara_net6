namespace Bars.Gkh.RegOperator.Modules.ClaimWork.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.Modules.DataExport.Domain;
    using DomainService;

    public class JurJournalDebtorController : BaseController
    {
        public ActionResult List(BaseParams baseParams)
        {
            var service = Resolve<IJurJournalDebtorService>();
            try
            {
                int totalCount;
                var data = service.GetList(baseParams, true, out totalCount);
                return new JsonListResult(data, totalCount);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("JurJournalDebtorExport");
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