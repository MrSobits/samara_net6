namespace Bars.GkhGji.Regions.Tomsk.Controller
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.Regions.Tomsk.DomainService;

    public class DisposalProvidedDocNumController : BaseController
    {
        public IDisposalProvidedDocNumService Service { get; set; }

        public ActionResult AddProvideDocNum(BaseParams baseParams)
        {
            var result = Service.AddProvideDocNum(baseParams);

            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        } 
    }
}
