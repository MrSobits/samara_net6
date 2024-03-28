namespace Bars.GkhGji.Regions.Tomsk.Controller
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.Regions.Tomsk.DomainService;

    public class RegionSpecificController : BaseController
    {
        public IRegionSpecificService Service { get; set; }

        public ActionResult GetAppealCitizenResponder(BaseParams baseParams)
        {
            var result = Service.GetAppealCitizenResponder(baseParams);

            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult CreateNewBaseStatment(BaseParams baseParams)
        {
            var result = Service.CreateNewBaseStatment(baseParams);

            return result.Success ?
                new JsonNetResult(new { success = result.Success, message = result.Message, data = result.Data }) { ContentType = "text/html; charset=utf-8" } :
                JsonNetResult.Failure(result.Message);
        }

        public ActionResult CreateAnswerAddressee(BaseParams baseParams)
        {
            var result = Service.CreateAnswerAddressee(baseParams);

            return result.Success ? new JsonNetResult(result.Success) : JsonNetResult.Failure(result.Message);
        }
    }
}