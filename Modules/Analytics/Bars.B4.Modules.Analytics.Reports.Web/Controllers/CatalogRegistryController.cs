namespace Bars.B4.Modules.Analytics.Reports.Web.Controllers
{
    using Bars.B4.Modules.Analytics.Reports.Params;

    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// 
    /// </summary>
    public class CatalogRegistryController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult List(BaseParams baseParams)
        {
            return JsSuccess(CatalogRegistry.GetAll());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult Get(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<string>("id", ignoreCase: true);
            return JsSuccess(CatalogRegistry.Get(id));
        }
    }
}
