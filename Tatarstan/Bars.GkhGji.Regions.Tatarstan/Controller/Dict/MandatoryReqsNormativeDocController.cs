using Bars.B4;
using Bars.GkhGji.Regions.Tatarstan.DomainService;
using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;
using Microsoft.AspNetCore.Mvc;

namespace Bars.GkhGji.Regions.Tatarstan.Controller.Dict
{
    public class MandatoryReqsNormativeDocController : B4.Alt.DataController<MandatoryReqsNormativeDoc>
    {
        /// <summary>
        /// Добавление, обновление и удаление MandatoryReqsNormativeDoc
        /// </summary>
        public ActionResult AddUpdateDeleteNpa(BaseParams baseParams)
        {
            var result = this.Container.Resolve<IMandatoryReqsNormativeDocService>().AddUpdateDeleteNpa(baseParams);
            return result.Success 
                ? new JsonNetResult(new { success = true }) 
                : JsonNetResult.Failure(result.Message);
        }
    }
}
