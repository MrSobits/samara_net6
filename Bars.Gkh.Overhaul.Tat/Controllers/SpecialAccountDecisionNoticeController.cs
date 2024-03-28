namespace Bars.Gkh.Overhaul.Tat.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Overhaul.Tat.DomainService;
    using Bars.Gkh.Overhaul.Tat.Entities;

    public class SpecialAccountDecisionNoticeController : FileStorageDataController<SpecialAccountDecisionNotice>
    {
        public ActionResult ListRegister(BaseParams baseParams)
        {
            var result = (ListDataResult)this.Container.Resolve<IDecisionNoticeService>().ListRegister(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        public ActionResult Export(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("DecisionNoticeExport");
            return export != null ? export.ExportData(baseParams) : null;
        }
    }
}