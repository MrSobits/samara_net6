namespace Bars.Gkh.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.Gkh.DomainService.GkhParam;

    [Obsolete("Использовать объект GkhConfigParam")]
    public class GkhParamController : BaseController
    {
        public IGkhParamService Service { get; set; }

        public ActionResult GetParams(BaseParams baseParams)
        {
            var prefix = baseParams.Params.GetAs<string>("prefix");

            var data = Service.GetParams(prefix);
            return data.Success ? new JsonNetResult(data) : JsonNetResult.Failure(data.Message);
        }

        public ActionResult GetClientParams(BaseParams baseParams)
        {
            var data = Service.GetClientParams();
            return data.Success ? new JsonNetResult(data) : JsonNetResult.Failure(data.Message);
        }

        public ActionResult GetParamByKey(BaseParams baseParams)
        {
            var key = baseParams.Params.GetAs<string>("key");
            var prefix = baseParams.Params.GetAs<string>("prefix");

            var data = Service.GetParamByKey(key, prefix);

            return JsSuccess(data);
        }

        public ActionResult SaveParams(BaseParams baseParams)
        {
            var data = Service.SaveParams(baseParams);
            return data.Success ? new JsonNetResult(data) : JsonNetResult.Failure(data.Message);
        }
    }
}