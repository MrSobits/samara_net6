namespace Bars.GkhGji.Regions.BaseChelyabinsk.Controllers.ActRemoval
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.Regions.BaseChelyabinsk.DomainService;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;

    public class ActRemovalProvidedDocController : B4.Alt.DataController<ActRemovalProvidedDoc>
    {
        public ActionResult AddProvidedDocuments(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActRemovalProvidedDocService>();
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