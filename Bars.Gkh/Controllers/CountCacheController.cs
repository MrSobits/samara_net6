namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Utils.Caching;

    /// <summary>
    /// Контроллер для с кэшем счетчиков
    /// </summary>
    public class CountCacheController : BaseController
    {
        /// <summary>
        /// Инвалидировать кэш счетчиков
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат</returns>
        public ActionResult Invalidate(BaseParams baseParams)
        {
            CountCacheHelper.ClearCache(baseParams.Params.GetAs<string>("key"));
            return this.JsSuccess();
        }
    }
}