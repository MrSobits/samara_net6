namespace Bars.GkhGji.Regions.Saha.Controllers
{
   using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Saha.Entities;

    public class ActCheckRealityObjectController : Bars.GkhGji.Controllers.ActCheckRealityObjectController
    {
        public IBlobPropertyService<ActCheckRealityObject, ActCheckRoLongDescription> LongTextService { get; set; }

        public virtual ActionResult SaveDescription(BaseParams baseParams)
        {
            var result = this.LongTextService.Save(baseParams);

            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public virtual ActionResult GetDescription(BaseParams baseParams)
        {
            var result = this.LongTextService.Get(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}