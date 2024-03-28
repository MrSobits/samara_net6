namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.Gkh.Config;

    public class GkhParamsController : BaseController
    {
        public IGkhParams ParamsService { get; set; }

        public ActionResult GetParams(BaseParams baseParams)
        {
            //var result = ParamsService.GetParams();
            return new JsonNetResult(ParamsService.GetParams());
        }

        public ActionResult SaveParams(BaseParams baseParams)
        {
            return new JsonNetResult(ParamsService.SaveParams(baseParams));
        }

        public ActionResult Get(BaseParams baseParams)
        {
            var paramName = baseParams.Params.GetAs<string>("paramName", ignoreCase: true);

            if (string.IsNullOrEmpty(paramName))
            {
                return new JsonNetResult(false);
            }

            return new JsonNetResult(ParamsService.GetParams().GetAs<bool>(paramName));
        }
    }
}