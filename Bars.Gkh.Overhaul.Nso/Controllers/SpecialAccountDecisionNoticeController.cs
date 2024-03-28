namespace Bars.Gkh.Overhaul.Nso.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Overhaul.Nso.DomainService;
    using Bars.Gkh.Overhaul.Nso.Entities;

    public class SpecialAccountDecisionNoticeController : FileStorageDataController<SpecialAccountDecisionNotice>
    {
        public ActionResult ListRegister(BaseParams baseParams)
        {
            var result = (ListDataResult)Container.Resolve<IDecisionNoticeService>().ListRegister(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("DecisionNoticeExport");
            return export != null ? export.ExportData(baseParams) : null;
        }
    }
}