namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using B4;
    using DomainService;
    using Entities;

    public class BasePlanActionController : BasePlanActionController<BasePlanAction>
    {
    }

    public class BasePlanActionController<T> : B4.Alt.DataController<T>
        where T : BasePlanAction
    {
        public ActionResult GetContragentInfo(BaseParams baseParams)
        {
            var service = Container.Resolve<IBasePlanActionService>();

            try
            {
                var result = service.GetContragentInfo(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally 
            {
                Container.Release(service);
            }
        }

        public ActionResult GetStartFilters()
        {
            var service = Container.Resolve<IBasePlanActionService>();

            try
            {
                var result = service.GetStartFilters();
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        /*
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("BaseInsCheckDataExport");
            return export != null ? export.ExportData(baseParams) : null;
        }
         */
    }
}