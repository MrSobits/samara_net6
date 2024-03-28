namespace Bars.Gkh.Overhaul.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.DomainService;

    public class CreditOrgController : B4.Alt.DataController<CreditOrg>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("CreditOrgExport");
            return export != null ? export.ExportData(baseParams) : null;
        }

        public ActionResult ListExceptChildren(BaseParams baseParams)
        {
            var result = (ListDataResult)Resolve<ICreditOrgService>().ListExceptChildren(baseParams);
            return new JsonListResult((IList) result.Data, result.TotalCount);
        }
    }
}