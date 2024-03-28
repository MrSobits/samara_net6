namespace Bars.GkhGji.Regions.Tatarstan.Controller.Dict
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.Regions.Tatarstan.DomainService;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    public class ControlListTypicalQuestionController : B4.Alt.DataController<ControlListTypicalQuestion>
    {
        /// <summary>
        /// Обновляет ссылку на MandatoryReq
        /// </summary>
        public ActionResult UpdateControlListTypicalQuestion(BaseParams baseParams)
        {
            var result = this.Container.Resolve<IControlListTypicalQuestionService>().UpdateControlListTypicalQuestion(baseParams);
            return result.Success
                ? new JsonNetResult(new { success = true })
                : JsonNetResult.Failure(result.Message);
        }        
    }
}
