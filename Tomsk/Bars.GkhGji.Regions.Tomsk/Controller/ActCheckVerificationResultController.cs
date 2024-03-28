namespace Bars.GkhGji.Regions.Tomsk.Controller
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.Regions.Tomsk.DomainService;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    public class ActCheckVerificationResultController : B4.Alt.DataController<ActCheckVerificationResult>
    {
        public IActCheckVerificationResultService Service { get; set; }

        public ActionResult AddActCheckVerificationResult(BaseParams baseParams)
        {
            var result = Service.AddActCheckVerificationResult(baseParams);

            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

    }
}
