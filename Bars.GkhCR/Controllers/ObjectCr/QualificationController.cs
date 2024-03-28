namespace Bars.GkhCr.Controllers.ObjectCr
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;

    public class QualificationController : B4.Alt.DataController<Qualification>
    {
        public ActionResult GetActiveColumns(BaseParams baseParams)
        {
            return new JsonNetResult(Container.Resolve<IQualificationService>().GetActiveColumns(baseParams));
        }

        public ActionResult ListView(BaseParams baseParams)
        {
            var listResult = (ListDataResult)Container.Resolve<IQualificationService>().ListView(baseParams);
            return new JsonNetResult(new { success = true, data = listResult.Data, totalCount = listResult.TotalCount });
        }

        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("QualificationDataExport");
            return export != null ? export.ExportData(baseParams) : null;
        }
    }
}
