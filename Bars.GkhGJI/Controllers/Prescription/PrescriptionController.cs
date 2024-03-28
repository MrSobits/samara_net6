namespace Bars.GkhGji.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGji.DomainService;

    public class PrescriptionController : PrescriptionController<Entities.Prescription>
    {
    }

    public class PrescriptionController<T> : B4.Alt.DataController<T>
        where T : Entities.Prescription
    {
        public ActionResult GetInfo(long? documentId)
        {
            var service = Container.Resolve<IPrescriptionService>();

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
            var service = Container.Resolve<IPrescriptionService>();
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
            var service = Container.Resolve<IPrescriptionService>();
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
            var service = this.Container.Resolve<IPrescriptionService>();

            using (this.Container.Using(service))
            {
                return service.Export(baseParams);
            }
        }
    }
}