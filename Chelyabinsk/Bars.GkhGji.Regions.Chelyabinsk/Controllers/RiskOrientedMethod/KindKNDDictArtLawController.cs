namespace Bars.GkhGji.Regions.Chelyabinsk.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Entities;
    using Bars.B4;
    using DomainService;
    using System.Collections;


    public class KindKNDDictArtLawController : B4.Alt.DataController<KindKNDDictArtLaw>
    {
        public IKindKNDDictArtLawService service { get; set; }
        public ActionResult AddArticleLaw(BaseParams baseParams)
        {
            var result = service.AddArticleLaw(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetListArticleLaw(BaseParams baseParams)
        {
            int totalCount;
            var result = service.GetListArticleLaw(baseParams, true, out totalCount);

            return result.Success ? new JsonListResult((IList)result.Data, totalCount) : JsFailure(result.Message);
        }
    }
}