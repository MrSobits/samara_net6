namespace Bars.GkhGji.Regions.Tatarstan.Controller
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using Bars.B4.IoC;
    using DomainService;

    public class GisGmpParamsController : BaseController
    {
        public ActionResult GetParams()
        {
            var config = Resolve<IGjiTatParamService>();

            using (Container.Using(config))
            {
                var result = config.GetConfig();

                return new JsonNetResult(result);
            }
        }

        public ActionResult SaveParams(BaseParams baseParams)
        {
            var result = Resolve<IGjiTatParamService>().SaveConfig(baseParams);

            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }

        public ActionResult SaveDict(BaseParams baseParams)
        {
            var result = this.Resolve<IGjiTatParamService>().SaveDict(baseParams);

            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }
    }
}