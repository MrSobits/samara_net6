namespace Bars.GkhGji.Regions.Nso.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.Regions.Nso.DomainService;
    using Bars.GkhGji.Regions.Nso.Entities;

    public class ActRemovalProvidedDocController : B4.Alt.DataController<ActRemovalProvidedDoc>
    {
        public ActionResult AddProvidedDocuments(BaseParams baseParams)
        {
            var service = Container.Resolve<IActRemovalProvidedDocService>();
            try
            {
                var result = service.AddProvidedDocs(baseParams);
                return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
            }
            finally 
            {
                Container.Release(service);
            }
            
        }
    }
}