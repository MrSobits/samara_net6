namespace Bars.Gkh.Gis.Reports.UI.Controller
{
    using B4;
    using DomainService;
    using Entities;

    using Microsoft.AspNetCore.Mvc;

    // TODO: Проверить функционал(из-за смены базового контроллера)
    public class ReportMunicipalityController : B4.Alt.DataController<ReportMunicipality>
    {
        public ActionResult ListWithoutPaging(BaseParams baseParams)
        {
            var result = (ListDataResult)this.Container.Resolve<IReportMunicipalityService>().ListWithoutPaging(baseParams);

            if (result.Success)
            {
                return new JsonNetResult(new { success = true, data = result.Data, totalCount = result.TotalCount });
            }

            return JsonNetResult.Message(result.Message);
        }
    }
}
