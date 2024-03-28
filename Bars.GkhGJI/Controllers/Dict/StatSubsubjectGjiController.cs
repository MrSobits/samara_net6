namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using B4;
    using Entities;
    using DomainService;

    /// <summary>
    /// Контроллер подтематик обращения
    /// </summary>
    public class StatSubsubjectGjiController : B4.Alt.DataController<StatSubsubjectGji>
    {
        /// <summary>
        /// добавление тематик
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult AddSubject(BaseParams baseParams)
        {
            var result = Container.Resolve<IStatSubsubjectGjiService>().AddSubject(baseParams);
            return result.Success ? new JsonNetResult(new {success = true}) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// добавление характеристик нарушений
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult AddFeature(BaseParams baseParams)
        {
            var result = Container.Resolve<IStatSubsubjectGjiService>().AddFeature(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}