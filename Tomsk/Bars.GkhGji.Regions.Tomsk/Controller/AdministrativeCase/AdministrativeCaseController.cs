namespace Bars.GkhGji.Regions.Tomsk.Controller
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.DomainService;
    using Bars.GkhGji.Regions.Tomsk.DomainService;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    public class AdministrativeCaseController : B4.Alt.DataController<AdministrativeCase>
    {
        public IAdminCaseService AdminCaseService { get; set; }

        public IBlobPropertyService<AdministrativeCase, AdministrativeCaseDescription> DescriptionService { get; set; }

        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("AdminCaseDataExport");
            return export != null ? export.ExportData(baseParams) : null;
        }

        public ActionResult GetInfo(BaseParams baseParams)
        {
            var result = AdminCaseService.GetInfo(baseParams);
            return JsSuccess(result.Data);
        }

        public ActionResult ListView(BaseParams baseParams)
        {
            var result = (ListDataResult)AdminCaseService.ListView(baseParams);
            return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
        }

        public virtual ActionResult SaveDescription(BaseParams baseParams)
        {
            var result = this.DescriptionService.Save(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public virtual ActionResult GetDescription(BaseParams baseParams)
        {
            var result = this.DescriptionService.Get(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}