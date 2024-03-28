namespace Bars.GkhCr.Controllers.ObjectCr
{
    using Bars.B4;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;

    using Microsoft.AspNetCore.Mvc;

    public class SpecialQualificationController : B4.Alt.DataController<SpecialQualification>
    {
        public ActionResult GetActiveColumns(BaseParams baseParams)
        {
            return new JsonNetResult(this.Container.Resolve<ISpecialQualificationService>().GetActiveColumns(baseParams));
        }
    }
}
