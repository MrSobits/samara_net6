namespace Bars.Gkh.Gji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using B4;
    using Bars.Gkh.Gji.DomainService;

    public class GjiParamsController : BaseController
    {
        public IGjiParamsService Service { get; set; }

        public ActionResult GetParams()
        {
            var data = Service.GetParams();
            return data.Success ? new JsonNetResult(data) : JsonNetResult.Failure(data.Message);
        }

        public ActionResult GetParamByKey(BaseParams baseParams)
        {
            var data = Service.GetParamByKey(baseParams.Params.GetAs<string>("key"));

            return JsSuccess(data);
        }

        public ActionResult SaveParams(BaseParams baseParams)
        {
            var data = Service.SaveParams(baseParams);
            return data.Success ? new JsonNetResult(data) : JsonNetResult.Failure(data.Message);
        }
    }
}