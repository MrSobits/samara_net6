namespace Bars.GkhGji.Regions.Chelyabinsk.Controllers
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
        public ActionResult GetInfo(BaseParams baseParams)
        {
            try
            {
                var result = service.GetInfo(baseParams);
                if (result.Success)
                {
                    return new JsonNetResult(result.Data);
                }

                return JsonNetResult.Failure(result.Message);
            }
            finally
            {
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

        public ActionResult AddDocs(BaseParams baseParams)
        {

            try
            {
                var result = service.AddDocs(baseParams);
                if (result.Success)
                {
                    return new JsonNetResult(new { success = true });
                }

                return JsonNetResult.Failure(result.Message);
            }
            finally
            {
            }
        }

        public ActionResult ListDocs(BaseParams baseParams)
        {
            try
            {
                var result = (ListDataResult)service.ListDocs(baseParams);
                return new JsonListResult((IList)result.Data, result.TotalCount);
            }
            finally
            {
            }
        }


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

        public ActionResult GetListDecision(BaseParams baseParams)
        {
            return service.GetListDecision(baseParams).ToJsonResult();
        }

        public ActionResult GetListAppealDecision(BaseParams baseParams)
        {
            return service.GetListAppealDecision(baseParams).ToJsonResult();
        }

        public ActionResult GetListAppealCitsDefinition(BaseParams baseParams)
        {
            return service.GetListAppealCitsDefinition(baseParams).ToJsonResult();
        }
        public ActionResult GetListResolutionDefinition(BaseParams baseParams)
        {
            return service.GetListResolutionDefinition(baseParams).ToJsonResult();
        }
    }

}