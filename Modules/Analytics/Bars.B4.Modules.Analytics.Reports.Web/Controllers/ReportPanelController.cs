namespace Bars.B4.Modules.Analytics.Reports.Web.Controllers
{
    using Bars.B4.Modules.Analytics.Reports.Domain;

    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Контроллер панели отчетов
    /// </summary>
    public class ReportPanelController : BaseController
    {
        /// <summary>
        /// Поиск отчета
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult Search(BaseParams baseParams)
        {
            var query = baseParams.Params.GetAs("query", string.Empty);
            var panelService = Container.Resolve<IReportPanelService>();
            var result = panelService.Search(query);
            Container.Release(panelService);
            return new JsonGetResult(result);
        }
    }
}
