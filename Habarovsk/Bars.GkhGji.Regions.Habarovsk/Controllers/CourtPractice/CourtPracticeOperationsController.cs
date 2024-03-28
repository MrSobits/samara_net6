namespace Bars.GkhGji.Regions.Habarovsk.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Entities;
    using Bars.B4;
    using DomainService;
    using Bars.GkhGji.Entities;
    using System.Collections;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Domain;

    public class CourtPracticeOperationsController : BaseController
    {
        public ICourtPracticeOperationsService service { get; set; }
        public ActionResult AddCourtPracticeRealityObjects(BaseParams baseParams)
        {
            var result = service.AddCourtPracticeRealityObjects(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
        public ActionResult Export(BaseParams baseParams)
        {
            IDataExportService export = null;
            try
            {
                export = this.Container.Resolve<IDataExportService>("CourtPracticeDataExport");
                return export.ExportData(baseParams);
            }
            finally
            {
                if (export != null)
                {
                    this.Container.Release(export);
                }
            }
        }
        public ActionResult ExportSOPR(BaseParams baseParams)
        {
            IDataExportService export = null;
            try
            {
                export = this.Container.Resolve<IDataExportService>("AppealOrderDataExport");
                return export.ExportData(baseParams);
            }
            finally
            {
                if (export != null)
                {
                    this.Container.Release(export);
                }
            }
        }
        public ActionResult ListDocsForSelect(BaseParams baseParams)
        {
           
            try
            {
                return service.ListDocsForSelect(baseParams).ToJsonResult();
            }
            finally
            {

            }
        }

        public ActionResult GetDocInfo(BaseParams baseParams)
        {
            try
            {
                var data = service.GetDocInfo(baseParams);
                return JsSuccess(data);
            }
            finally
            {

            }
        }
    }
 
}