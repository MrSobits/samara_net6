namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class ActIsolatedProvidedDocController : B4.Alt.DataController<ActIsolatedProvidedDoc>
    {
        public ActionResult AddProvidedDocuments(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActIsolatedProvidedDocService>();
            try
            {
                var result = service.AddProvidedDocs(baseParams);
                return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
            }
            finally 
            {
                this.Container.Release(service);
            }
            
        }
    }
}