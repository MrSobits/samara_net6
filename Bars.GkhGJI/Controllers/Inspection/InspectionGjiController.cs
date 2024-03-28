namespace Bars.GkhGji.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.InspectionRules;

    public class InspectionGjiController : InspectionGjiController<InspectionGji>
    {
    }

    public class InspectionGjiController<T> : B4.Alt.DataController<T>
        where T : InspectionGji
    {
        public ActionResult CreateDocument(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IInspectionGjiProvider>();
            try
            {
                var result = service.CreateDocument(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally 
            {
                this.Container.Release(service);
            }
        }

        public ActionResult GetListRules(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IInspectionGjiProvider>();
            try
            {
                var result = service.GetRules(baseParams);
                return result.Success ? new JsonListResult((IEnumerable)result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        public ActionResult ListContragentRisk(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IInspectionBaseContragentService>();
            using (this.Container.Using(service))
            {
                return service.ListContragentRisk(baseParams).ToJsonResult();
            }
        }
    }
}