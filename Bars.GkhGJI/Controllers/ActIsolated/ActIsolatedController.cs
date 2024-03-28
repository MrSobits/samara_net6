namespace Bars.GkhGji.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class ActIsolatedController : ActIsolatedController<ActIsolated>
    {
    }

    public class ActIsolatedController<T> : B4.Alt.DataController<T>
        where T : ActIsolated
    {
        public ActionResult GetInfo(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActIsolatedService>();

            try
            {
                var result = service.GetInfo(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally 
            {
                this.Container.Release(service);
            }
        }

        public ActionResult ListView(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActIsolatedService>();

            try
            {
                var result = (ListDataResult)service.ListView(baseParams);
                return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }
        
        public ActionResult Export(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("ActIsolatedDataExport");

            try
            {
                return export?.ExportData(baseParams);
            }
            finally 
            {
                this.Container.Release(export);
            }
        }

        public ActionResult ListForStage(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActIsolatedService>();

            try
            {
                var result = (ListDataResult)service.ListForStage(baseParams);
                return result.Success 
                    ? new JsonListResult((IList)result.Data, result.TotalCount) 
                    : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }
    }
}