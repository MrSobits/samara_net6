namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using B4;

    using Bars.B4.IoC;

    using DomainService;
    using Entities;

    public class PresentationController : PresentationController<Presentation>
    {
    }

    public class PresentationController<T> : B4.Alt.DataController<T>
        where T : Presentation
    {
        public ActionResult GetInfo(BaseParams baseParams)
        {
            var service = Container.Resolve<IPresentationService>();

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

        public ActionResult Export(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IPresentationService>();

            using (this.Container.Using(service))
            {
                return service.Export(baseParams);
            }
        }
    }
}