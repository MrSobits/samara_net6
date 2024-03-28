namespace Bars.GkhGji.Regions.Nso.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using Entities;
    using Gkh.DomainService;
    using GkhGji.Entities;

    public class ActCheckViolationController : GkhGji.Controllers.ActCheckViolationController
    {
        public IBlobPropertyService<ActCheckViolation, ActCheckViolationLongText> LongTextService { get; set; }

        public ActionResult GetDescription(BaseParams baseParams)
        {
            return GetBlob(baseParams);
        }

        public ActionResult SaveDescription(BaseParams baseParams)
        {
            return SaveBlob(baseParams);
        }

        private ActionResult SaveBlob(BaseParams baseParams)
        {
            var result = LongTextService.Save(baseParams);

            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        private ActionResult GetBlob(BaseParams baseParams)
        {
            var result = LongTextService.Get(baseParams);

            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}