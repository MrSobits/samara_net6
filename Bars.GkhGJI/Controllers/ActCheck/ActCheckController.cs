namespace Bars.GkhGji.Controllers
{
    using System.Collections;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    using Microsoft.AspNetCore.Mvc;

    public class ActCheckController : ActCheckController<ActCheck>
    {
    }

    public class ActCheckController<T> : B4.Alt.DataController<T>
        where T : ActCheck
    {

        public IActCheckService ActCheckService { get; set; }
        public ActionResult GetInfo(BaseParams baseParams)
        {
            var service = Container.Resolve<IActCheckService>();

            try
            {
                var result = service.GetInfo(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }
        public ActionResult AddActCheckControlMeasures(BaseParams baseParams)
        {
            var result = this.ActCheckService.AddActCheckControlMeasures(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult ListView(BaseParams baseParams)
        {
            var service = Container.Resolve<IActCheckService>();

            try
            {
                var result = (ListDataResult)service.ListView(baseParams);
                return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult ListForStage(BaseParams baseParams)
        {
            var service = Container.Resolve<IActCheckService>();

            try
            {
                var result = (ListDataResult)service.ListForStage(baseParams);
                return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult Export(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActCheckService>();

            using (this.Container.Using(service))
            {
                return service.Export(baseParams);
            }
        }
    }
}