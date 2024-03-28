using Microsoft.AspNetCore.Mvc;
using Bars.Gkh.DomainService;

namespace Bars.Gkh.Controllers
{
    using Bars.B4;

    public class GkhScriptsController : BaseController
    {
        /// <summary>
        /// Корректировка договоров
        /// </summary>
        public ActionResult CorrectJskTsjContract(BaseParams baseParams)
        {
            var result = Resolve<IGkhScriptService>().CorrectJskTsjContract(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message, result.Data);
        }

        public ActionResult CorrectContractJskTsj(BaseParams baseParams)
        {
            var result = Resolve<IGkhScriptService>().CorrectContractJskTsj(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message, result.Data);
        }

        public ActionResult CreateRelation(BaseParams baseParams)
        {
            var result = Resolve<IGkhScriptService>().CreateRelation(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message, result.Data);
        }

        public ActionResult CreateRelationSecond(BaseParams baseParams)
        {
            var result = Resolve<IGkhScriptService>().CreateRelationSecond(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message, result.Data);
        }
    }
}