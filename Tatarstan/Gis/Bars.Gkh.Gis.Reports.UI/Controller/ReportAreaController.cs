namespace Bars.Gkh.Gis.Reports.UI.Controller
{
    using Bars.B4;

    using DomainService;
    using Entities;

    using Microsoft.AspNetCore.Mvc;

    // TODO: Проверить функционал(из-за смены базового контроллера)
    public class ReportAreaController : B4.Alt.DataController<ReportArea>
    {
        public ActionResult ListWithoutPaging(BaseParams baseParams)
        {
            var result = (ListDataResult)this.Container.Resolve<IReportAreaService>().ListWithoutPaging(baseParams);

            if (result.Success)
            {
                return new JsonNetResult(new { success = true, data = result.Data, totalCount = result.TotalCount });
            }

            return JsonNetResult.Message(result.Message);
        }
    }
}
