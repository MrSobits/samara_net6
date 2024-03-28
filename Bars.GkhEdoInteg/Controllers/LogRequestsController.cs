namespace Bars.GkhEdoInteg.Controllers
{
    using Bars.B4;
    using Bars.GkhEdoInteg.DomainService;
    using Bars.GkhEdoInteg.Entities;

    using Microsoft.AspNetCore.Mvc;

    public class LogRequestsController : B4.Alt.DataController<LogRequests>
    {
        public virtual ActionResult ListLogRequests(BaseParams baseParams)
        {
            var domainService = Container.Resolve<IAppealCitsEdoIntegService>();
            var listResult = (ListDataResult)domainService.ListLogRequests(baseParams);
            return new JsonNetResult(new { success = true, data = listResult.Data, totalCount = listResult.TotalCount });
        }

        public virtual ActionResult ListRequestsAppealCits(BaseParams baseParams)
        {
            var domainService = Container.Resolve<IAppealCitsEdoIntegService>();
            var listResult = (ListDataResult)domainService.ListRequestsAppealCits(baseParams);
            return new JsonNetResult(new { success = true, data = listResult.Data, totalCount = listResult.TotalCount });
        }
    }
}
