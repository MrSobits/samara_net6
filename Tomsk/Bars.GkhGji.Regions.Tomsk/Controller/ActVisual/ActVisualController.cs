namespace Bars.GkhGji.Regions.Tomsk.Controller
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using System.Collections;
    using Bars.B4.Modules.DataExport.Domain;
    using DomainService;
    using Entities;

    public class ActVisualController : B4.Alt.DataController<ActVisual>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("ActVisualDataExport");
            return export != null ? export.ExportData(baseParams) : null;
        }

        public ActionResult GetInfo(BaseParams baseParams)
        {
            var result = Container.Resolve<IActVisualService>().GetInfo(baseParams);
            return JsSuccess(result.Data);
        }

        public ActionResult ListView(BaseParams baseParams)
        {
            var result = (ListDataResult)Container.Resolve<IActVisualService>().ListView(baseParams);
            return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
        }
    }
}