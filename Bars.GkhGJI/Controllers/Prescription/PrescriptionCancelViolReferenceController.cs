namespace Bars.GkhGji.Controllers.Prescription
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class PrescriptionCancelViolReferenceController : B4.Alt.DataController<PrescriptionCancelViolReference>
    {
        public ActionResult AddPrescriptionCancelViolReference(BaseParams baseParams)
        {
            var result = Container.Resolve<IPrescriptionCancelViolReferenceService>().AddPrescriptionCancelViolReference(baseParams);
            return result.Success ? new JsonNetResult(new { succes = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}
