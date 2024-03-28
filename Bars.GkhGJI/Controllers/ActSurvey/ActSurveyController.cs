namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class ActSurveyController : ActSurveyController<ActSurvey>
    {
    }

    public class ActSurveyController<T> : B4.Alt.DataController<T>
        where T : ActSurvey
    {
        public ActionResult GetInfo(long? documentId)
        {
            var service = Container.Resolve<IActSurveyService>();
            try
            {
                var result = service.GetInfo(documentId);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally 
            {
                Container.Release(service);
            }
        }

        public ActionResult ListView(BaseParams baseParams)
        {
            var service = Container.Resolve<IActSurveyService>();
            try
            {
                var totalCount = 0;
                var result = service.ListView(baseParams, true, ref totalCount);
                return new JsonListResult(result, totalCount);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("ActSurveyDataExport");

            try
            {
                if (export != null)
                {
                    return export.ExportData(baseParams);
                }

                return null;
            }
            finally 
            {
                Container.Release(export);
            }
        }
    }
}
