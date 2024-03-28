namespace Bars.Gkh.RegOperator.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using B4;
    using DomainService.RegoperatorParams;

    public class RegoperatorParamsController : BaseController
    {
        public IRegoperatorParamsService Service { get; set; }

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

        public ActionResult ValidateSaveParams(BaseParams baseParams)
        {
            var result = Service.ValidateSave(baseParams);
            return result.Success ? JsSuccess(result.Data) : JsFailure(result.Message);
        }
    }
}