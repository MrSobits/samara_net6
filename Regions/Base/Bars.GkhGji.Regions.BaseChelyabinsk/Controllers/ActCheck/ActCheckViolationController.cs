namespace Bars.GkhGji.Regions.BaseChelyabinsk.Controllers.ActCheck
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActCheck;

    public class ActCheckViolationController : GkhGji.Controllers.ActCheckViolationController
    {
        public IBlobPropertyService<ActCheckViolation, ActCheckViolationLongText> LongTextService { get; set; }

        public ActionResult GetDescription(BaseParams baseParams)
        {
            return this.GetBlob(baseParams);
        }

        public ActionResult SaveDescription(BaseParams baseParams)
        {
            return this.SaveBlob(baseParams);
        }

        private ActionResult SaveBlob(BaseParams baseParams)
        {
            var result = this.LongTextService.Save(baseParams);

            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        private ActionResult GetBlob(BaseParams baseParams)
        {
            var result = this.LongTextService.Get(baseParams);

            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}